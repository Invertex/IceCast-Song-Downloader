﻿using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StreamRipper.Interfaces;
using StreamRipper.Models;
using StreamRipper.Models.Events;
using StreamRipper.Models.State;
using StreamRipper.Utilities;
using static StreamRipper.Logic.EventHandlerImpl;

namespace StreamRipper
{
    internal class StreamRipperImpl : IStreamRipper
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Flag to indicate whether task is running or not
        /// </summary>
        private CancellationTokenSource _cancellationToken;

        private readonly StreamRipperOptions _options;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public StreamRipperImpl(StreamRipperOptions options, ILogger logger)
        {
            _logger = logger;
            _options = options.Validate();

            // Initialize
            _cancellationToken = new CancellationTokenSource();
        }

        public EventHandler<MetadataChangedEventArg> MetadataChangedHandlers { get; set; }

        public EventHandler<StreamUpdateEventArg> StreamUpdateEventHandlers { get; set; }

        public EventHandler<StreamStartedEventArg> StreamStartedEventHandlers { get; set; }

        public EventHandler<StreamEndedEventArg> StreamEndedEventHandlers { get; set; }

        public EventHandler<SongChangedEventArg> SongChangedEventHandlers { get; set; }

        public EventHandler<StreamFailedEventArg> StreamFailedHandlers { get; set; }

        /// <summary>
        /// Start the streaming in async fashion
        /// </summary>
        public void Start()
        {
            if (_taskRef?.Status == TaskStatus.Running)
            {
                Dispose();
            }

            // Refresh cancellation token
            _cancellationToken = new CancellationTokenSource();

            var token = _cancellationToken.Token;

            _taskRef = Task.Factory
                .StartNew(state => StreamHttpRadio((EventState) state, token), new EventState(_options.Url.AbsoluteUri, _logger)
                {
                    MaxBufferSize = _options.MaxBufferSize,
                    CancellationToken = _cancellationToken,
                    EventHandlers = new EventHandlers
                    {
                        SongChangedEventHandlers = TypedHandler(SongChangedEventHandler) + SongChangedEventHandlers,
                        StreamEndedEventHandlers = TypedHandler(StreamEndedEventHandler) + StreamEndedEventHandlers,
                        StreamStartedEventHandlers = TypedHandler(StreamStartedEventHandler) + StreamStartedEventHandlers,
                        StreamUpdateEventHandlers = TypedHandler(StreamUpdateEventHandler) + StreamUpdateEventHandlers,
                        MetadataChangedHandlers = TypedHandler(MetadataChangedHandler) + MetadataChangedHandlers,
                        StreamFailedHandlers = TypedHandler(StreamFailedEventHandler) + StreamFailedHandlers
                    }
                }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private Task _taskRef;

        /// <summary>
        /// Stream HTTP Radio
        /// </summary>
        private static void StreamHttpRadio(EventState state, CancellationToken token)
        {
            try
            {
                HttpClient client = new()
                {
                    BaseAddress = new Uri(state.Url),
                    Timeout = TimeSpan.FromSeconds(8)
                };
                client.DefaultRequestHeaders.Add("icy-metadata", "1");

                var streamRequest = client.GetAsync("", HttpCompletionOption.ResponseHeadersRead, token);
                streamRequest.Wait(token);
                
                using (var response = streamRequest.Result)
                {
                    response.EnsureSuccessStatusCode();
                    // Trigger on stream started
                    state.EventHandlers.StreamStartedEventHandlers.Invoke(state, new StreamStartedEventArg());

                    // Get the position of metadata
                    var metaInt = 0;
                    var icyHead = response.Headers.GetValues(name: "icy-metaint").First();

                    if (!string.IsNullOrEmpty(icyHead))
                    {
                        metaInt = Convert.ToInt32(icyHead);
                    }

                    using (var socketStream = response.Content.ReadAsStream(token))
                    {
                        try
                        {
                            var buffer = new byte[(uint) Math.Pow(2, 14)];
                            var metadataLength = 0;
                            var streamPosition = 0;
                            var bufferPosition = 0;
                            var readBytes = 0;
                            var metadataSb = new StringBuilder();

                            // Loop forever
                            while (!token.IsCancellationRequested)
                            {
                                if (bufferPosition >= readBytes)
                                {
                                    if (socketStream != null)
                                    {
                                        readBytes = socketStream.Read(buffer, 0, buffer.Length);
                                    }

                                    bufferPosition = 0;
                                }

                                if (readBytes <= 0)
                                {
                                    // Stream ended
                                    state.EventHandlers.StreamEndedEventHandlers.Invoke(state, new StreamEndedEventArg());
                                    break;
                                }

                                if (metadataLength == 0)
                                {
                                    if (metaInt == 0 || streamPosition + readBytes - bufferPosition <= metaInt)
                                    {
                                        streamPosition += readBytes - bufferPosition;
                                        ProcessStreamData(state, buffer, ref bufferPosition, readBytes - bufferPosition);
                                        continue;
                                    }

                                    ProcessStreamData(state, buffer, ref bufferPosition, metaInt - streamPosition);
                                    metadataLength = Convert.ToInt32(buffer[bufferPosition++]) * 16;

                                    // Check if there's any metadata, otherwise skip to next block
                                    if (metadataLength == 0)
                                    {
                                        streamPosition = Math.Min(readBytes - bufferPosition, metaInt);
                                        ProcessStreamData(state, buffer, ref bufferPosition, streamPosition);
                                        continue;
                                    }
                                }
                                if (bufferPosition < readBytes)
                                {
                                    Span<byte> slice = buffer.AsSpan<byte>(bufferPosition, metadataLength);
                                    string metadata = Encoding.UTF8.GetString(slice);
                                   
                                    bufferPosition += metadataLength;
                                    metadataLength = 0;

                                    streamPosition = Math.Min(readBytes - bufferPosition, metaInt);
                                    ProcessStreamData(state, buffer, ref bufferPosition, streamPosition);

                                    // Trigger song change event
                                    state.EventHandlers.MetadataChangedHandlers.Invoke(state, new MetadataChangedEventArg
                                    {
                                        SongMetadata = MetadataUtility.ParseMetadata(metadata)
                                    });

                                    // Increment the count
                                    state.Count++;

                                    metadataSb.Clear();
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            // Invoke on stream ended
                            state.EventHandlers.StreamEndedEventHandlers.Invoke(state, new StreamEndedEventArg());
                            state.EventHandlers.StreamFailedHandlers.Invoke(state, new StreamFailedEventArg {Exception = e, Message = "Stream loop threw an exception"});
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Invoke on stream ended
                state.EventHandlers.StreamFailedHandlers.Invoke(state, new StreamFailedEventArg {Exception = e, Message = "Stream threw an exception"});
            }
        }

        /// <summary>
        /// Process the stream
        /// </summary>
        /// <param name="state"></param>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        private static void ProcessStreamData(EventState state, byte[] buffer, ref int offset, int length)
        {
            if (length < 1)
            {
                return;
            }

            var data = new byte[length];

            Buffer.BlockCopy(buffer, offset, data, 0, length);

            // Trigger update
            state.EventHandlers.StreamUpdateEventHandlers.Invoke(state, new StreamUpdateEventArg
            {
                SongRawPartial = data
            });

            offset += length;
        }

        /// <summary>
        /// Dispose the running task
        /// </summary>
        public void Dispose()
        {
            _cancellationToken.Cancel();
        }
    }
}
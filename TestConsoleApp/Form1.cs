using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StreamRipper.Interfaces;
using StreamRipper.Models;
using System.IO;
using StreamRipper.Extensions;
using System;
using System.Windows.Forms;
using System.Collections.Generic;
using StreamRipper.Models.Song;

namespace TestConsoleApp
{
    public partial class IceStreamForm : Form
    {
        List<string> filters = new List<string>(128);

        public IceStreamForm()
        {
            InitializeComponent();
            UpdateStatus("Waiting for start...");
        }

        private void FilterText_Changed(object sender, EventArgs e)
        {
            filters = new List<string>(filterWordsInput.Text.Split(Environment.NewLine));
            if(filters.Count == 0) {
                filterWordsInput.Text = "";
                return; }
            for(int i = filters.Count - 1; i >= 0; i--)
            {
                if(String.IsNullOrWhiteSpace(filters[i]))
                {
                    filters.RemoveAt(i);
                }
            }
            if (filters.Count == 0)
            {
                filterWordsInput.Text = "";
                return;
            }
        }

        IStreamRipper stream;

        private void Stop()
        {
            if (stream != null) { stream.Dispose(); }

            startButton.Text = "START";
            UpdateLog("STREAM STOPPED");
            UpdateStatus("Waiting to start.");
        }

        private void Start()
        {
            if (!SavePathValid()) { return; }

            startButton.Text = "STOP";

            var serviceProvider = new ServiceCollection()
                .AddLogging(cfg => cfg.AddConsole())
                .Configure<LoggerFilterOptions>(cfg => cfg.MinLevel = LogLevel.Trace)
                .AddStreamRipper()
                .BuildServiceProvider();

            var streamRipperFactory = serviceProvider.GetService<IStreamRipperFactory>();

            try
            {
                stream = streamRipperFactory.New(new StreamRipperOptions
                {
                    Url = new Uri(streamUrlBox.Text),
                    MaxBufferSize = 10 * 2000000    // stop when buffer size passes 20 megabytes
                });
            }
            catch
            {
                UpdateLog("Stream Failed to run. Ensure it's the correct type of url!", true);
                Stop();
                return;
            }
            stream.StreamFailedHandlers += StreamFailed;
            stream.StreamStartedEventHandlers += StreamStart;
            stream.SongChangedEventHandlers += SongChanged;
            stream.MetadataChangedHandlers += MetadataChanged;

            UpdateStatus("Starting Stream...");
            stream.Start();
        }

        private void Browse_Clicked(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog().ToString() == "OK")
            {
                saveLocationInput.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void Start_Clicked(object sender, EventArgs e)
        {
            if(startButton.Text == "STOP")
            {
                Stop();
                return;
            }
            Start();
        }

        private bool SavePathValid()
        {
            bool pathValid = Directory.Exists(saveLocationInput.Text);
            if (!pathValid)
            {
                UpdateLog("Save Path Invalid! Can't start.");
            }

            return pathValid;
        }

        private bool SongMatchesFilter(SongMetadata metadata)
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var filter in filters)
{
                    if (metadata.ToString().Contains(filter, StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        private void MetadataChanged(object sender, StreamRipper.Models.Events.MetadataChangedEventArg arg)
        {
            if (arg != null && arg.SongMetadata != null)
            {
                if(SongMatchesFilter(arg.SongMetadata))
                {
                    FlashWindow.Flash(this);
                    UpdateLog("Found a matching song! Will save when completed playing: " + arg.SongMetadata.ToString());
                }
                UpdateSongData(arg.SongMetadata);
            }
        }

        private void SongChanged(object sender, StreamRipper.Models.Events.SongChangedEventArg arg)
        {
            string songName = arg.SongInfo.SongMetadata.ToString();

            if (filters != null && filters.Count > 0)
            {
                if(SongMatchesFilter(arg.SongInfo.SongMetadata))
{
                    SaveSong(arg);
                }
            }
            else
            {
                SaveSong(arg);
            }
        }

        private void SaveSong(StreamRipper.Models.Events.SongChangedEventArg songArg)
        {
            UpdateLog("SAVING SONG: " + songArg.SongInfo.ToString());
            string savePath = Path.Combine(saveLocationInput.Text, $"{songArg.SongInfo.SongMetadata}.mp3");
            if (SavePathValid())
            {
                System.IO.File.WriteAllBytes(Path.Combine(saveLocationInput.Text, $"{songArg.SongInfo.SongMetadata}.mp3"), songArg.SongInfo.Stream.ToArray());
            }
        }

        private void StreamFailed(object sender, StreamRipper.Models.Events.StreamFailedEventArg arg)
        {
            UpdateLog("Stream Failed to run. Ensure it's the correct type of url!", true);
            Stop();
        }

        private void StreamStart(object sender, StreamRipper.Models.Events.StreamStartedEventArg arg)
        {
            UpdateLog("Stream Started!");
            if(arg != null && arg.SongInfo != null && arg.SongInfo.SongMetadata != null)
                UpdateSongData(arg.SongInfo.SongMetadata);
}

        private void UpdateStatus(string text)
        {
            //Take care of off-thread writing
            if (statusStrip1.InvokeRequired)
            {
                Action safeWrite = delegate { UpdateStatus($"{text}"); };
                statusStrip1.Invoke(safeWrite);
            }
            else
            {
                statusLabel.Text = text;
            }
        }

        private void UpdateLog(string text, bool setLabel = false)
        {
            if(logBox.InvokeRequired)
            {
                Action safeWrite = delegate { UpdateLog($"{text}"); };
                logBox.Invoke(safeWrite);
            }
            else
            {
                logBox.AppendText(Environment.NewLine + text);
            }
            if(setLabel)
            {
                UpdateStatus(text);
            }
        }

        private void UpdateSongData(SongMetadata songData)
        {
            if (songData != null && !string.IsNullOrEmpty(songData.Title))
            {
                UpdateStatus("Now Playing: " + songData.ToString());
            }
        }

        private void onFormClosing(object sender, FormClosingEventArgs e)
        {
            if(stream!= null) { stream?.Dispose(); }
        }
    }
}

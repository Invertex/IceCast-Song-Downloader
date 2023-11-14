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
using System.Text.RegularExpressions;
using Invertex.Properties;

namespace Invertex
{
	//bllop
	//extra bloop
    /// <summary>
    /// Main Form for program
    /// </summary>
    public partial class IceStreamForm : Form
    {
        private IStreamRipper stream;

        private List<string> filters = new List<string>(128);
        private readonly string regSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        private readonly Regex rg;

        private int reconnectCount = 0;
        private bool successfullyConnected = false;
        private bool connected = false;

        private bool saveCurrentlyPlaying = false;
        private string currentSong = "";

        private bool SaveCurrentlyPlaying
        {
            get => saveCurrentlyPlaying;
            set {
                saveCurrentlyPlaying = value;
                SetSaveSongButtonState(value);
            }
            
        }

        private bool HasFilters { get => (filters != null && filters.Count > 0); }

        /// Create new program form
        public IceStreamForm()
        {
            rg = new Regex(string.Format("[{0}]", Regex.Escape(regSearch)));
            InitializeComponent();
            LoadSettings();
            if(!HasFilters) { SetSaveSongButtonState(true); }
            UpdateStatus("Waiting for start...");
        }

        private void Start()
        {
            if (!SavePathValid()) { return; }

            SetStartButtonLabel("STOP");
            if(stream != null) { stream?.Dispose(); stream = null; }

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
                    MaxBufferSize = 10 * 2000000   // stop when buffer size passes 20 megabytes
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
            
            UpdateStatus("Starting...");
            stream.Start();
        }
        private void Stop()
        {
            if (stream != null) { stream.Dispose(); stream = null; }
            successfullyConnected = false;
            connected = false;
            reconnectCount = 0;
            SetStartButtonLabel("START");
            UpdateLog("STREAM STOPPED");
            UpdateStatus("Waiting to start.");
        }

#region FORM EVENTS
        private void Start_Clicked(object sender, EventArgs e)
        {
            if (startButton.Text == "STOP")
            {
                Stop();
                return;
            }
            Start();
        }

        private void Browse_Clicked(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog().ToString() == "OK")
            {
                saveLocationInput.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void FilterText_Changed(object sender, EventArgs e)
        {
            filters = new List<string>(filterWordsInput.Text.Split(Environment.NewLine));
            if (filters.Count != 0)
            {
                for (int i = filters.Count - 1; i >= 0; i--)
                {
                    if (String.IsNullOrWhiteSpace(filters[i]))
                    {
                        filters.RemoveAt(i);
                    }
                }
            }
            if (filters.Count == 0)
            {
                filterWordsInput.Text = "";
                SetSaveSongButtonState(true);
                return;
            }
            else if(SongMatchesFilter(currentSong))
            {
                SetSaveSongButtonState(true);
            }
            else if (!saveCurrentlyPlaying)
            {
                SetSaveSongButtonState(false);
            }
        }

        private void SaveCurrentlyPlaying_Clicked(object sender, EventArgs e)
        {
            SaveCurrentlyPlaying = true;
        }

        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            SaveSettings();
            if (stream != null) { stream?.Dispose(); }
        }

#endregion

#region StreamingEvents
        private void MetadataChanged(object sender, StreamRipper.Models.Events.MetadataChangedEventArg arg)
        {
            if (!HasFilters) { SetSaveSongButtonState(true); }
            if (!connected)
            {
                connected = true;
                UpdateLog("Connected!");
                UpdateLog("");
            }
            if (arg != null && arg.SongMetadata != null)
            {
                successfullyConnected = true;
                reconnectCount = 0;

                currentSong = arg.SongMetadata.ToString();

                if (SongMatchesFilter(arg.SongMetadata))
                {
                    SaveCurrentlyPlaying = true;
                    FlashWindow.Flash(this);
                    UpdateLog("");
                    UpdateLog("Found a matching song! Will save when completed playing: " + arg.SongMetadata.ToString());
                } 
                else if (HasFilters) { SaveCurrentlyPlaying = false; }

                UpdateSongData(arg.SongMetadata);
            }
        }

        private void SongChanged(object sender, StreamRipper.Models.Events.SongChangedEventArg arg)
        {
            if(arg == null || arg.SongInfo == null || arg.SongInfo.SongMetadata == null || String.IsNullOrWhiteSpace(arg.SongInfo.SongMetadata.Artist)) { SaveCurrentlyPlaying = false; return; }

            if(SaveCurrentlyPlaying || !HasFilters)
            {
                SaveSong(arg);
            }
            else if (SongMatchesFilter(arg.SongInfo.SongMetadata))
            {
                SaveSong(arg);
            }

            SaveCurrentlyPlaying = false;
        }


        private void StreamFailed(object sender, StreamRipper.Models.Events.StreamFailedEventArg arg)
        {
            connected = false;

            if (successfullyConnected)
            {
                if (reconnectCount > reconnectAttempts.Value)
                {
                    UpdateLog("Tried reconnecting max times. Stream may be down or URL is incorrect.");
                    Action safeWrite = delegate {
                        Stop();
                    };
                    reconnectAttempts.Invoke(safeWrite);
                }
                else
                {
                    Action safeWrite = delegate { 
                        reconnectCount += 1;
                        stream?.Start();
                    };
                    reconnectAttempts.Invoke(safeWrite);
                }
            }
            else
            {
                UpdateLog("Stream Failed to run. Ensure it's the correct type of url!", true);
                Stop();
            }
        }

        private void StreamStart(object sender, StreamRipper.Models.Events.StreamStartedEventArg arg)
        {
            if (reconnectCount > 0)
            {
                UpdateLog("Attempting to reconnect...");
            }
            else
            {
                UpdateLog("Attempting to connect...");
            }
            if (arg != null && arg.SongInfo != null && arg.SongInfo.SongMetadata != null)
            {
                UpdateLog("Connected!");
                UpdateLog(" ");
                UpdateSongData(arg.SongInfo.SongMetadata);
            }
        }
        #endregion

#region UTILITY FUNCTIONS
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
            if(metadata != null) { return SongMatchesFilter(metadata.ToString()); }
            return false;
        }

        private bool SongMatchesFilter(string song)
        {
            if (filters != null && filters.Count > 0)
            {
                foreach (var filter in filters)
                {
                    if (song.Contains(filter, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private void SaveSong(StreamRipper.Models.Events.SongChangedEventArg songArg)
        {
            if (SavePathValid())
            {
                UpdateLog("SAVED SONG: " + songArg.SongInfo.ToString());
                System.IO.File.WriteAllBytes(Path.Combine(saveLocationInput.Text, $"{rg.Replace(songArg.SongInfo.SongMetadata.ToString(), "_")}.mp3"), songArg.SongInfo.Stream.ToArray());
            }
        }
        void LoadSettings()
        {
            reconnectAttempts.Value = Settings.Default.ReconnectMax;
            filterWordsInput.Text = Settings.Default.LastFilters;
            saveLocationInput.Text = Settings.Default.LastPath;
            streamUrlBox.Text = Settings.Default.LastStream;
        }

        private void SaveSettings()
        {
            Settings.Default.ReconnectMax = (uint)reconnectAttempts.Value;
            Settings.Default.LastFilters = filterWordsInput.Text;
            Settings.Default.LastPath = saveLocationInput.Text;
            Settings.Default.LastStream = streamUrlBox.Text;
            Settings.Default.Save();
        }
        #endregion

        #region UI
        private void SetSaveSongButtonState(bool saveCurrent)
        {
            if (saveCurrentlyPlayingBtn.InvokeRequired)
            {
                Action safeWrite = delegate { SetSaveSongButtonState(saveCurrent); };
                saveCurrentlyPlayingBtn.Invoke(safeWrite);
            }
            else
            {
                if (saveCurrent)
                {
                    saveCurrentlyPlayingBtn.Enabled = false;
                    saveCurrentlyPlayingBtn.Text = "Saving Currently Playing Song";
                }
                else
                {
                    saveCurrentlyPlayingBtn.Enabled = true;
                    saveCurrentlyPlayingBtn.Text = "Save Currently Playing Song?";
                }
            }
        }
        private void SetStartButtonLabel(string text)
        {
            if (startButton.InvokeRequired)
            {
                Action safeWrite = delegate { SetStartButtonLabel($"{text}"); };
                startButton.Invoke(safeWrite);
            }
            else
            {
                startButton.Text = text;
            }
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
            if (logBox.InvokeRequired)
            {
                Action safeWrite = delegate { UpdateLog($"{text}"); };
                logBox.Invoke(safeWrite);
            }
            else
            {
                logBox.AppendText(Environment.NewLine + text);
            }
            if (setLabel)
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
        #endregion
    }
}

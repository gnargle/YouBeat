using NAudio.Wave;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YouBeatTypes;

namespace YouBeatMapper {
    public partial class Form1 : Form {

        private WaveOutEvent outputDevice;
        private AudioFileReader audioFile;

        private string SongFolder;

        private bool closing = false;

        public Form1() {
            InitializeComponent();
            fileDialogSongLoad.InitialDirectory = Directory.GetCurrentDirectory();
            this.FormClosing += buttonStop_Click;
        }

        public Song CurrentSong { get; private set; }

        private void loadSongToolStripMenuItem_Click(object sender, EventArgs e) {
            if (fileDialogSongLoad.ShowDialog() == DialogResult.OK) {
                SongFolder = Path.GetDirectoryName(fileDialogSongLoad.FileName);
                var file = fileDialogSongLoad.FileName;
                var json = File.ReadAllText(file);
                CurrentSong = JsonConvert.DeserializeObject<Song>(json);
            }
        }

        private void buttonPlay_Click(object sender, EventArgs e) {
            if (CurrentSong != null) {
                if (outputDevice == null) {
                    outputDevice = new WaveOutEvent();
                    outputDevice.PlaybackStopped += OnPlaybackStopped;
                }
                if (audioFile == null) {
                    audioFile = new AudioFileReader(Path.Combine(SongFolder, "Tracks", CurrentSong.FileName));
                    outputDevice.Init(audioFile);
                    waveViewer1.
                }
                outputDevice.Play();
            }
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e) {
            if (closing) { outputDevice.Dispose(); audioFile.Dispose(); }
        }

        private void buttonStop_Click(object sender, EventArgs e) {
            outputDevice?.Stop();
        }
    }
}

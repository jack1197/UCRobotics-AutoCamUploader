using Google.Apis.Auth.OAuth2;
using Google.Apis.YouTube.v3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video.FFMPEG;
using AForge.Video.DirectShow;
using Google.Apis.Services;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Upload;
using System.IO.Ports;

namespace UCRobotics_AutoCamUploader
{
    public partial class Form1 : Form
    {
        UserCredential Credential;
        VideoFileWriter FileWriter;
        YouTubeService youtubeService;
        VideoCaptureDevice device;
        SerialPort port;
        bool validPort = false;
        bool ready = false;
        bool recording = false;


        public Form1()
        {
            InitializeComponent();
            FileWriter = new VideoFileWriter();
            updatePortList();
        }

        private void updatePortList()
        {
            var selected = portSelect.SelectedItem;
            var newList = SerialPort.GetPortNames();
            if(newList.Where((item) => !portSelect.Items.Contains(item)).Count() != 0 || newList.Length != portSelect.Items.Count || !newList.Contains(selected))
            {
                portSelect.Items.Clear();
                portSelect.Items.AddRange(newList);
                if(selected?.GetType() == typeof(string) && newList.Contains(selected))
                {
                    portSelect.SelectedItem = selected;
                }
                else
                {
                    portSelect.SelectedIndex = 0;
                }
            }
            port = new SerialPort((string)portSelect.SelectedItem);
        }

        public async Task checkPort()
        {
            try
            {
                validPort = false;
                port.Open();
                port.ReadTimeout = 100;
                port.WriteTimeout = 100;
                port.WriteLine("ack");
                string result = port.ReadLine();
                if (result != "acked")
                {
                    port.Close();
                    return;
                }
                port.WriteLine("id");
                result = port.ReadLine();
                if (result != "UCRoboticsFieldController")
                {
                    port.Close();
                    return;
                }
                port.Close();
                validPort = true;
            }
            catch(Exception)
            {
                port.Close();
                validPort = false;
                return;
            }
        }

        private void updateReady()
        {
            serialIndicator.Checked = validPort;
            youtubeIndicator.Checked = youtubeService != null;
            camIndicator.Checked = device != null;
            if(serialIndicator.Checked && youtubeIndicator.Checked && camIndicator.Checked)
            {
                indicatorPanel.BackColor = Color.Green;
            }
            else
            {
                indicatorPanel.BackColor = Color.Red;
            }
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            using (Stream stream = new MemoryStream(Properties.Resources.client_secrets))
            {
                Credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { YouTubeService.Scope.YoutubeUpload }, Guid.NewGuid().ToString(), CancellationToken.None);
            }
            Console.WriteLine("logged");

            youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = "UC Robotics Auto Uploader"
            });
            updateReady();
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            FileWriter.Open("Test.mp4", device.VideoResolution.FrameSize.Width, device.VideoResolution.FrameSize.Height);
            recording = true;
        }

        private void Device_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            if (recording)
            {
                FileWriter.WriteVideoFrame(eventArgs.Frame);
            }
            display.Image = new Bitmap(eventArgs.Frame);
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            recording = false;
            await Task.Delay(500);
            FileWriter.Close();

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = "Default Video Title";
            video.Snippet.Description = "Default Video Description";
            video.Snippet.Tags = new string[] { "tag1", "tag2" };
            video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "unlisted"; // or "private" or "public"
            var filePath = @"Test.mp4"; // Replace with path to actual movie file.

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");

                await videosInsertRequest.UploadAsync();
            }
        }

        private void btnCamSetup_Click(object sender, EventArgs e)
        {
            VideoCaptureDeviceForm dialog = new VideoCaptureDeviceForm();
            DialogResult res = dialog.ShowDialog();
            if (res != DialogResult.OK)
            {
                return;
            }
            device = dialog.VideoDevice;
            device.NewFrame += Device_NewFrame;
            device.Start();
            updateReady();
        }

        private void autoSelect_CheckedChanged(object sender, EventArgs e)
        {
            portSelect.Enabled = !autoSelect.Checked;
        }

        private async void refreshPortsTimer_Tick(object sender, EventArgs e)
        {
            refreshPortsTimer.Enabled = false;
            
            updatePortList();
            await checkPort();
            if(autoSelect.Checked && !validPort)
            {
                if(portSelect.SelectedIndex == portSelect.Items.Count - 1)
                {
                    portSelect.SelectedIndex = 0;
                }
                else
                {
                    portSelect.SelectedIndex += 1;
                }
            }

            updateReady();
            refreshPortsTimer.Enabled = true;
        }
        

        private void portSelect_SelectionChangeCommitted(object sender, EventArgs e)
        {
            port = new SerialPort((string)portSelect.SelectedItem);
        }
    }
}

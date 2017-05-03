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

namespace UCRobotics_AutoCamUploader
{
    public partial class Form1 : Form
    {
        UserCredential Credential;
        VideoFileWriter FileWriter;
        YouTubeService youtubeService;
        VideoCaptureDevice device;
        bool recording = false;

        public Form1()
        {
            InitializeComponent();
            FileWriter = new VideoFileWriter();
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
            if (device != null)
            {

            btnUpload.Enabled = true;
            }
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            FileWriter.Open("Test.mp4", device.VideoResolution.FrameSize.Width, device.VideoResolution.FrameSize.Height);
            btnUpload.Enabled = false;
            btnStop.Enabled = true;
            recording = true;
        }

        private void Device_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            if (recording)
            {
                FileWriter.WriteVideoFrame(eventArgs.Frame);
            }
            if(display.Image != null)
            {
                display.Image.Dispose();
            }
            display.Image = new Bitmap(eventArgs.Frame);
            eventArgs.Frame.Dispose();
        }

        private async void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
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
            btnUpload.Enabled = true;
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
            if(youtubeService != null)
            {
                btnUpload.Enabled = true;
            }
        }
    }
}

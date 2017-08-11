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
        bool auton = false;
        bool recording = false;
        TimeSpan maxTime = new TimeSpan(0, 0, 5);
        DateTime startTime = DateTime.MaxValue;
        bool overTimedFlag = false;
        string filename = "";

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
            if (newList.Length == 0)
            {
                port = null;
                portSelect.Items.Clear();
                portSelect.Enabled = false;
                autoSelect.Enabled = false;
                autoSelect.Checked = false;
            }
            else
            {
                autoSelect.Enabled = true;

                if (newList.Where((item) => !portSelect.Items.Contains(item)).Count() != 0 || newList.Length != portSelect.Items.Count || !newList.Contains(selected))
                {
                    portSelect.Items.Clear();
                    portSelect.Items.AddRange(newList);
                    if (selected?.GetType() == typeof(string) && newList.Contains(selected))
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
        }

        public async Task checkPort()
        {
            try
            {
                validPort = false;
                port.BaudRate = 57600;
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
            catch (Exception)
            {
                try
                {
                    port.Close();
                }
                catch { }
                validPort = false;
                return;
            }
        }

        private void updateReady()
        {
            serialIndicator.Checked = validPort;
            youtubeIndicator.Checked = youtubeService != null;
            camIndicator.Checked = device != null;
            if (serialIndicator.Checked && youtubeIndicator.Checked && camIndicator.Checked)
            {
                indicatorPanel.BackColor = Color.Green;
                ready = true;
            }
            else
            {
                indicatorPanel.BackColor = Color.Red;
                ready = false;
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

        private void StartRec()
        {
            filename = $"{Guid.NewGuid().ToString()}.mp4";
            FileWriter.Open(filename, device.VideoResolution.FrameSize.Width, device.VideoResolution.FrameSize.Height);
            recording = true;
            BackColor = Color.Green;
            startTime = DateTime.Now;    
        }

        private void Device_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)

        {
            if (recording)
            {                        
                    FileWriter.WriteVideoFrame(eventArgs.Frame);
            }
            Image newImage = new Bitmap(eventArgs.Frame);
            try
            {
                Image temp = display.Image;
                display.Image = newImage;
                temp.Dispose();
            }
            catch { }
        }

        private async void StopRec()
        {
            string saveFile = filename;
            recording = false;           
            await Task.Delay(1000); 
            FileWriter.Close();
            await Task.Delay(500);

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = $"UC Robotics AutoRecorded {(auton ? ("Auton") : ("Driver"))} {DateTime.Now.ToString()}";
            video.Snippet.Description = $"Recorded on {DateTime.Now.ToLongDateString()} at {DateTime.Now.ToLongTimeString()}";
            video.Snippet.Tags = new string[] { auton ? "Autonomous" : "Manual" };
            video.Snippet.CategoryId = "22"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "unlisted"; // or "private" or "public"
            var filePath = saveFile; // Replace with path to actual movie file.

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");

                await videosInsertRequest.UploadAsync();
            }

        }

        private void btnCamSetup_Click(object sender, EventArgs e)
        {
            if (device != null)
            {
                device.Stop();
                device.NewFrame -= Device_NewFrame;
            }
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

            portSelect.Enabled = !autoSelect.Checked;
            updatePortList();
            await checkPort();
            if (autoSelect.Checked && !validPort)
            {
                if (portSelect.SelectedIndex == portSelect.Items.Count - 1)
                {
                    portSelect.SelectedIndex = 0;
                }
                else
                {
                    portSelect.SelectedIndex += 1;
                }
            }
            if (ready)
            {
                port.Open();
                port.WriteTimeout = 100;
                port.ReadTimeout = 100;
                port.WriteLine("mode");
                var res = port.ReadLine();
                auton = res == "auton";
                port.WriteLine("status");
                res = port.ReadLine();
                port.Close();
                if (res == "rec")
                {
                    if (!recording && !overTimedFlag)
                    {
                        StartRec();
                    }
                    else if (recording && DateTime.Now - startTime > maxTime)
                    {
                        overTimedFlag = true;
                        BackColor = Color.Red;
                        StopRec();
                    }
                }
                else if (recording)
                {
                    StopRec();
                }     
                else if(res == "norec")
                {
                    overTimedFlag = false;

                    BackColor = DefaultBackColor;
                }

            }
            else
            {
                if (recording)
                {
                    StopRec();
                }
            }

            updateReady();
            refreshPortsTimer.Enabled = true;
        }


        private void portSelect_SelectionChangeCommitted(object sender, EventArgs e)
        {
            port = new SerialPort((string)portSelect.SelectedItem);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (port != null)
            {
                port.Dispose();

            }
            port = null;
            device = null;
            if (youtubeService != null)
                youtubeService.Dispose();
            youtubeService = null;
        }
    }
}

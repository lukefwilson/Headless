using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Headless
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.IO.Compression;
    using System.Net;
    public class Experience
    {
        public WebClient Client { get; }
        public string DownloadLink { get; }
        public string FileName { get; }
        public string DownloadSavePath { get; }
        public string ExtractSavePath { get; }

        public int Id { get; set; }
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool HasBeenViewed { get; set; }

        public static readonly string[] Experiences = {
            "C:/Users/hwray/Downloads/ovr_sdk_win_1.3.0_public/OculusSDK/Samples/OculusRoomTiny/OculusRoomTiny (DX11)/Bin/Windows/Win32/Debug/VS2015/OculusRoomTiny (DX11).exe",
            "C:/Users/hwray/Documents/Visual Studio 2015/Projects/Headless/Headless/bin/Debug/experiences/het/WindowsNoEditor/TestProject.exe",
            "C:/Users/hwray/Documents/Visual Studio 2015/Projects/Headless/Headless/bin/Debug/experiences/TPDragons1.3/tpdragons1.3.exe"
        };
        public int ExperienceIndex { get; set; }
        public int NumExperiences { get; }

        public Experience(string downloadLink)
        {
            this.Client = new WebClient();
            this.DownloadLink = downloadLink;

            this.FileName = DownloadLink.Split('/').Last();
            this.DownloadSavePath = "tmp/" + this.FileName;
            this.ExtractSavePath = "experiences/" + System.IO.Path.GetFileNameWithoutExtension(this.FileName);

            EnsureSafeSavePath(this.DownloadSavePath);


     //       this.ExperienceIndex = -1;
     //       this.NumExperiences = Experiences.Count();
     //       this.RunNextExperience();
        }

        public override string ToString()
        {
            return "Experience from: " + this.DownloadLink;
        }

        private void StartDownload()
        {
            this.Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
            this.Client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFinishedCallback);
            this.Client.DownloadFileAsync(new Uri(DownloadLink), this.DownloadSavePath);
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine("    downloaded {0} of {1} bytes. {2} % complete...",
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
        }

        private void DownloadFinishedCallback(object sender, AsyncCompletedEventArgs e)
        {
            Console.WriteLine("Finished downloading and saved at: " + this.DownloadSavePath);

            // try/catch on bad .zip
            // want to make extracting async
            Console.WriteLine("Extracting to: " + this.ExtractSavePath);
            EnsureSafeSavePath(this.ExtractSavePath);
            ZipFile.ExtractToDirectory(this.DownloadSavePath, this.ExtractSavePath);
            Console.WriteLine("Finished extracting to: " + this.ExtractSavePath);

            // this.Run();
        }

        private void Run(string experiencePath)
        {
            Console.WriteLine("Running program...");
            //    Process.Start(this.ExtractSavePath + "/WindowsNoEditor/TestProject.exe");
            // Console.WriteLine(System.Reflection.Assembly.GetEntryAssembly().Location + "/experiences/het/WindowsNoEditor/TestProject.exe");

            Process p = Process.Start(experiencePath);
            p.Exited += P_Exited;
            p.EnableRaisingEvents = true;
        }

        private void P_Exited(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            Console.WriteLine("Latest program exited with code: " + p.ExitCode);
            RunNextExperience();
        }

        private void RunNextExperience()
        {
            ExperienceIndex++;
            if (ExperienceIndex > NumExperiences - 1)
            {
                ExperienceIndex = 0;
            }
            Run(Experiences[ExperienceIndex]);
        }

        private void EnsureSafeSavePath(string downloadSavePath)
        {
            System.IO.FileInfo file = new System.IO.FileInfo(downloadSavePath);
            file.Directory.Create();

            if (File.Exists(downloadSavePath))
            {
                Console.WriteLine("File with same name already exists, continuing anyways... will overwrite");
            }
        }
    }

}

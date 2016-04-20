using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.ComponentModel;

using System.Data;
using System.Drawing;
using System.Diagnostics;

namespace Headless
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();
        }

        private void URLField_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void DownloadProgressBar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }

        private void SearchExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            Console.WriteLine("Downloading " + e.Parameter);

            ExperienceDownload download = new ExperienceDownload(e.Parameter.ToString(), DownloadProgressBar);

            e.Handled = true;
        }

    }


    public class ExperienceDownload
    {
        public WebClient Client { get; }
        public string DownloadLink { get; }
        public string FileName { get; }
        public string DownloadSavePath { get; }
        public string ExtractSavePath { get; }
        public ProgressBar ViewProgressBar { get; }

        public static readonly string[] Experiences = {
            "C:/Users/hwray/Documents/Visual Studio 2015/Projects/Headless/Headless/bin/Debug/experiences/het/WindowsNoEditor/TestProject.exe",
            "C:/Users/hwray/Documents/Visual Studio 2015/Projects/Headless/Headless/bin/Debug/experiences/TPDragons1.3/tpdragons1.3.exe"
        };
        public int ExperienceIndex { get; set; }
        public int NumExperiences { get; }

        public ExperienceDownload(string downloadLink, ProgressBar viewProgressBar)
        {
            /*  this.Client = new WebClient();
              this.DownloadLink = downloadLink;
              this.ViewProgressBar = viewProgressBar;

              this.FileName = DownloadLink.Split('/').Last();
              this.DownloadSavePath = "tmp/" + this.FileName;
              this.ExtractSavePath = "experiences/" + System.IO.Path.GetFileNameWithoutExtension(this.FileName);



              EnsureSafeSavePath(this.DownloadSavePath);

              this.Client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(DownloadProgressCallback);
              this.Client.DownloadFileCompleted += new AsyncCompletedEventHandler(DownloadFinishedCallback);
              this.Client.DownloadFileAsync(new Uri(DownloadLink), this.DownloadSavePath);
              */
            this.ExperienceIndex = -1;
            this.NumExperiences = Experiences.Count();
            this.RunNextExperience();
        }

        private void DownloadProgressCallback(object sender, DownloadProgressChangedEventArgs e)
        {
            Console.WriteLine("    downloaded {0} of {1} bytes. {2} % complete...",
                e.BytesReceived,
                e.TotalBytesToReceive,
                e.ProgressPercentage);
            ViewProgressBar.Value = e.ProgressPercentage;
            // Should be replaced with a delegate to view controller
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
            Console.WriteLine("Exited latest program");
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

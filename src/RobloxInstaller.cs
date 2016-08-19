using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.IO;
using System.IO.Compression;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace RobloxModManager
{
    public struct RbxInstallProtocol
    {
        public string FileName;
        public string LocalDirectory;

        public RbxInstallProtocol(string fileName, string localDirectory = "")
        {
            FileName = fileName;
            LocalDirectory = localDirectory;
        }
    }

    public partial class RobloxInstaller : Form
    {
        private string database;
        private int echoSteps = 0;

        public event EventHandler OnFinished;
        public string robloxStudioPath;

        WebClient http = new WebClient();
        List<RbxInstallProtocol> instructions = new List<RbxInstallProtocol>()
        {
            new RbxInstallProtocol("RobloxStudio"),
            new RbxInstallProtocol("Libraries"),
            new RbxInstallProtocol("redist"),
            new RbxInstallProtocol("imageformats"),

            new RbxInstallProtocol("content-fonts",        @"content\fonts"),
            new RbxInstallProtocol("content-music",        @"content\music"),
            new RbxInstallProtocol("content-particles",    @"content\particles"),
            new RbxInstallProtocol("content-scripts",      @"content\scripts"),
            new RbxInstallProtocol("content-sky",          @"content\sky"),
            new RbxInstallProtocol("content-sounds",       @"content\sounds"),
            new RbxInstallProtocol("content-textures",     @"content\textures"),
            new RbxInstallProtocol("content-textures2",    @"content\textures"),

            new RbxInstallProtocol("content-terrain",      @"PlatformContent\pc\terrain"),
            new RbxInstallProtocol("content-textures3",    @"PlatformContent\pc\textures"),

            new RbxInstallProtocol("BuiltInPlugins",       @"BuiltInPlugins"),
            new RbxInstallProtocol("shaders",              @"shaders"),
        };

        public async Task setStatus(string status)
        {
            await Task.Delay(500);
            statusLbl.Text = status;
        }


        public async Task echo(string text)
        {
            echoSteps++;
            if (echoSteps % 20 == 0)
            {
                await Task.Delay(1);
            }
            log.Items.Add(text);
            log.SelectedIndex = log.Items.Count - 1;
            log.SelectedIndex = -1;
        }

        public RobloxInstaller()
        {
            InitializeComponent();
        }

        public string getDirectory(params string[] paths)
        {
            string basePath = "";
            foreach (string path in paths)
            {
                basePath = Path.Combine(basePath, path);
            }
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            return basePath;
        }

        public async Task<string> RunInstaller(string database, bool forceInstall)
        {
            this.Show();
            this.BringToFront();
            string setupDir = "http://setup." + database + ".com/";
            await setStatus("Checking for updates");
            string buildName = await http.DownloadStringTaskAsync(setupDir + "versionQTStudio");
            string localAppData = Environment.GetEnvironmentVariable("LocalAppData");
            string rootDir = getDirectory(localAppData, "ROBLOX Studio");
            string downloads = getDirectory(rootDir, "downloads");
            string buildPath = getDirectory(rootDir, buildName);
            string appSettings = Path.Combine(buildPath, "AppSettings.xml");
            await echo("Checking build installation...");
            if (!File.Exists(appSettings) || forceInstall)
            {
                await echo("This build hasn't been installed!");
                await setStatus("Loading latest ROBLOX Studio build from " + database + ".com");
                progressBar.Maximum = instructions.Count*30;
                progressBar.Value = 0;
                progressBar.Style = ProgressBarStyle.Continuous;
                foreach (RbxInstallProtocol protocol in instructions)
                {
                    string zipFileUrl = setupDir + buildName + "-" + protocol.FileName + ".zip";
                    string extractDir = getDirectory(buildPath, protocol.LocalDirectory);
                    Console.WriteLine(extractDir);
                    string downloadPath = Path.Combine(downloads, protocol.FileName + ".zip");
                    Console.WriteLine(downloadPath);
                    await echo("Fetching: " + zipFileUrl);
                    byte[] downloadedFile = await http.DownloadDataTaskAsync(zipFileUrl);
                    FileStream writeFile = File.Create(downloadPath);
                    writeFile.Write(downloadedFile,0,downloadedFile.Length);
                    writeFile.Close();
                    ZipArchive archive = ZipFile.Open(downloadPath, ZipArchiveMode.Read);
                    foreach (ZipArchiveEntry entry in archive.Entries)
                    {
                        if (string.IsNullOrEmpty(entry.Name))
                        {
                            string localPath = Path.Combine(protocol.LocalDirectory, entry.FullName);
                            string path = Path.Combine(buildPath,localPath);
                            await echo("Creating directory " + localPath);
                            getDirectory(path);
                        }
                        else
                        {
                            string entryName = entry.FullName;
                            await echo("Extracting " + entryName + " to " + buildName + "\\" + protocol.LocalDirectory);
                            string relativePath = Path.Combine(extractDir, entryName);
                            string directoryParent = Directory.GetParent(relativePath).ToString();
                            getDirectory(directoryParent);
                            if (!File.Exists(relativePath))
                            {
                                entry.ExtractToFile(relativePath);
                            }
                        }
                    }
                    progressBar.Increment(30);
                }
                await setStatus("Configuring ROBLOX Studio");
                progressBar.Style = ProgressBarStyle.Marquee;
                await echo("Writing AppSettings.xml");
                File.WriteAllText(appSettings, "<Settings><ContentFolder>content</ContentFolder><BaseUrl>http://www.roblox.com</BaseUrl></Settings>");
                await echo("ROBLOX Studio " + buildName + " successfully installed!");
            }
            else
            {
                await echo("This version of ROBLOX Studio has been installed!");
            }
            await setStatus("Starting ROBLOX Studio...");
            return Path.Combine(buildPath,"RobloxStudioBeta.exe");
        }
    }
}
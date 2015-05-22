using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace RobloxModManager
{
    public partial class Launcher : Form
    {
        WebClient http = new WebClient();

        public string getModPath()
        {
            string appData = Environment.GetEnvironmentVariable("AppData");
            string root = Path.Combine(appData, "RbxModManager", "ModFiles");
            if (!Directory.Exists(root))
            {
                // Build a folder structure so the usage of my mod manager is more clear.
                string[] folderPaths = new string[]
                {
                    "BuiltInPlugins",
                    "ClientSettings",
                    "content/fonts",
                    "content/music",
                    "content/particles",
                    "content/scripts",
                    "content/sky",
                    "content/sounds",
                    "content/textures/loading",
                    "content/textures/particles",
                    "content/textures/ui",
                    "PlatformContent/pc/textures/aluminum",
                    "PlatformContent/pc/textures/brick",
                    "PlatformContent/pc/textures/cobblestone",
                    "PlatformContent/pc/textures/concrete",
                    "PlatformContent/pc/textures/diamondplate",
                    "PlatformContent/pc/textures/fabric",
                    "PlatformContent/pc/textures/granite",
                    "PlatformContent/pc/textures/grass",
                    "PlatformContent/pc/textures/ice",
                    "PlatformContent/pc/textures/marble",
                    "PlatformContent/pc/textures/metal",
                    "PlatformContent/pc/textures/pebble",
                    "PlatformContent/pc/textures/plastic",
                    "PlatformContent/pc/textures/rust",
                    "PlatformContent/pc/textures/sand",
                    "PlatformContent/pc/textures/sky",
                    "PlatformContent/pc/textures/slate",
                    "PlatformContent/pc/textures/terrain",
                    "PlatformContent/pc/textures/water",
                    "PlatformContent/pc/textures/wood",
                    "PlatformContent/pc/textures/woodplanks"
                };
                foreach (string f in folderPaths)
                {
                    string path = Path.Combine(root,f.Replace("/", "\\"));
                    Directory.CreateDirectory(path);
                }
            }
            return root;
        }

        public async void DownloadStudio(string path, string dataBase)
        {
            string filePath = Path.Combine(path, "RobloxStudioLauncherBeta.exe");

            byte[] RobloxStudio = http.DownloadData("http://setup." + dataBase + ".com/RobloxStudioLauncherBeta.exe");
            FileStream download = File.Create(filePath);
            download.Write(RobloxStudio, 0, RobloxStudio.Length);
            download.Close();

            Process.Start(filePath);

            bool waitingToComplete = true;
            bool possiblyComplete = false;

            while (waitingToComplete)
            {
                Process[] stillUpdating = Process.GetProcessesByName("RobloxStudioLauncherBeta");
                if (stillUpdating.Length > 0)
                {
                    possiblyComplete = false;
                    Process currentStudio = stillUpdating[0];
                    currentStudio.WaitForExit();
                }
                else
                {
                    if (!possiblyComplete)
                    {
                        possiblyComplete = true;
                        await Task.Delay(1000);
                    }
                    else
                    {
                        waitingToComplete = false;
                    }
                }
            }

            // The launcher will attempt to start studio once its done downloading and stuff.
            // We need to prevent this.

            await Task.Delay(1000);
            Process[] running = Process.GetProcessesByName("RobloxStudioBeta");
            foreach (Process p in running)
            {
                try
                {
                    p.Kill();
                }
                catch
                {
                    // Sometimes it won't let us :/
                }
            }
        }

        public string getStudioExePath()
        {
            string[] envPaths = new string[] { 
                Environment.GetEnvironmentVariable("LocalAppData"), 
                Environment.GetEnvironmentVariable("ProgramFiles")
            };
            string dataBase = (string)dataBaseSelect.Items[Properties.Settings.Default.Database];
            string version = http.DownloadString("http://setup." + dataBase + ".com/versionQTStudio");
            string exePath = null;
            foreach (string envPath in envPaths)
            {
                string directory = Path.Combine(envPath, "Roblox", "Versions", version, "RobloxStudioBeta.exe");
                if (File.Exists(directory))
                {
                    exePath = directory;
                    break;
                }
            }
            if (exePath != null)
            {
                return exePath;
            }
            else
            {
                string appData = Environment.GetEnvironmentVariable("AppData");
                string root = Path.Combine(appData, "RbxModManager");
                if (!Directory.Exists(root))
                {
                    Directory.CreateDirectory(root);
                }
                DownloadStudio(root,dataBase);
                foreach (string envPath in envPaths)
                {
                    string directory = Path.Combine(envPath, "Roblox", "Versions", version, "RobloxStudioBeta.exe");
                    if (File.Exists(directory))
                    {
                        return directory;
                    }
                }
            }
            throw new Exception("Couldn't locate Roblox Studio");
        }

        public Launcher()
        {
            InitializeComponent();
        }

        private void manageMods_Click(object sender, EventArgs e)
        {
            string modPath = getModPath();
            Process.Start(modPath);
        }

        private void launchStudio_Click(object sender, EventArgs e)
        {
            this.Hide();
            // Check mod folders to see if we need to override anything, then launch.
            string studioPath = getStudioExePath();
            string studioRoot = Directory.GetParent(studioPath).ToString();
            string modPath = getModPath();
            string[] studioFiles = Directory.GetFiles(studioRoot);
            string[] modFiles = Directory.GetFiles(modPath,"*.*",SearchOption.AllDirectories);
            foreach (string modFile in modFiles)
            {
                try
                {
                    byte[] fileContents = File.ReadAllBytes(modFile);
                    FileInfo modFileControl = new FileInfo(modFile);
                    string relativeFile = modFile.Replace(modPath, studioRoot);
                    string relativeDir = Directory.GetParent(relativeFile).ToString();
                    if (!Directory.Exists(relativeDir))
                    {
                        Directory.CreateDirectory(relativeDir);
                    }
                    if (!File.Exists(relativeFile))
                    {
                        File.Create(relativeFile);
                    }
                    byte[] studioFile = File.ReadAllBytes(relativeFile);
                    if (!fileContents.SequenceEqual(studioFile))
                    {
                        modFileControl.CopyTo(relativeFile, true);
                    }
                }
                catch
                {
                    Console.WriteLine("Failed to overwrite " + modFile + "\nMight be open in another program\nThats their problem, not mine <3");
                }
            }
            Process Studio = Process.Start(studioPath);
            Studio.WaitForExit();
            Application.Exit();
        }

        private void Launcher_Load(object sender, EventArgs e)
        {
            dataBaseSelect.SelectedIndex = Properties.Settings.Default.Database;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Database = dataBaseSelect.SelectedIndex;
            Properties.Settings.Default.Save();
        }
    }
}
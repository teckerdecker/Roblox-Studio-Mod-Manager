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

        public Launcher()
        {
            InitializeComponent();
        }

        private void manageMods_Click(object sender, EventArgs e)
        {
            string modPath = getModPath();
            Process.Start(modPath);
        }

        private async void launchStudio_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            string dataBase = (string)dataBaseSelect.Items[Properties.Settings.Default.Database];
            RobloxInstaller installer = new RobloxInstaller();
            string studioPath = await installer.RunInstaller(dataBase,forceRebuild.Checked);
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
                        FileStream currentStream = File.Create(relativeFile);
                        currentStream.Close();
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
            Process roblox = Process.Start(studioPath);
            await Task.Delay(500);
            installer.Dispose();
            this.Dispose();
            roblox.WaitForExit();
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
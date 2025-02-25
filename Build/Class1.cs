using System;
using System.IO;
using System.Collections.Generic;
using WixSharp;
using File = WixSharp.File;

namespace Build
{
    internal class Program
    {
        private static string projectName = "BIMExpetrise";
        private static string version = "1.0.6";

        static void Main(string[] args)
        {
            var versions = new List<string> { "22", "23" };
            var patterns = new List<string>();
            var destinationDirs = new List<string>();
            
            foreach (var ver in versions)
            {
                string pattern = $@"..\BIMExpertise\bin\Release R{ver}\publish\Revit 20{ver} Release R{ver} addin\*.*";
                patterns.Add(pattern);
                
                string destDir = $@"%AppDataFolder%\Autodesk\Revit\Addins\20{ver}";
                destinationDirs.Add(destDir);
            }
            

            int selectedIndex = 1;
            string selectedPattern = patterns[selectedIndex];
            string selectedDestination = destinationDirs[selectedIndex];
            
            var dirsList = new List<Dir>();
            for (int i = 0; i < versions.Count; i++)
            {
                dirsList.Add(
                    new InstallDir(destinationDirs[i],
                        new Files(patterns[i])
                    )
                );
            }

            var project = new Project
            {
                Name = projectName,
                UI = WUI.WixUI_ProgressOnly,
                OutDir = "output",
                GUID = new Guid("D56A3F69-DEB4-4332-B726-1DF06709DE7E"),
                MajorUpgrade = MajorUpgrade.Default,
                ControlPanelInfo =
                {
                    Manufacturer = Environment.UserName
                },
                Dirs = new[]
                {
                    new InstallDir(selectedDestination,
                        new Files(selectedPattern)
                    )
                }
            };

            project.Version = new Version(version);
            project.BuildMsi();
        }
    }
}

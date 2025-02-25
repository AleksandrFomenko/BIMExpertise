using WixSharp;

namespace Build;

public class Class1
{
    internal class Program
        {
            private static string projectName = "йцуйцуйцу";
            private static string version = "1";
            static void Main(string[] args)
            {
                var project = new Project()
                {
                    Name = projectName,
                    UI = WUI.WixUI_ProgressOnly,
                    OutDir = "output",
                    GUID = new Guid("D56A3F69-DEB4-4332-B726-1DF06709DE7E"),
                    MajorUpgrade = MajorUpgrade.Default,
                    ControlPanelInfo =
                    {
                        Manufacturer = Environment.UserName,
                    },
                    Dirs = new Dir[]
                    {
                        new InstallDir(@"%AppDataFolder%\Autodesk\Revit\Addins\2023\",
                            new File(@"C:\Users\Sergei Nefyodov\source\repos\Revit-API-Blog\FirstRevitPlugin.addin"),
                            new Dir(@"FirstRevitPlugin",
                            new DirFiles(@"C:\Users\Sergei Nefyodov\source\repos\Revit-API-Blog\bin\Debug\*.*")))
                    },
    
                };
                project.Version = new Version(version);
    
                project.BuildMsi();
            }
        }
}
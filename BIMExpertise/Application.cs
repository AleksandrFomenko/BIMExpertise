using Nice3point.Revit.Toolkit.External;
using BIMExpertise.Commands;

namespace BIMExpertise;

/// <summary>
///     Application entry point
/// </summary>
[UsedImplicitly]
public class Application : ExternalApplication
{
    public override void OnStartup()
    {
        CreateRibbon();
    }

    private void CreateRibbon()
    {
        var panel = Application.CreatePanel("Commands", "BIMExpertise");

        panel.AddPushButton<StartupCommand>("Execute")
            .SetImage("/BIMExpertise;component/Resources/Icons/RibbonIcon16.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/RibbonIcon32.png");
    }
}
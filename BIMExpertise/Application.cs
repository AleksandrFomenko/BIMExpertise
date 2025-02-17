using System.Windows.Media;
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
        var panelGeneral = Application.CreatePanel("General", "BIMExpertise");
        var panelBackgroundBrushLightCoral = new SolidColorBrush(System.Windows.Media.Color.FromRgb(240, 128, 128));
        panelGeneral.SetTitleBarBackground(panelBackgroundBrushLightCoral);

        panelGeneral.AddPushButton<StartupCommand>("Execute")
            .SetImage("/BIMExpertise;component/Resources/Icons/RibbonIcon16.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/RibbonIcon32.png");
        
        var panelMep = Application.CreatePanel("MEP", "BIMExpertise");
        var panelBackgroundBrushTurquoise = new SolidColorBrush(System.Windows.Media.Color.FromRgb(100, 149, 237));
        panelMep.SetTitleBarBackground(panelBackgroundBrushTurquoise);
        
        var panelAr = Application.CreatePanel("Architecture/construction", "BIMExpertise");
        var panelBackgroundBrushPurple =
            new SolidColorBrush(System.Windows.Media.Color.FromRgb(204, 204, 255));
        panelAr.SetTitleBarBackground(panelBackgroundBrushPurple);
    }
}
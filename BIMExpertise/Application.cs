using System.Windows.Media;
using BIMExpertise.Commands;
using Nice3point.Revit.Toolkit.External;
using Color = System.Windows.Media.Color;

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
        // GENERAL
        var panelGeneral = Application.CreatePanel("General", "BIMExpertise");
        var panelBackgroundBrushLightCoral = new SolidColorBrush(Color.FromRgb(240, 128, 128));
        panelGeneral.SetTitleBarBackground(panelBackgroundBrushLightCoral);
        panelGeneral.AddPushButton<StartupCommand>("Execute")
            .SetImage("/BIMExpertise;component/Resources/Icons/RibbonIcon16.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/RibbonIcon32.png");

        // MEP
        var panelMep = Application.CreatePanel("MEP", "BIMExpertise");
        var panelBackgroundBrushTurquoise = new SolidColorBrush(Color.FromRgb(100, 149, 237));
        panelMep.SetTitleBarBackground(panelBackgroundBrushTurquoise);
        panelMep.AddPushButton<Pipes.Commands.StartupCommand>("Pipes")
            .SetImage("/BIMExpertise;component/Resources/Icons/PipesIcon32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/PipesIcon32.png");
        panelMep.AddPushButton<PipesFittings.Commands.StartupCommand>("PipesFittings")
            .SetImage("/BIMExpertise;component/Resources/Icons/PipesFittings32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/PipesFittings32.png");
        panelMep.AddPushButton<Valve.Commands.StartupCommand>("Valve")
            .SetImage("/BIMExpertise;component/Resources/Icons/Valve32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Valve32.png");
        panelMep.AddPushButton<SpaceHeater.Commands.StartupCommand>("SpaceHeater")
            .SetImage("/BIMExpertise;component/Resources/Icons/SpaceHeater32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/SpaceHeater32.png");

        // AR/CR
        var panelAr = Application.CreatePanel("Architecture/construction", "BIMExpertise");
        var panelBackgroundBrushPurple =
            new SolidColorBrush(Color.FromRgb(204, 204, 255));
        panelAr.SetTitleBarBackground(panelBackgroundBrushPurple);
        panelAr.AddPushButton<Walls.Commands.StartupCommand>("Walls")
            .SetImage("/BIMExpertise;component/Resources/Icons/Wall32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Wall32.png");
    }
}
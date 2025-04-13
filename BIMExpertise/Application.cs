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

        // 
        var panelPipes = Application.CreatePanel("Pipeline systems", "BIMExpertise");
        var panelBackgroundBrushTurquoise = new SolidColorBrush(Color.FromRgb(100, 149, 237));
        panelPipes.SetTitleBarBackground(panelBackgroundBrushTurquoise);
        panelPipes.AddPushButton<Pipes.Commands.StartupCommand>("Pipeline")
            .SetImage("/BIMExpertise;component/Resources/Icons/PipesIcon32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/PipesIcon32.png");
        panelPipes.AddPushButton<PipeInsulation.Commands.StartupCommand>("Pipeline\ninsulation")
            .SetImage("/BIMExpertise;component/Resources/Icons/PipesIsolation32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/PipesIsolation32.png");
        panelPipes.AddPushButton<PipesFittings.Commands.StartupCommand>("Pipeline\nFittings")
            .SetImage("/BIMExpertise;component/Resources/Icons/PipesFittings32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/PipesFittings32.png");
        panelPipes.AddPushButton<Valve.Commands.StartupCommand>("Valve")
            .SetImage("/BIMExpertise;component/Resources/Icons/Valve32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Valve32.png");
        panelPipes.AddPushButton<SpaceHeater.Commands.StartupCommand>("Space\nHeater")
            .SetImage("/BIMExpertise;component/Resources/Icons/SpaceHeater32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/SpaceHeater32.png");
        
        
        var panelVentilation = Application.CreatePanel("Ventilation systems", "BIMExpertise");
        var panelBackgroundVentilation = new SolidColorBrush(Color.FromRgb(204, 204, 0));
        panelVentilation.SetTitleBarBackground(panelBackgroundVentilation);
        panelVentilation.AddPushButton<Duct.Commands.StartupCommand>("Duct")
            .SetImage("/BIMExpertise;component/Resources/Icons/DuctIcon32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/DuctIcon32.png");
        panelVentilation.AddPushButton<DuctInsolation.Command.StartupCommand>("Duct\ninsulation")
            .SetImage("/BIMExpertise;component/Resources/Icons/DuctIsolation32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/DuctIsolation32.png");
        //panelVentilation.AddPushButton<DuctFittings.Commands.StartupCommand>("Duct\nfittings")
          //  .SetImage("/BIMExpertise;component/Resources/Icons/DuctFittings32.png")
          //  .SetLargeImage("/BIMExpertise;component/Resources/Icons/DuctFittings32.png");
        panelVentilation.AddPushButton<AirTerminal.Commands.StartupCommand>("Air\nterminal")
            .SetImage("/BIMExpertise;component/Resources/Icons/AirTerminal32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/AirTerminal32.png");
        panelVentilation.AddPushButton<DuctFan.Commands.StartupCommand>("Fan")
            .SetImage("/BIMExpertise;component/Resources/Icons/Fan32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Fan32.png");

        // AR/CR
        var panelAr = Application.CreatePanel("Architecture/construction", "BIMExpertise");
        var panelBackgroundBrushPurple =
            new SolidColorBrush(Color.FromRgb(204, 204, 255));
        panelAr.SetTitleBarBackground(panelBackgroundBrushPurple);
        panelAr.AddPushButton<Walls.Commands.StartupCommand>("Walls")
            .SetImage("/BIMExpertise;component/Resources/Icons/Wall32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Wall32.png");
        panelAr.AddPushButton<Floor.Commands.StartupCommand>("Floor")
            .SetImage("/BIMExpertise;component/Resources/Icons/Wall32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Wall32.png");
    }
}
using System.Windows.Media;
using BIMExpertise.Commands;
using Nice3point.Revit.Toolkit.External;
using Wpf.Ui.Appearance;
using WpfResourcesBimExpertise.Services.Appearance;
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
        ApplyResources();
    }

    private void CreateRibbon()
    {
        
        // GENERAL
        var panelGeneral = Application.CreatePanel("General", "BIMExpertise");
        var panelBackgroundBrushLightCoral = new SolidColorBrush(Color.FromRgb(225, 174, 148));
        panelGeneral.SetTitleBarBackground(panelBackgroundBrushLightCoral);
        panelGeneral.AddPushButton<StartupCommand>("Execute")
            .SetImage("/BIMExpertise;component/Resources/Icons/RibbonIcon16.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/RibbonIcon32.png");

        // Pipe
        var panelPipes = Application.CreatePanel("Pipeline systems", "BIMExpertise");
        var panelBackgroundBrushTurquoise = new SolidColorBrush(Color.FromRgb(172, 229, 238));
        panelPipes.SetTitleBarBackground(panelBackgroundBrushTurquoise);
        panelPipes.AddPushButton<Pipes.Commands.StartupCommand>("Pipeline")
            .SetImage("/BIMExpertise;component/Resources/Icons/PipesIcon32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/PipesIcon32.png");
        panelPipes.AddPushButton<PipesFittings.Commands.StartupCommand>("Pipeline\nFittings")
            .SetImage("/BIMExpertise;component/Resources/Icons/PipesFittings32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/PipesFittings32.png");
        panelPipes.AddPushButton<Valve.Commands.StartupCommand>("Valve")
            .SetImage("/BIMExpertise;component/Resources/Icons/Valve32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Valve32.png");
        panelPipes.AddPushButton<SpaceHeater.Commands.StartupCommand>("Space\nHeater")
            .SetImage("/BIMExpertise;component/Resources/Icons/SpaceHeater32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/SpaceHeater32.png");
        
        // Ventilation
        var panelVentilation = Application.CreatePanel("Ventilation systems", "BIMExpertise");
        var panelBackgroundVentilation = new SolidColorBrush(Color.FromRgb(219, 112, 147));
        panelVentilation.SetTitleBarBackground(panelBackgroundVentilation);
        panelVentilation.AddPushButton<Duct.Commands.StartupCommand>("Duct")
            .SetImage("/BIMExpertise;component/Resources/Icons/DuctIcon32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/DuctIcon32.png");
        //panelVentilation.AddPushButton<DuctFittings.Commands.StartupCommand>("Duct\nfittings")
          //  .SetImage("/BIMExpertise;component/Resources/Icons/DuctFittings32.png")
          //  .SetLargeImage("/BIMExpertise;component/Resources/Icons/DuctFittings32.png");
        panelVentilation.AddPushButton<AirTerminal.Commands.StartupCommand>("Air\nterminal")
            .SetImage("/BIMExpertise;component/Resources/Icons/AirTerminal32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/AirTerminal32.png");
        panelVentilation.AddPushButton<DuctFan.Commands.StartupCommand>("Fan")
            .SetImage("/BIMExpertise;component/Resources/Icons/Fan32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Fan32.png");
        
        // Mep general
        var panelMepGeneral = Application.CreatePanel("MEPGeneral", "BIMExpertise");
        var panelBackgroundMepGeneral = new SolidColorBrush(Color.FromRgb(80, 200, 120));
        panelMepGeneral.SetTitleBarBackground(panelBackgroundMepGeneral);
        panelMepGeneral.AddPushButton<Insulation.Commands.StartupCommand>("Insulation")
            .SetImage("/BIMExpertise;component/Resources/Icons/PipesIsolation32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/PipesIsolation32.png");

        // AR/CR
        var panelAr = Application.CreatePanel("Architecture/construction", "BIMExpertise");
        var panelBackgroundBrushPurple =
            new SolidColorBrush(Color.FromRgb(204, 204, 255));
        panelAr.SetTitleBarBackground(panelBackgroundBrushPurple);
        panelAr.AddPushButton<Walls.Commands.StartupCommand>("Walls")
            .SetImage("/BIMExpertise;component/Resources/Icons/Wall32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Wall32.png");
        panelAr.AddPushButton<Floor.Commands.StartupCommand>("Floor")
            .SetImage("/BIMExpertise;component/Resources/Icons/Floor32.png")
            .SetLargeImage("/BIMExpertise;component/Resources/Icons/Floor32.png");
    }

    private static void ApplyResources()
    {
        ThemeWatcherService.Initialize();
        ThemeWatcherService.ApplyTheme(ApplicationTheme.Dark);
       
    }
}
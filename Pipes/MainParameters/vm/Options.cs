using System.Windows;

namespace Pipes.MainParameters.vm;

internal class Options
{
    public string Name { get; }
    public FilteredElementCollector Fec;

    public Options(string name, FilteredElementCollector filteredElementCollector)
    {
        Name = name;
        Fec = filteredElementCollector;
    }
}

public partial class Options2 : ObservableObject
{
    public string Name { get; }
    
    [ObservableProperty]
    private GridLength _widthComboBox;
    
    public Options2(string name, GridLength widthComboBox)
    {
        Name = name;
        this.WidthComboBox = widthComboBox;
    }
}
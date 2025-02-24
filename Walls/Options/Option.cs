namespace Walls.Options;

internal class SelectionOption
{
    public string Name { get; set; }
    public FilteredElementCollector Fec { get; set; }

    internal SelectionOption(Document doc, string name, bool isView)
    {
        Name = name;
        Fec = isView ? new FilteredElementCollector(doc, doc.ActiveView.Id) : new FilteredElementCollector(doc);
    }
}

internal class SelectionOptionParameter
{
    public string Name { get; set; }
    public double WidthComboBox {get; set;}

    internal SelectionOptionParameter(string name, double widthComboBox)
    {
        Name = name;
        WidthComboBox = widthComboBox;
    }
}

namespace Insulation.ViewModels.Option;

public class SelectionOption
{
    public SelectionOption(Document doc, string name, bool isView)
    {
        Name = name;
        Fec = isView ? new FilteredElementCollector(doc, doc.ActiveView.Id) : new FilteredElementCollector(doc);
    }

    public string Name { get; set; }
    public FilteredElementCollector Fec { get; set; }
}
using ChecklistApp.Model;

namespace ChecklistApp.Services;

public interface IFileService
{
    public void ExportChecklist(Checklist checklist, string fileName);
    public void ExportChecklists(IEnumerable<Checklist> checklists, string fileName);
    public Checklist ImportChecklist();
    public List<Checklist> ImportChecklists();
}
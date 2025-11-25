using Android.App;
using Android.Content;
using AndroidX.Activity.Result.Contract;
using ChecklistApp.Model;
using Application = Microsoft.Maui.Controls.Application;

namespace ChecklistApp.Services;

public class AndroidFileService : IFileService
{
    public void ExportChecklist(Checklist checklist, string fileName = "checklist.json")
    {
        Intent intent = new Intent(Intent.ActionCreateDocument);
        intent.AddCategory(Intent.CategoryOpenable);
        intent.SetType("application/json");
        intent.PutExtra(Intent.ExtraTitle, fileName);

        Activity activity = Platform.CurrentActivity;
        activity?.StartActivityForResult(intent, 1); 
    }

    public void ExportChecklists(IEnumerable<Checklist> checklists, string fileName)
    {
        throw new NotImplementedException();
    }

    public Checklist ImportChecklist()
    {
        throw new NotImplementedException();
    }

    public List<Checklist> ImportChecklists()
    {
        throw new NotImplementedException();
    }

    public void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
        
    }
}
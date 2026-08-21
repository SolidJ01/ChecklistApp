namespace ChecklistApp.Model;

public class ColorsUpdatedEventArgs(List<int>affectedIds) : EventArgs
{
    public List<int> AffectedIds => affectedIds;
}
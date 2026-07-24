using System.ComponentModel.DataAnnotations.Schema;

namespace ChecklistApp.Model;

public class ChecklistColor
{
    public int Id { get; set; }

    public float Red { get; set; }
    public float Green { get; set; }
    public float Blue { get; set; }
}
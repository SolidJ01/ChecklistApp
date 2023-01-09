using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Model
{
    public class Checklist
    {
        public enum ChecklistColor
        {
            Grey, 
            Cyan, 
            Blue, 
            Purple, 
            Magenta, 
            Red, 
            Orange, 
            Green
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public ChecklistColor Color { get; set; }
        public bool UseDeadline { get; set; }
        public DateTime Deadline { get; set; }
        public List<Item> Items { get; set; }

        //public Checklist()
        //{
        //    Name = "";
        //    Color = ChecklistColor.Grey;
        //    UseDeadline = false;
        //    Deadline = DateTime.Now;
        //    Items = new List<Item>();
        //}
    }
}

using ChecklistApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.ViewModel
{
    public class ChecklistCardViewModel
    {
        private Checklist _checklist;

        #region properties

        public int Id { get { return _checklist.Id; } }

        public string Name { get { return _checklist.Name; } }

        public string ItemsStatus
        {
            get
            {
                //return _checklist.Items != null ? "Yes" : "No";
                return _checklist.Items != null && _checklist.Items.Any()
                    ? _checklist.Items.Where(x => x.IsChecked).ToList().Any() 
                        ? $"{_checklist.Items.Where(x => x.IsChecked).ToList().Count}/{_checklist.Items.Count} Items" 
                        : $"{_checklist.Items.Count} Items" 
                    : "No Items Yet";
            }
        }

        public string DeadlineStatus
        {
            get
            {
                return _checklist.UseDeadline
                    ? (_checklist.Deadline.Date - DateTime.Now.Date).TotalDays >= 1
                        ? $"{(int)(_checklist.Deadline - DateTime.Now).TotalDays}d left"
                        : $"{(int)(_checklist.Deadline - DateTime.Now).TotalHours}h left"
                    : string.Empty;
            }
        }

        #endregion
        public ChecklistCardViewModel(Checklist checklist)
        {
            _checklist = checklist;
        }
    }
}

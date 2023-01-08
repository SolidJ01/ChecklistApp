using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.ViewModel
{
    public class ViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged = (sender, Easing) => { };

        public void OnPropertyChanged([CallerMemberName] string property = null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(property));
        }
    }
}

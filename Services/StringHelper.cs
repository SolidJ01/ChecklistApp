using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Services
{
    public static class StringHelper
    {
        public static string FormatItemName(string name)
        {
            return name.ToLower().Titleize();
        }
    }
}

using Humanizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Model;

namespace ChecklistApp.Services
{
    public static class StringHelper
    {
        public static string FormatItemName(string name)
        {
            return name.ToLower().Titleize();
        }

        public static string GenerateNotificationTitle(Notification notification)
        {
            return $"Deadline - {notification.Checklist.Name}";
        }

        public static string GenerateNotificationMessage(Notification notification)
        {
            return $"{notification.Value.Humanize()} remaining";
        }
    }
}

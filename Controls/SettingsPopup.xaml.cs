using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Controls;

public partial class SettingsPopup : Popup
{
    public SettingsPopup()
    {
        InitializeComponent();
    }

    private void CloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }
}
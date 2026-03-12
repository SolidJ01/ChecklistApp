using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChecklistApp.Model;
using ChecklistApp.ViewModel;

namespace ChecklistApp.Controls;

public partial class CreateChecklistPopup : Popup
{
    public static readonly BindableProperty ViewModelProperty = BindableProperty.Create(nameof(ViewModel), typeof(CreateChecklistPopupViewModel), typeof(CreateChecklistPopup));

    public CreateChecklistPopupViewModel ViewModel
    {
        get => (CreateChecklistPopupViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }
    
    public CreateChecklistPopup()
    {
        InitializeComponent();
    }

    protected override void Back()
    {
        Close(() => ViewModel.CancelCommand.Execute(null));
    }

    public override void Open()
    {
        ViewModel.ResetChecklist();
        base.Open();
    }

    protected override void CloseButtonClicked(object sender, EventArgs e)
    {
        Back();
    }

    private void ImportButtonClicked(object sender, EventArgs e)
    {
        ViewModel.ImportCommand.Execute(() =>
        {
            Close();
        });
    }

    private void SaveButtonClicked(object sender, EventArgs e)
    {
        ViewModel.SaveCommand.Execute(Back);
    }
}
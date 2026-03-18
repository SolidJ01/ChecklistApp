using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChecklistApp.Controls;

public partial class Toast : ContentView
{
    public event EventHandler ToastCompleted;
    
    private string _message = "Cheers!";

    public string Message
    {
        get => _message;
        set
        {
            _message = value;
            OnPropertyChanged();
        }
    }
    
    public Toast()
    {
        InitializeComponent();
        this.SizeChanged += (sender, args) =>
        {
            Box.TranslationY = this.Height - Box.Y;
        };
    }

    private async Task PopIn()
    {
        await Box.TranslateTo(Box.TranslationX, 0, easing: Easing.SpringOut);
        await Task.Delay(5000).ContinueWith(_ => PopOut());
    }

    private async Task PopOut()
    {
        await Box.TranslateTo(Box.TranslationX, @this.Height - Box.Y, easing: Easing.SpringIn);
        ToastCompleted?.Invoke(this, EventArgs.Empty);
    }

    public void PerformToast(string message)
    {
        Message = message;
        PopIn();
    }
}
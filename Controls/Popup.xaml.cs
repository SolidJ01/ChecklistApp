using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChecklistApp.Controls;

public partial class Popup : ContentView
{
    private static readonly double s_BaseOpacity = 0.5;
    private static readonly double s_BaseScale = 1;
    
    private double _opacity;
    private double _scale;

    public double Opacity
    {
        get
        {
            return _opacity;
        }
        private set
        {
            _opacity = value;
            OnPropertyChanged(nameof(Opacity));
        }
    }

    public double Scale
    {
        get
        {
            return _scale;
        }
        private set
        {
            _scale = value;
            OnPropertyChanged(nameof(Scale));
        }
    }

    public ICommand CloseCommand { get; private set; }
    
    public Popup()
    {
        _opacity = s_BaseOpacity;
        _scale = s_BaseScale;
        CloseCommand = new Command(Close);
        InitializeComponent();
    }

    public void Open()
    {
        
    }

    public async void Close()
    {
        var animation = new Animation(x => Opacity = x, s_BaseOpacity, 0);
        animation.Commit(this, "OverlayHide", 16, 500, Easing.CubicInOut);
        
        var popupAnimation = new Animation(x => Scale = x, s_BaseScale, 0);
        popupAnimation.Commit(this, "PopupScale", 16, 500, Easing.SpringIn, (c, v) => { this.IsVisible = false; });
        
    }

    private void OverlayTapped(object sender, TappedEventArgs e)
    {
        Close();
    }
}
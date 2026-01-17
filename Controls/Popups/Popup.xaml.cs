using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChecklistApp.Controls;

public partial class Popup : ContentView
{
    public static readonly BindableProperty BackgroundOpacityProperty = BindableProperty.Create(nameof(BackgroundOpacity), typeof(double), typeof(Popup), 0.5);
    public static readonly BindableProperty PopupMarginProperty = BindableProperty.Create(nameof(PopupMargin), typeof(Thickness), typeof(Popup), new Thickness(10, 25));
    //private static readonly double s_BaseOpacity = 0.5;
    private static readonly double s_BaseScale = 1;
    private static readonly uint s_AnimRate = 16U;

    protected Action<Action> _backButtonDeregisterCallback;
    
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

    public double BackgroundOpacity
    {
        get => (double)GetValue(BackgroundOpacityProperty);
        set => SetValue(BackgroundOpacityProperty, value);
    }

    public Thickness PopupMargin
    {
        get => (Thickness)GetValue(PopupMarginProperty);
        set => SetValue(PopupMarginProperty, value);
    }

    public ICommand CloseCommand { get; private set; }
    
    public Popup()
    {
        _opacity = 0;
        _scale = 0;
        //this.IsVisible = false;
        InputTransparent = true;
        CloseCommand = new Command<Action>(Close);
        InitializeComponent();
    }

    public void Open()
    {
        //this.IsVisible = true;
        this.InputTransparent = false;
        
        var backgroundAnimation = new Animation(x => Opacity = x, 0, BackgroundOpacity);
        backgroundAnimation.Commit(this, "PopupOpacity", s_AnimRate, 500, Easing.CubicInOut);

        var popupAnimation = new Animation(x => Scale = x, 0, s_BaseScale);
        popupAnimation.Commit(this, "PopupScale", s_AnimRate, 500, Easing.SpringOut);
    }

    public virtual void Open(Action<Action> backButtonRegisterCallback, Action<Action> backButtonDeregisterCallback)
    {
        _backButtonDeregisterCallback = backButtonDeregisterCallback;
        Open();
    }

    public async void Close(Action callback = null)
    {
        var animation = new Animation(x => Opacity = x, BackgroundOpacity, 0);
        animation.Commit(this, "OverlayHide", s_AnimRate, 500, Easing.CubicInOut);
        
        var popupAnimation = new Animation(x => Scale = x, s_BaseScale, 0);
        popupAnimation.Commit(this, "PopupScale", s_AnimRate, 500, Easing.SpringIn, (c, v) => { this.InputTransparent = true; callback?.Invoke(); });
        
    }

    protected void QuickClose()
    {
        Close();
    }

    protected virtual void CloseButtonClicked(object sender, EventArgs e)
    {
        Close();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChecklistApp.Controls;
using ChecklistApp.Controls.Popups;
using ChecklistApp.Model;
using ChecklistApp.Services;

namespace ChecklistApp.View;

public partial class DialoguePage : PopupPage
{
    private ToastService _toastService;
    private Dictionary<object, Rect> _toastSpacingLayers = [];
    
    public DialoguePage(ToastService toastService)
    {
        _toastService = toastService;
        InitializeComponent();
        ((Toast)GetTemplateChild("Toast")).ToastCompleted += OnToastCompleted;
    }

    private void CheckQueuedToasts()
    {
        if (_toastService._toastsQueued)
            PerformToast();
    }

    private void PerformToast()
    {
        ((Toast)GetTemplateChild("Toast")).PerformToast(_toastService.GetToast());
    }

    private void ToastServiceOnToastProposed(object sender, EventArgs e)
    {
        PerformToast();
    }

    private void OnToastCompleted(object sender, EventArgs e)
    {
        CheckQueuedToasts();
    }

    protected void OnToastAnchorChanged(object sender, ToastAnchorChangeEventArgs e)
    {
        switch (e.EventIntent)
        {
            case ToastAnchorChangeEventArgs.Intent.Apply:
                _toastSpacingLayers.Add(sender, e.Value);
                ((Toast)GetTemplateChild("Toast")).AdjustToNewAnchor(_toastSpacingLayers.Last().Value);
                break;
            case ToastAnchorChangeEventArgs.Intent.Remove:
                _toastSpacingLayers.Remove(sender);
                if (_toastSpacingLayers.Count > 0)
                {
                    ((Toast)GetTemplateChild("Toast")).AdjustToNewAnchor(_toastSpacingLayers.Last().Value);
                }
                else
                {
                    ((Toast)GetTemplateChild("Toast")).ResetAnchoring();
                }
                break;
        }
    }

    protected void OnQueryDialogue(object sender, DialogueQueryEventArgs e)
    {
        ((DialoguePopup)GetTemplateChild("Dialogue")).Prompt(e);
    }

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        _toastService.ToastProposed += ToastServiceOnToastProposed;
        CheckQueuedToasts();
    }

    protected override void OnNavigatedFrom(NavigatedFromEventArgs args)
    {
        base.OnNavigatedFrom(args);
        _toastService.ToastProposed -= ToastServiceOnToastProposed;
    }
}
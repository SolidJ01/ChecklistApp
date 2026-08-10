using System.Globalization;
using ChecklistApp.Model;
using ChecklistApp.Services;

namespace ChecklistApp.ValueConverters;

public class ChecklistColorToBrushConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        Checklist.ChecklistColor color = Checklist.ChecklistColor.Grey;
        int? customColorId = null;
        foreach (var item in values)
        {
            if (item is Checklist.ChecklistColor colorItem)
                color = colorItem;
            else if (item is int intColor)
                customColorId = intColor;
        }

        string resourceKey = "";

        switch (color)
        {
            case Checklist.ChecklistColor.Grey:
                resourceKey = "ForegroundBrush";
                break;
            case Checklist.ChecklistColor.Cyan:
                resourceKey = "ForegroundCyanBrush";
                break;
            case Checklist.ChecklistColor.Blue:
                resourceKey = "ForegroundBlueBrush";
                break;
            case Checklist.ChecklistColor.Purple:
                resourceKey = "ForegroundPurpleBrush";
                break;
            case Checklist.ChecklistColor.Magenta:
                resourceKey = "ForegroundMagentaBrush";
                break;
            case Checklist.ChecklistColor.Red:
                resourceKey = "ForegroundRedBrush";
                break;
            case Checklist.ChecklistColor.Orange:
                resourceKey = "ForegroundOrangeBrush";
                break;
            case Checklist.ChecklistColor.Green:
                resourceKey = "ForegroundGreenBrush";
                break;
            case Checklist.ChecklistColor.Yellow:
                resourceKey = "ForegroundYellowBrush";
                break;
            case Checklist.ChecklistColor.Custom:
                resourceKey = $"{ColorService.S_CustomColourString}{customColorId}";
                break;
        }

        if (Application.Current.Resources.TryGetValue(resourceKey, out var resource))
        {
            return resource as Brush;
        }
        else
        {
            throw new ArgumentException($"Resource {resourceKey} not found");
        }
    }
    
    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
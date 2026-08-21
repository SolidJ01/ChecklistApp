using ChecklistApp.Data;
using ChecklistApp.Model;

namespace ChecklistApp.Services;

public class ColorService
{
    public static readonly string S_CustomColourString = "CustomColour";
    public event EventHandler<ColorSelectRequestEventArgs> ColorSelectRequested;
    public event EventHandler<ColorsUpdatedEventArgs> ColorsUpdated;
    
    private readonly ChecklistContext _context;
    private ResourceDictionary _customColourDictionary;

    public ColorService(ChecklistContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Initializes the custom colour resource dictionary, loads all custom colours into it, and appends it to the applications merged dictionaries
    /// </summary>
    public void Initialize()
    {
        var customColours = _context.GetColorsAsync().Result;
		
        _customColourDictionary = new ResourceDictionary();
        foreach (var color in customColours)
        {
            _customColourDictionary.Add($"{S_CustomColourString}{color.Id}", new Color(color.Red,  color.Green, color.Blue));
        }

        Application.Current?.Resources.MergedDictionaries.Add(_customColourDictionary);
    }

    /// <summary>
    /// Refreshes the custom colour resource dictionary. Use when custom colours have been added or removed from the database
    /// </summary>
    public void Update()
    {
        var customColours = _context.GetColorsAsync().Result;
        List<int> affectedIds = [];
        foreach (var customColor in customColours)
        {
            Color color = new Color(customColor.Red, customColor.Green, customColor.Blue);
            if (!_customColourDictionary.ContainsKey($"{S_CustomColourString}{customColor.Id}"))
            {
                _customColourDictionary.Add($"{S_CustomColourString}{customColor.Id}", color);
                affectedIds.Add(customColor.Id);
            }
            else if (_customColourDictionary.TryGetValue($"{S_CustomColourString}{customColor.Id}", out var customColour) && customColour is Color c && !c.Equals(color))
            {
                _customColourDictionary[$"{S_CustomColourString}{customColor.Id}"] = new Color(customColor.Red, customColor.Green, customColor.Blue);
                affectedIds.Add(customColor.Id);
            }
        }

        foreach (var key in _customColourDictionary.Keys.Where(key => key.Contains(S_CustomColourString)))
        {
            int id = int.Parse(key.Split(S_CustomColourString, StringSplitOptions.RemoveEmptyEntries)[0]);
            if (customColours.All(x => x.Id != id))
            {
                _customColourDictionary.Remove(key);
                affectedIds.Add(id);
            }
        }
        
        ColorsUpdated?.Invoke(this, new ColorsUpdatedEventArgs(affectedIds));
    }

    public void RequestColourCreation(Action<int> callback)
    {
        Color? newColour = null;
        if (Application.Current.Resources.TryGetValue("Foreground", out var resource))
            newColour = (Color)resource;
        ColorSelectRequested?.Invoke(this, new ColorSelectRequestEventArgs(newColour, (Color c) => CreateNewColour(c,  callback)));
    }

    public async void RequestColourEditing(int id, Action callback)
    {
        ChecklistColor color = await _context.GetColorAsync(id);
        ColorSelectRequested?.Invoke(this, new ColorSelectRequestEventArgs(Color.FromRgb(color.Red, color.Green, color.Blue), (Color c) => EditColour(id, c, callback), () => DeleteColour(id, callback)));
    }

    private void CreateNewColour(Color color, Action<int> callback)
    {
        ChecklistColor newColor = new ChecklistColor
        {
            Red = color.Red,
            Blue = color.Blue,
            Green = color.Green
        };
        _context.CreateColor(newColor);
        Update();
        callback?.Invoke(newColor.Id);
    }

    private void EditColour(int id, Color color, Action? callback)
    {
        ChecklistColor editedColor = _context.GetColorAsync(id).Result;
        editedColor.Red = color.Red;
        editedColor.Green = color.Green;
        editedColor.Blue = color.Blue;
        _context.UpdateColor(editedColor);
        Update();
        callback?.Invoke();
    }

    private void DeleteColour(int id, Action callback)
    {
        _ = _context.DeleteColorAsync(id);
        Update();
        callback?.Invoke();
    }

    public static string CalculateResourceKey(Checklist.ChecklistColor color, int? customColorId = null)
    {
        switch (color)
        {
            case Checklist.ChecklistColor.Grey:
                return "Foreground";
            case Checklist.ChecklistColor.Cyan:
                return "ForegroundCyan";
            case Checklist.ChecklistColor.Blue:
                return "ForegroundBlue";
            case Checklist.ChecklistColor.Purple:
                return "ForegroundPurple";
            case Checklist.ChecklistColor.Magenta:
                return "ForegroundMagenta";
            case Checklist.ChecklistColor.Red:
                return "ForegroundRed";
            case Checklist.ChecklistColor.Orange:
                return "ForegroundOrange";
            case Checklist.ChecklistColor.Yellow:
                return "ForegroundYellow";
            case Checklist.ChecklistColor.Green:
                return "ForegroundGreen";
            case Checklist.ChecklistColor.Custom:
                return customColorId is not null ? $"{S_CustomColourString}{customColorId}" : "Foreground";
        }

        return null;
    }

    public static string CalculateBrushResourceKey(Checklist.ChecklistColor color, int? customColorId = null)
    {
        return $"{CalculateResourceKey(color, customColorId)}Brush";
    }
}
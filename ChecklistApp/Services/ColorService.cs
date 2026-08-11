using ChecklistApp.Data;
using ChecklistApp.Model;

namespace ChecklistApp.Services;

public class ColorService
{
    public static readonly string S_CustomColourString = "CustomColour";
    public event EventHandler<ColorSelectRequestEventArgs> ColorSelectRequested;
    
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
            _customColourDictionary.Add($"{S_CustomColourString}{color.Id}", new SolidColorBrush(new Color(color.Red,  color.Green, color.Blue)));
        }

        Application.Current?.Resources.MergedDictionaries.Add(_customColourDictionary);
    }

    /// <summary>
    /// Refreshes the custom colour resource dictionary. Use when custom colours have been added or removed from the database
    /// </summary>
    public void Update()
    {
        var customColours = _context.GetColorsAsync().Result;
        foreach (var color in customColours)
        {
            if (!_customColourDictionary.ContainsKey($"{S_CustomColourString}{color.Id}"))
            {
                _customColourDictionary.Add($"{S_CustomColourString}{color.Id}", new SolidColorBrush(new Color(color.Red,  color.Green, color.Blue)));
            }
        }

        foreach (var key in _customColourDictionary.Keys.Where(key => key.Contains(S_CustomColourString)))
        {
            int id = int.Parse(key.Split(S_CustomColourString, StringSplitOptions.RemoveEmptyEntries)[0]);
            if (customColours.All(x => x.Id != id))
            {
                _customColourDictionary.Remove(key);
            }
        }
    }

    public void RequestColourCreation(Action<int> callback)
    {
        ColorSelectRequested?.Invoke(this, new ColorSelectRequestEventArgs(null, null, (int? i, Color c) => CreateNewColour(c,  callback)));
    }

    public async void RequestColourEditing(int id, Action callback)
    {
        ChecklistColor color = await _context.GetColorAsync(id);
        ColorSelectRequested?.Invoke(this, new ColorSelectRequestEventArgs(id, Color.FromRgb(color.Red, color.Green, color.Blue), (int? i, Color c) => EditColour(id, c, callback)));
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
}
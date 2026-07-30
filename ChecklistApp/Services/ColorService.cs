using ChecklistApp.Data;

namespace ChecklistApp.Services;

public class ColorService
{
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
            _customColourDictionary.Add($"CustomColour{color.Id}", new SolidColorBrush(new Color(color.Red,  color.Green, color.Blue)));
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
            if (!_customColourDictionary.ContainsKey($"CustomColour{color.Id}"))
            {
                _customColourDictionary.Add($"CustomColour{color.Id}", new SolidColorBrush(new Color(color.Red,  color.Green, color.Blue)));
            }
        }

        foreach (var key in _customColourDictionary.Keys.Where(key => key.Contains("CustomColour")))
        {
            int id = int.Parse(key.Split("CustomColour", StringSplitOptions.RemoveEmptyEntries)[0]);
            if (customColours.All(x => x.Id != id))
            {
                _customColourDictionary.Remove(key);
            }
        }
    }
}
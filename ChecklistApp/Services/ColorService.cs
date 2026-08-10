using ChecklistApp.Data;

namespace ChecklistApp.Services;

public class ColorService
{
    public static readonly string S_CustomColourString = "CustomColour";
    
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
        
    }

    public void RequestColourEditing(int id, Action callback)
    {
        
    }

    private void AddColour(Color newColor, Action callback)
    {
        
    }

    private void EditColour(int id, Color newColor)
    {
        
    }
}
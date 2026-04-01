namespace ChecklistApp.Model.Remote;

public class Version
{
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }

    public static Version Parse(string version)
    {
        string[] parts = version.Split('.');
        switch (parts.Length)
        {
            case 1:
                return new Version { Major = int.Parse(parts[0]), Minor = 0, Patch = 0 };
            case 2:
                return new Version { Major = int.Parse(parts[0]), Minor = int.Parse(parts[1]), Patch = 0 };
            case 3:
                return new Version { Major = int.Parse(parts[0]), Minor = int.Parse(parts[1]), Patch = int.Parse(parts[2]) };
        }
        throw new FormatException("Invalid version format");
    }
    
    public static bool operator > (Version version1, Version version2)
    {
        return version1.Major.Equals(version2.Major) && version1.Minor.Equals(version2.Minor) && version1.Patch.Equals(version2.Patch)
            ? false
            : version1.Major > version2.Major
                ? true
                : version1.Major.Equals(version2.Major) && version1.Minor > version2.Minor
                    ? true
                    : version1.Major.Equals(version2.Major) && version1.Minor.Equals(version2.Minor) && version1.Patch > version2.Patch;
    }

    public static bool operator < (Version version1, Version version2)
    {
        return version2 > version1;
    }

    public override string ToString()
    {
        return $"{Major}.{Minor}.{Patch}";
    }

    public override bool Equals(object obj)
    {
        if (obj is not Version v)
            return false;
        return v.Major.Equals(Major) && v.Minor.Equals(Minor) && v.Patch.Equals(Patch);
    }
}
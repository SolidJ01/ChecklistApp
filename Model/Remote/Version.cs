namespace ChecklistApp.Model.Remote;

public class Version
{
    public int Major { get; set; }
    public int Minor { get; set; }
    public int Patch { get; set; }

    public Version(int major, int minor, int patch)
    {
        Major = major;
        Minor = minor;
        Patch = patch;
    }

    public static Version Parse(string version)
    {
        string[] parts = version.Split('.');
        switch (parts.Length)
        {
            case 1:
                return new Version(int.Parse(parts[0]), 0, 0);
            case 2:
                return new Version(int.Parse(parts[0]), int.Parse(parts[1]), 0);
            case 3:
                return new Version(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
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
}
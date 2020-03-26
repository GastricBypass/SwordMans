public enum VersusPlayMode
{
    Swords = 0,
    Guns = 1,
    Combined = 2
}

public static class VersusPlayModeHelper
{
    public static VersusPlayMode ParseFromString(string playModeName)
    {
        if (VersusPlayMode.Swords.ToString() == playModeName)
        {
            return VersusPlayMode.Swords;
        }

        if (VersusPlayMode.Guns.ToString() == playModeName)
        {
            return VersusPlayMode.Guns;
        }

        if (VersusPlayMode.Combined.ToString() == playModeName)
        {
            return VersusPlayMode.Combined;
        }

        return VersusPlayMode.Swords;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameConstants {

    public static class PlayerColors
    {
        public static Color red = Color.red;
        public static Color blue = Color.blue;
        public static Color green = Color.green;
        public static Color yellow = Color.yellow;
        public static Color orange = new Color(255f / 255f, 100f / 255f, 0f / 255f);
        public static Color purple = Color.magenta;

        // Returns a color by the given name if it exists, otherwise it returns black
        public static Color ParseFromName(string name)
        {
            string lowerCaseName = name.ToLower();
            Color color = Color.black;

            if (lowerCaseName == "red")
            {
                color = red;
            }
            else if (lowerCaseName == "blue")
            {
                color = blue;
            }
            else if (lowerCaseName == "green")
            {
                color = green;
            }
            else if (lowerCaseName == "yellow")
            {
                color = yellow;
            }
            else if (lowerCaseName == "orange")
            {
                color = orange;
            }
            else if (lowerCaseName == "purple")
            {
                color = purple;
            }

            return color;
        }
    }

    public static class SkinColors
    {
        public static Color light = new Color(255f / 255f, 209f / 255f, 180f / 255f);
        public static Color medium = new Color(200f / 255f, 140f / 255f, 70f / 255f);
        public static Color dark = new Color(60f / 255f, 30f / 255f, 0f / 255f);
        public static Color green = new Color(50f / 255f, 100f / 255f, 35f / 255f);

        // Returns a color by the given name if it exists, otherwise it returns black
        public static Color ParseFromName(string name)
        {
            string lowerCaseName = name.ToLower();
            Color color = Color.black;

            if (lowerCaseName == "light")
            {
                color = light;
            }
            else if (lowerCaseName == "medium")
            {
                color = medium;
            }
            else if (lowerCaseName == "dark")
            {
                color = dark;
            }
            else if (lowerCaseName == "green")
            {
                color = green;
            }

            return color;
        }
    }

    public static class Players
    {
        public static int playerMaxHealth = 1000;
    }

    public static class Sounds
    {
        public static string metalOnMetal = "metalOnMetal";
        public static string metalOnWood = "metalOnWood";
        public static string metalOnSoft = "metalOnSoft";
        public static string metalOnGlass = "metalOnGlass";

        public static string woodOnWood = "woodOnWood";
        public static string woodOnSoft = "woodOnSoft";
        public static string woodOnGlass = "woodOnGlass";

        public static string softOnSoft = "softOnSoft";
        public static string softOnGlass = "softOnGlass";

        public static string glassOnGlass = "glassOnGlass";
    }

    public static class Unlocks
    {
        public static List<string> startingHats = new List<string>(new string[] { "None", "Bandana", "Coif", "Helmet", "Hood", "Jester", "Noble" });
        public static List<string> startingMisc = new List<string>(new string[] { "None", "Belt", "Cape", "Spaulders" });

        public static List<string> startingVersusStages = new List<string>(new string[] { "Arena", "Castle", "Tavern", "Cabin", "Tower", "Ship", "Throne Room", "Pond", "Highlands", "Icebergs", "Bridge", "Market", "Volcano", "Colosseum", "Dungeon", "Mill" });
        public static List<string> startingCoopStages = new List<string>(new string[] { "Ch 1" });

        public static List<string> allHats = new List<string>(new string[] { "None", "Bandana", "Coif", "Helmet", "Hood", "Jester", "Noble", "Cap", "Crown", "Viking", "Space", "Tricorn", "Spartan", "Fedora", "Bowler", "Tophat", "Boater", "Shades", "Bucket", "Cone", "Sunhat", "Tiara", "Circlet", "Headband", "Elf Hat", "Mustache" });
        public static List<string> allMisc = new List<string>(new string[] { "None", "Belt", "Cape", "Spaulders", "Breastplate", "Armor", "Necklace", "Collar", "Ruff", "Spikes", "Dagger", "Equipment", "Vest", "Sweater Vest", "Skirt", "Tabard", "Loincloth", "Poncho" });

        public static List<string> allVersusStages = new List<string>(new string[] { "Arena", "Castle", "Tavern", "Cabin", "Tower", "Ship", "Space Station", "Throne Room", "Pond", "Highlands", "Icebergs", "Bridge", "Market", "Volcano", "Colosseum", "Dungeon", "Mill" });
        public static List<string> allCoopStages = new List<string>(new string[] { "Ch 1", "Ch 1 Part 1", "Ch 1 Part 2", "Ch 1 Part 3"});
    }

    public static class Files
    {
        public static string cosmeticsFileName = "unlocks.dat";
        public static string settingsFileName = "settings.dat";
    }
}

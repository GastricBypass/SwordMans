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
            else if (lowerCaseName == "random")
            {
                int randomNumber = Random.Range(0, 3);
                if (randomNumber == 0)
                {
                    color = light;
                }
                else if (randomNumber == 1)
                {
                    color = medium;
                }
                else if (randomNumber == 2)
                {
                    color = dark;
                }
            }

            return color;
        }
    }

    public static class UIColors
    {
        public static Color red = new Color(255f / 255f, 50f / 255f, 50f / 255f);
        public static Color blue = new Color(60f / 255f, 60f / 255f, 255f / 255f);
        public static Color green = Color.green;
        public static Color yellow = new Color(200f / 255f, 200f / 255f, 0f / 255f);
        public static Color orange = new Color(255f / 255f, 100f / 255f, 0f / 255f);
        public static Color purple = Color.magenta;

        public static Color ParseFromName(string name, int transparency = 255)
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

            return new Color(color.r, color.g, color.b, transparency / 255f);
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
        public static List<string> startingHats = new List<string>(new string[] { "None", "Bandana", "Headband", "Bun Black", "Messy Black", "Ponytail Black" });
        public static List<string> startingMisc = new List<string>(new string[] { "None", "Belt", "Shirt", "Scarf", "Backpack", "Loincloth" });

        public static List<string> startingVersusStages = new List<string>(new string[] { "Joust", "Castle", "Tavern", "Cabin", "Tower", "Ship", "Throne Room", "Pond", "Highlands", "Icebergs", "Bridge", "Market", "Volcano", "Colosseum", "Dungeon", "Mill", "Mansion" });
        public static List<string> startingCoopStages = new List<string>(new string[] { "Ch 1" });
        public static List<string> startingArenaStages = new List<string>(new string[] { "Stadium" });

        public static List<string> allHats = new List<string>(new string[] { "None", "Bandana", "Coif", "Helmet", "Hood", "Jester", "Noble", "Cap", "Crown", "Viking", "Space", "Tricorn", "Spartan", "Fedora", "Bowler", "Tophat", "Boater", "Shades", "Bucket", "Cone", "Sunhat", "Tiara", "Circlet", "Headband", "Elf Hat", "Mustache", "Wizard", "Squid", "Pirate", "Bun Black", "Messy Black", "Ponytail Black", "Bun Blond", "Messy Blond", "Ponytail Blond", "Bun Brown", "Messy Brown", "Ponytail Brown", "Bun Red", "Messy Red", "Ponytail Red" });
        public static List<string> allMisc = new List<string>(new string[] { "None", "Belt", "Cape", "Spaulders", "Breastplate", "Armor", "Necklace", "Collar", "Ruff", "Spikes", "Dagger", "Equipment", "Vest", "Sweater Vest", "Skirt", "Tabard", "Loincloth", "Poncho", "Dress", "Shirt", "Tuxedo", "Scarf", "Backpack", "Lantern" });

        public static List<string> allVersusStages = new List<string>(new string[] { "Joust", "Castle", "Tavern", "Cabin", "Tower", "Ship", "Space Station", "Throne Room", "Pond", "Highlands", "Icebergs", "Bridge", "Market", "Volcano", "Colosseum", "Dungeon", "Mill", "Mansion" });
        public static List<string> allCoopStages = new List<string>(new string[] { "Ch 1", "Ch 1 Part 1", "Ch 1 Part 2", "Ch 1 Part 3", "Ch 1 Resolution", "Ch 2", "Ch 2 Part 1", "Ch 2 Part 2", "Ch 2 Part 3", "Ch 2 Resolution" });
        public static List<string> allArenaStages = new List<string>(new string[] { "Stadium" });

        public static List<string> allCoopGameModes = new List<string>(new string[] { "Arena", "Campaign" });

        public static List<string> purchasableHats = new List<string>(new string[] { "Bandana", "Coif", "Helmet", "Hood", "Jester", "Noble", "Cap", "Viking", "Tricorn", "Spartan", "Fedora", "Bowler", "Tophat", "Boater", "Shades", "Bucket", "Cone", "Sunhat", "Tiara", "Circlet", "Headband", "Elf Hat", "Mustache", "Wizard", "Bun Black", "Messy Black", "Ponytail Black", "Bun Blond", "Messy Blond", "Ponytail Blond", "Bun Brown", "Messy Brown", "Ponytail Brown", "Bun Red", "Messy Red", "Ponytail Red" });
        public static List<string> purchasableMisc = new List<string>(new string[] { "Belt", "Cape", "Spaulders", "Breastplate", "Armor", "Necklace", "Collar", "Ruff", "Spikes", "Dagger", "Equipment", "Vest", "Sweater Vest", "Skirt", "Tabard", "Loincloth", "Poncho", "Dress", "Shirt", "Tuxedo", "Scarf", "Backpack" });

        public static Dictionary<string, float> hatPrices = new Dictionary<string, float>
        {
            { "Bandana", 10 },
            { "Coif", 10 },
            { "Helmet", 80 },
            { "Hood", 30 },
            { "Jester", 100 },
            { "Noble", 80 },
            { "Cap", 10 },
            { "Viking", 50 },
            { "Tricorn", 50 },
            { "Spartan", 80 },
            { "Fedora", 100 },
            { "Bowler", 100 },
            { "Tophat", 100 },
            { "Boater", 80 },
            { "Shades", 50 },
            { "Bucket", 80 },
            { "Cone", 10 },
            { "Sunhat", 80 },
            { "Tiara", 100 },
            { "Circlet", 100 },
            { "Headband", 10 },
            { "Elf Hat", 80 },
            { "Mustache", 50 },
            { "Wizard", 100 },
            { "Bun Black", 10 },
            { "Messy Black", 10 },
            { "Ponytail Black", 10 },
            { "Bun Blond", 10 },
            { "Messy Blond", 10 },
            { "Ponytail Blond", 10 },
            { "Bun Brown", 10 },
            { "Messy Brown", 10 },
            { "Ponytail Brown", 10 },
            { "Bun Red", 10 },
            { "Messy Red", 10 },
            { "Ponytail Red", 10 }
        };

        public static Dictionary<string, float> miscPrices = new Dictionary<string, float>
        {
            { "Belt", 10 },
            { "Cape", 100 },
            { "Spaulders", 50 },
            { "Breastplate", 80 },
            { "Armor", 100 },
            { "Necklace", 100 },
            { "Collar", 80 },
            { "Ruff", 80 },
            { "Spikes", 50 },
            { "Dagger", 50 },
            { "Equipment", 100 },
            { "Vest", 80 },
            { "Sweater Vest", 80 },
            { "Skirt", 50 },
            { "Tabard", 100 },
            { "Loincloth", 50 },
            { "Poncho", 80 },
            { "Dress", 100 },
            { "Shirt", 20 },
            { "Tuxedo", 100 },
            { "Scarf", 20 },
            { "Backpack", 30 }
        };
    }

    public static class Files
    {
        public static string cosmeticsFileName = "unlocks.dat";
        public static string settingsFileName = "settings.dat";
    }

    public enum AchievementId : int
    {
        HEAVY_HITTER, // Done
        I_SHOULDNT_BE_ALIVE, // Done
        FREQUENT_FLYER, // Done
        FREQUENT_FRAGGER, // Done
        FREQUENT_FLEEER, // Done
        WELL_ROUNDED, // Done
        FRIENDS_MAKE_EVERYTHING_BETTER, // Done
        WARRIOR, // Done
        GLADIATOR, // Done
        HOARDER, // Done
        CONSUMER, // Done
        AIR_TIME, // Done
        NEW_KING, // Done
        WE_HAVE_LIFTOFF, // Done
        SCOURGE_OF_THE_SEWERS // Done
    };
}

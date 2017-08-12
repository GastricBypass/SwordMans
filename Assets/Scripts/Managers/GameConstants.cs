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
}

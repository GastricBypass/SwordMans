using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Steamworks;
using System.ComponentModel;

public class Steam : MonoBehaviour
{
    private class Achievement
    {
        public GameConstants.AchievementId m_eAchievementID;
        public string m_strName;
        public string m_strDescription;
        public bool m_bAchieved;

        /// <summary>
        /// Creates an Achievement. You must also mirror the data provided here in https://partner.steamgames.com/apps/achievements/yourappid
        /// </summary>
        /// <param name="achievement">The "API Name Progress Stat" used to uniquely identify the achievement.</param>
        /// <param name="name">The "Display Name" that will be shown to players in game and on the Steam Community.</param>
        /// <param name="desc">The "Description" that will be shown to players in game and on the Steam Community.</param>
        public Achievement(GameConstants.AchievementId achievementID, string name, string desc)
        {
            m_eAchievementID = achievementID;
            m_strName = name;
            m_strDescription = desc;
            m_bAchieved = false;
        }
    }

    private List<Achievement> m_Achievements = new List<Achievement>
    {
        new Achievement(GameConstants.AchievementId.HEAVY_HITTER, "Heavy Hitter", "Deal 500 damage in a single hit."),
        new Achievement(GameConstants.AchievementId.I_SHOULDNT_BE_ALIVE, "I Shouldn't Be Alive", "Heal 1,000 health in a single life."),
        new Achievement(GameConstants.AchievementId.FREQUENT_FLYER, "Frequent Flyer", "Use a total of 1,000 boost."),
        new Achievement(GameConstants.AchievementId.FREQUENT_FRAGGER, "Frequent Fragger", "Deal a total of 100,000 damage."),
        new Achievement(GameConstants.AchievementId.FREQUENT_FLEEER, "Frequent Flee-er", "Heal a total of 50,000 damage."),
        new Achievement(GameConstants.AchievementId.WELL_ROUNDED, "Well Rounded", "Play a complete versus match with all 10 weapons."),
        new Achievement(GameConstants.AchievementId.FRIENDS_MAKE_EVERYTHING_BETTER, "Friends Make Everything Better", "Play a complete versus match with 4 players."),
        new Achievement(GameConstants.AchievementId.WARRIOR, "Warrior", "Survive 5 waves in the arena."),
        new Achievement(GameConstants.AchievementId.GLADIATOR, "Gladiator", "Survive 10 waves in the arena."),
        new Achievement(GameConstants.AchievementId.HOARDER, "Hoarder", "Own 200 gold at one time."),
        new Achievement(GameConstants.AchievementId.CONSUMER, "Consumer", "Purchase an item from the store."),
        new Achievement(GameConstants.AchievementId.AIR_TIME, "Air Time", "Use 50 boost without touching the ground."),
        new Achievement(GameConstants.AchievementId.NEW_KING, "New King", "Usurp the throne."),
        new Achievement(GameConstants.AchievementId.WE_HAVE_LIFTOFF, "We Have Liftoff", "Boldly go where no sword man has gone before."),
        new Achievement(GameConstants.AchievementId.SCOURGE_OF_THE_SEWERS, "Scourge of the Sewers", "Complete chapter 1 of the campaign."),
        new Achievement(GameConstants.AchievementId.BARBARIANS_BANE, "Barbarian's Bane", "Complete chapter 2 of the campaign.")
    };

    protected Callback<GameOverlayActivated_t> m_GameOverlayActivated;

    // Our GameID
    private CGameID m_GameID;

    private Steam steamInstance;

    // Did we get the stats from Steam?
    private bool m_bRequestedStats;
    private bool m_bStatsValid;

    // Should we store stats this frame? Yes? Always?
    //private bool m_bStoreStats;

    // Persisted Stat details
    private float m_damageDealt;
    private float m_hpRecovered;
    private float m_boostUsed;
    private int m_weaponsUsed;

    protected Callback<UserStatsReceived_t> m_UserStatsReceived;
    protected Callback<UserStatsStored_t> m_UserStatsStored;
    protected Callback<UserAchievementStored_t> m_UserAchievementStored;

    private void OnEnable()
    {
        if (!SteamManager.Initialized)
        {
            return;
        }

        m_GameOverlayActivated = Callback<GameOverlayActivated_t>.Create(OnGameOverlayActivated);

        m_GameID = new CGameID(SteamUtils.GetAppID());

        m_UserStatsReceived = Callback<UserStatsReceived_t>.Create(OnUserStatsReceived);
        m_UserStatsStored = Callback<UserStatsStored_t>.Create(OnUserStatsStored);
        m_UserAchievementStored = Callback<UserAchievementStored_t>.Create(OnAchievementStored);

        // These need to be reset to get the stats upon an Assembly reload in the Editor.
        m_bRequestedStats = false;
        m_bStatsValid = false;

    }

    private void OnGameOverlayActivated(GameOverlayActivated_t pCallback)
    {
        if (pCallback.m_bActive != 0)
        {
            UIManager ui = FindObjectOfType<UIManager>();
            if (ui.gsm.currentStage != "Main Menu")
            {
                ui.StartPressed(); // To pause the game when the steam overlay is opened.
            }
        }
    }

    private void Update()
    {
        if (!SteamManager.Initialized)
            return;

        if (!m_bRequestedStats)
        {
            // Is Steam Loaded? if no, can't get stats, done
            if (!SteamManager.Initialized)
            {
                m_bRequestedStats = true;
                return;
            }

            // If yes, request our stats
            bool bSuccess = SteamUserStats.RequestCurrentStats();

            // This function should only return false if we weren't logged in, and we already checked that.
            // But handle it being false again anyway, just ask again later.
            m_bRequestedStats = bSuccess;
        }

        if (!m_bStatsValid)
            return;

        //Store stats in the Steam database if necessary
        //if (m_bStoreStats)
        //{
        //    // already set any achievements in UnlockAchievement

        //    // set stats
        //    SteamUserStats.SetStat("damage_dealt", m_damageDealt);
        //    SteamUserStats.SetStat("hp_recovered", m_hpRecovered);
        //    SteamUserStats.SetStat("boost_used", m_boostUsed);
        //    SteamUserStats.SetStat("weapons_used", m_weaponsUsed);

        //    bool bSuccess = SteamUserStats.StoreStats();
        //    // If this failed, we never sent anything to the server, try
        //    // again later.
        //    m_bStoreStats = !bSuccess;
        //}
    }

    public void UnlockAchievement(GameConstants.AchievementId achievementId)
    {
        Achievement achievement = m_Achievements.Find(x => x.m_eAchievementID == achievementId);

        UnlockAchievement(achievement);
    }

    private void UnlockAchievement(Achievement achievement)
    {
        if (achievement.m_bAchieved)
        {
            return; // No need to unlock if it's already achieved
        }

        achievement.m_bAchieved = true;

        // the icon may change once it's unlocked
        //achievement.m_iIconImage = 0;

        // mark it down
        SteamUserStats.SetAchievement(achievement.m_eAchievementID.ToString());

        // Store stats end of frame
        //m_bStoreStats = true;
    }

    public void AddBoostUsed(float boost)
    {
        m_boostUsed += boost;
        SteamUserStats.SetStat("boost_used", m_boostUsed);
        bool bSuccess = SteamUserStats.StoreStats();
        
        if (m_boostUsed >= 1000)
        {
            // Achievement: Frequent Flyer
            UnlockAchievement(GameConstants.AchievementId.FREQUENT_FLYER);
        }
    }

    public void AddDamageDealt(float damage)
    {
        m_damageDealt += damage;
        SteamUserStats.SetStat("damage_dealt", m_damageDealt);
        bool bSuccess = SteamUserStats.StoreStats();

        if (bSuccess && m_damageDealt >= 100000)
        {
            // Achievement: Frequent Fragger
            UnlockAchievement(GameConstants.AchievementId.FREQUENT_FRAGGER);
        }
    }

    public void AddHpRecovered(float health)
    {
        m_hpRecovered += health;
        SteamUserStats.SetStat("hp_recovered", m_hpRecovered);
        bool bSuccess = SteamUserStats.StoreStats();

        if (bSuccess && m_hpRecovered >= 50000)
        {
            // Achievement: Frequent Flee-er
            UnlockAchievement(GameConstants.AchievementId.FREQUENT_FLEEER);
        }
    }

    public void AddWeaponsUsed(int weaponsUsed)
    {
        m_weaponsUsed += weaponsUsed;
        SteamUserStats.SetStat("weapons_used", m_weaponsUsed);
        bool bSuccess = SteamUserStats.StoreStats();

        if (bSuccess && m_weaponsUsed >= 10)
        {
            // Achievement: Well Rounded
            UnlockAchievement(GameConstants.AchievementId.WELL_ROUNDED);
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: We have stats data from Steam. It is authoritative, so update
    //			our data with those results now.
    //-----------------------------------------------------------------------------
    private void OnUserStatsReceived(UserStatsReceived_t pCallback)
    {
        if (!SteamManager.Initialized)
            return;

        // we may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("Received stats and achievements from Steam\n");

                m_bStatsValid = true;

                // load achievements
                foreach (Achievement ach in m_Achievements)
                {
                    bool ret = SteamUserStats.GetAchievement(ach.m_eAchievementID.ToString(), out ach.m_bAchieved);
                    if (ret)
                    {
                        ach.m_strName = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "name");
                        ach.m_strDescription = SteamUserStats.GetAchievementDisplayAttribute(ach.m_eAchievementID.ToString(), "desc");
                    }
                    else
                    {
                        Debug.LogWarning("SteamUserStats.GetAchievement failed for Achievement " + ach.m_eAchievementID + "\nIs it registered in the Steam Partner site?");
                    }
                }

                // load stats
                SteamUserStats.GetStat("damage_dealt", out m_damageDealt);
                SteamUserStats.GetStat("hp_recovered", out m_hpRecovered);
                SteamUserStats.GetStat("boost_used", out m_boostUsed);
                SteamUserStats.GetStat("weapons_used", out m_weaponsUsed);
            }
            else
            {
                Debug.Log("RequestStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Our stats data was stored!
    //-----------------------------------------------------------------------------
    private void OnUserStatsStored(UserStatsStored_t pCallback)
    {
        // we may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (EResult.k_EResultOK == pCallback.m_eResult)
            {
                Debug.Log("StoreStats - success");
            }
            else if (EResult.k_EResultInvalidParam == pCallback.m_eResult)
            {
                // One or more stats we set broke a constraint. They've been reverted,
                // and we should re-iterate the values now to keep in sync.
                Debug.Log("StoreStats - some failed to validate");
                // Fake up a callback here so that we re-load the values.
                UserStatsReceived_t callback = new UserStatsReceived_t();
                callback.m_eResult = EResult.k_EResultOK;
                callback.m_nGameID = (ulong)m_GameID;
                OnUserStatsReceived(callback);
            }
            else
            {
                Debug.Log("StoreStats - failed, " + pCallback.m_eResult);
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: An achievement was stored
    //-----------------------------------------------------------------------------
    private void OnAchievementStored(UserAchievementStored_t pCallback)
    {
        // We may get callbacks for other games' stats arriving, ignore them
        if ((ulong)m_GameID == pCallback.m_nGameID)
        {
            if (0 == pCallback.m_nMaxProgress)
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' unlocked!");
            }
            else
            {
                Debug.Log("Achievement '" + pCallback.m_rgchAchievementName + "' progress callback, (" + pCallback.m_nCurProgress + "," + pCallback.m_nMaxProgress + ")");
            }
        }
    }

    //-----------------------------------------------------------------------------
    // Purpose: Display the user's stats and achievements
    //-----------------------------------------------------------------------------
    //public void Render()
    //{
    //    if (!SteamManager.Initialized)
    //    {
    //        GUILayout.Label("Steamworks not Initialized");
    //        return;
    //    }

    //    GUILayout.Label("damage_dealt: " + m_damageDealt);
    //    GUILayout.Label("hp_recovered: " + m_hpRecovered);
    //    GUILayout.Label("boost_used: " + m_boostUsed);
    //    GUILayout.Label("weapons_used: " + m_weaponsUsed);

    //    GUILayout.BeginArea(new Rect(Screen.width - 300, 0, 300, 800));
    //    foreach (Achievement ach in m_Achievements)
    //    {
    //        GUILayout.Label(ach.m_eAchievementID.ToString());
    //        GUILayout.Label(ach.m_strName + " - " + ach.m_strDescription);
    //        GUILayout.Label("Achieved: " + ach.m_bAchieved);
    //        GUILayout.Space(20);
    //    }

    //    // FOR TESTING PURPOSES ONLY!
    //    if (GUILayout.Button("RESET STATS AND ACHIEVEMENTS"))
    //    {
    //        SteamUserStats.ResetAllStats(true);
    //        SteamUserStats.RequestCurrentStats();
    //    }
    //    GUILayout.EndArea();
    //}

    public void ResetStatsAndAchievements()
    {
        SteamUserStats.ResetAllStats(true);
        SteamUserStats.RequestCurrentStats();
    }
}

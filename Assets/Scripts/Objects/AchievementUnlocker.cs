using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementUnlocker : IEntity
{
    public GameConstants.AchievementId achievementId;

    public void OnTriggerEnter(Collider other)
    {
        if (!active)
        {
            return;
        }

        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            UnlockAchievement();
            // Destroy(this.gameObject); // Should I destroy it?
        }
    }

    public void UnlockAchievement()
    {
        FindObjectOfType<GameSettingsManager>().steam.UnlockAchievement(achievementId);
    }
}

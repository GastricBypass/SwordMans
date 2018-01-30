using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineLobbyManager : IMainMenuManager
{
    public Text firstStage;
    public bool versus = true;

    public override void Start()
    {
        base.Start();

        gsm.SpawnMenuPlayers();
    }

    public override void BackToMainMenu()
    {
        gsm.SwitchToLocalPlay();
    }

    public void Play()
    {
        if (versus)
        {
            gsm.stages = gsm.data.versusStages;
        }
        else
        {
            gsm.stages = gsm.data.coopStages;
        }

        gsm.LoadStage(firstStage.text);
        Time.timeScale = 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    public List<AudioClip> songs;

    private GameSettingsManager gsm;
    private AudioSource audioSource;

    private Dictionary<string, List<AudioClip>> songsByStage;

    // Use this for initialization
    void Start ()
    {
        if (FindObjectsOfType<MusicManager>().Length > 1)
        {
            // Not the best, but I need to make sure there are never multiple instances of the music manager. Even for a breif instance as is the case when you return to the main menu. 
            // This might be fixed now with the new way I handle deleting the extra gsm's. Needs testing though.
            Destroy(this.gameObject);
            return;
        }
        gsm = this.GetComponent<GameSettingsManager>();

        if (songsByStage == null)
        {
            songsByStage = new Dictionary<string, List<AudioClip>>();
            SetStagePlaylists(FindObjectsOfType<StageMusic>());
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        SetSong("Main Menu");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Play()
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }

    public void SetSong(string stage, bool fadeIn = false)
    {
        // set default to be main menu music
        songsByStage.TryGetValue("Main Menu", out songs);

        List<AudioClip> newSongs;
        songsByStage.TryGetValue(stage, out newSongs);
        if (newSongs != null)
        {
            songs = newSongs;
        }

        audioSource.clip = songs[(int)Random.Range(0, songs.Count)];
        audioSource.Play();
        if (fadeIn)
        {
            FadeIn(3);
        }
        else
        {
            audioSource.volume = gsm.settings.musicVolume;
        }
    }

    private void SetStagePlaylists(StageMusic[] stageSongs)
    {
        for (int i = 0; i < stageSongs.Length; i++)
        {
            songsByStage.Add(stageSongs[i].stage, stageSongs[i].songs);
        }
    }

    public void Mute(bool muted)
    {
        if (audioSource != null)
        {
            audioSource.mute = muted;
        }
    }

    public void SetVolume(float volume)
    {
        if (audioSource != null)
        {
            audioSource.volume = volume;
        }
    }

    public void FadeIn(float time)
    {
        StartCoroutine(Fade(true, time));
    }

    public void FadeOut(float time)
    {
        StartCoroutine(Fade(false, time));
    }

    private IEnumerator Fade(bool fadeIn, float time, int precision = 10)
    {
        System.DateTime startTime = System.DateTime.Now;

        float dV = gsm.settings.musicVolume / precision; 

        if (fadeIn)
        {
            audioSource.volume = 0;
            for (int i = 0; i < 10 && audioSource.volume <= gsm.settings.musicVolume; i++)
            {
                audioSource.volume += dV;
                yield return new WaitForSeconds(time / precision);
            }
        }
        else
        {
            for (int i = 0; i < 10 && audioSource.volume >= 0; i++)
            {
                audioSource.volume -= dV;
                yield return new WaitForSeconds(time / precision);
            }
        }
    }
}

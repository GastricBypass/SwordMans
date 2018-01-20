using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrap : ITrap
{
    public List<Transform> fireSpawners;
    public ParticleSystem firePrefab;

    private List<ParticleSystem> fires = new List<ParticleSystem>();

    public override void Activate()
    {
        base.Activate();

        foreach (Transform spawner in fireSpawners)
        {
            ParticleSystem fire = Instantiate<ParticleSystem>(firePrefab, spawner);
            fire.Stop();
            ParticleSystem.MainModule main = fire.main;
            main.duration = readyTime - main.startLifetime.constantMax;

            AudioSource audio = fire.GetComponent<AudioSource>();
            if(audio != null)
            {
                audio.volume = audio.volume / fireSpawners.Count;
                StartCoroutine(waitToStopSound(main.duration, audio));
            }

            fire.Play();
            fires.Add(fire);
        }
    }

    public override void Ready()
    {
        base.Ready();

        foreach (ParticleSystem fire in fires)
        {
            if (fire != null)
            {
                Destroy(fire.gameObject);
            }
        }
    }

    private IEnumerator waitToStopSound(float seconds, AudioSource audio)
    {
        yield return new WaitForSeconds(seconds);
        audio.mute = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEmitter : MonoBehaviour {

    public List<AudioClip> possibleClips;

    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
        audioSource.playOnAwake = false;

        GameSettingsManager gsm = FindObjectOfType<GameSettingsManager>();

        audioSource.volume = gsm == null ? 1f : gsm.settings.effectsVolume;
    }

    private void OnTriggerEnter(Collider other)
    {
        PlaySound();
    }

    private void OnCollisionEnter(Collision collision)
    {
        PlaySound();
    }

    public void PlaySound()
    {
        int index = Random.Range(0, possibleClips.Count);

        audioSource.clip = possibleClips[index];
        audioSource.Play();
    }
}


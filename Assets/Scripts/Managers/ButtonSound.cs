using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSound : MonoBehaviour {

    public Button button;
    public AudioClip clickSound;
    public AudioClip selectSound;

    private AudioSource audioSource;

	// Use this for initialization
	public void Start ()
    {
		if (button == null)
        {
            button = this.GetComponent<Button>();
        }

        audioSource = this.gameObject.AddComponent<AudioSource>();
        audioSource.clip = clickSound;
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        button.onClick.AddListener(PlayClickSound);
        //button.onSelect.AddListener(PlaySelectSound);
    }

    public void PlaySelectSound()
    {
        // The problem with this is a lot of my buttons disappear after they are pressed, thus making them not make the sound.
        audioSource.clip = selectSound;
        audioSource.Play();
    }

    public void PlayClickSound()
    {
        //audioSource.clip = clickSound;
        audioSource.PlayOneShot(clickSound);
    }
}

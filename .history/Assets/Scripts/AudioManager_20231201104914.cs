using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public GameObject bgMusic;
    private AudioSource audioSource;
    public AudioMixerGroup normalMixer;
    public AudioMixerGroup buttonMixer;
    public AudioClip makeMove;
    public AudioClip reverse;
    public AudioClip button;
    public AudioClip win;

    public Button muteSign;
    public Sprite mute;
    public Sprite unmute;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void MakeMove()
    {
        audioSource.outputAudioMixerGroup = normalMixer;
        audioSource.clip = makeMove;
        audioSource.Play();
    }

    public void Reverse()
    {
        audioSource.outputAudioMixerGroup = normalMixer;
        audioSource.clip = reverse;
        audioSource.Play();
    }

    public void Button()
    {
        audioSource.outputAudioMixerGroup = buttonMixer;
        audioSource.clip = button;
        audioSource.Play();
    }

    public void Win()
    {
        audioSource.outputAudioMixerGroup = normalMixer;
        audioSource.clip = win;
        audioSource.Play();
    }

    public void Mute()
    {
        AudioSource audio = bgMusic.GetComponent<AudioSource>();
        if (audio.volume == 0)
        {
            audio.volume = 0.349f;
            muteSign.GetComponent<UnityEngine.UI.Image>().sprite = unmute;
        }
        else {
            audio.volume = 0;
            muteSign.GetComponent<UnityEngine.UI.Image>().sprite = mute;
        }
    }
}

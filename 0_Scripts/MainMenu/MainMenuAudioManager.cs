using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AudioEnum
{
    ButtonClick, MatchingEff, Countdown, Duck, Robot, Radar, RaderDiscovery, Shoot , Copy,Recovery, Temptation,
    Car, Chicken, Firework, Train, beShot, tang, vs_eff, timelimit
}
public class MainMenuAudioManager : MonoBehaviour {
    public static MainMenuAudioManager instance;
    public AudioSource audio;
    public AudioClip[] _audio;
    public Slider SFXSlider;

    private void Start()
    {
        if (instance == null)
            instance = this;
        audio = GetComponent<AudioSource>();
    }

    public void Play(AudioEnum _enum)
    {
        audio.PlayOneShot(_audio[(int)_enum]);
    }

    public void SFXVolume()
    {
        audio.volume = SFXSlider.value;
    }
}
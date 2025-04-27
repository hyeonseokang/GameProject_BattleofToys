using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class BGMng : MonoBehaviour
{
    public static BGMng instance;
    public AudioSource bgmAudio;
    public AudioClip[] bgmclip;
    public Slider bgmSlider;

    void Start()
    {
        if (instance == null)
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.SetInt("ROUND", 1);
            PlayerPrefs.SetInt("ROUNDTYPE1", 0);
            PlayerPrefs.SetInt("ROUNDTYPE2", 0);
            PlayerPrefs.SetInt("ROUNDTYPE3", 0);
            PlayerPrefs.SetInt("WIN", 0);
            PlayerPrefs.SetInt("LOSE", 0);
            PlayerPrefs.SetInt("EXP", 50);
            PlayerPrefs.SetInt("Possession", 0);
            PlayerPrefs.SetInt("Kill", 0);
            PlayerPrefs.SetFloat("AbilityAverage", 0f);
            PlayerPrefs.SetFloat("TimeAverag", 0f);
            PlayerPrefs.SetInt("Time", 0);
            PlayerPrefs.SetInt("TimeNum", 0);
            PlayerPrefs.SetInt("Ability", 0);
            PlayerPrefs.SetInt("AbilityNum", 0);

            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        bgmAudio = GetComponent<AudioSource>();
        bgmAudio.PlayOneShot(bgmclip[0]);
    }

    public void BGMVolume()
    {
        bgmAudio.volume = bgmSlider.value;
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void BtnClickSound()
    {
        MainMenuAudioManager.instance.Play(AudioEnum.ButtonClick);
    }

    public void MatchingSound()
    {
        MainMenuAudioManager.instance.Play(AudioEnum.MatchingEff);
    }
}

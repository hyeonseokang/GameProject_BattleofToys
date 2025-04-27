using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ResultSound : MonoBehaviour {

    public AudioEnum abcd;


    public void EventSound()
    {
        MainMenuAudioManager.instance.Play(abcd);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TitleScript : MonoBehaviour {
    public float AniTime;
    public Image TouchToScreen;

    private float Start = 1f;
    private float end = 0f;
    private float time = 0;

    int FadeMode = 1;//1 는 FadeIn -1은 FadeOut
    private void Update()
    {
        FadeInOut();
        if(Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
            MainMenuAudioManager.instance.Play(AudioEnum.ButtonClick);
            Debug.Log(AudioEnum.ButtonClick);
        }
    }

    void FadeInOut()
    {
        time += FadeMode * (Time.deltaTime / AniTime);
        Color color = TouchToScreen.color;
        color.a = Mathf.Lerp(Start, end, time);
        TouchToScreen.color = color;
        if (TouchToScreen.color.a <= 0 || TouchToScreen.color.a >= 1)
        {
            FadeMode *= -1;
        }
    }

}

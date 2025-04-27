using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Logo : MonoBehaviour {
    public Text txt;
    public GameObject GameLogo;
    public Image TeamLogoImg;
    public Image GameLogoImg;
    public Image GameLogoImg2;
    private float TeamLogoColor = 1;
    private float GameLogoColor = 0;
    bool fadestate = true;
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        Fade();
        LogoFade();
    }

    private void Fade()
    {

        if (fadestate)
        {
            Color goalcolor = txt.color;
            goalcolor.a = 0;
            txt.color = Color.Lerp(txt.color, goalcolor, Time.deltaTime * 0.9f);

            if (txt.color.a < 0.07)
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
            }

            if (txt.color == goalcolor)
            {
                fadestate = false;
            }
        }

        if (!fadestate)
        {
            Color goalcolor = txt.color;
            goalcolor.a = 1;
            txt.color = Color.Lerp(txt.color, goalcolor, Time.deltaTime * 0.7f);

            if (txt.color.a > 0.95)
            {
                txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
            }

            if (txt.color == goalcolor)
            {
                fadestate = true;
            }
        }
    }

    void LogoFade()
    {
        if (TeamLogoColor >= 0)
        {
            TeamLogoColor -= Time.deltaTime * 0.6f;
            TeamLogoImg.color = new Color(TeamLogoImg.color.r, TeamLogoImg.color.g, TeamLogoImg.color.b, TeamLogoColor);
        }

        if (GameLogoColor <= 1 && TeamLogoColor <= 0.05)
        {
            TeamLogoImg.enabled = false;
            GameLogo.SetActive(true);
            GameLogoColor += Time.deltaTime * 0.6f;
            GameLogoImg.color = new Color(GameLogoImg.color.r, GameLogoImg.color.g, GameLogoImg.color.b, GameLogoColor);
            GameLogoImg2.color = new Color(GameLogoImg.color.r, GameLogoImg.color.g, GameLogoImg.color.b, GameLogoColor);
        }
    }
    public Image Screen;
    IEnumerator MoveSoundOnOff()
    {
        float Timef = 0f;
        Screen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), Timef);

        while (Screen.color.a < 1f)
        {
            Timef += Time.deltaTime / 1.0f;
            Screen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), Timef);
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
    }
    public void NextScene()
    {
        StartCoroutine("MoveSoundOnOff");
    }
}

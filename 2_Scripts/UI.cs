using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {
    public Sprite[] ShopSprite;
    public Image ShopImage;
    public Image PersonalImg;
    public GameObject CreditImg;
    public GameObject CreditTxt;
    public GameObject inform;
    public GameObject img;
    public GameObject vsparticle;
    public Text Havenumtxt;
    public Text Equiptxt;
    public Text Moneytxt;
    public Text Scoretxt;
    public Text Player1Scoretxt;
    public Text player2Scoretxt;
    public Text TimerTxt;
    public Image[] BGMImg;
    public Image[] SFXImg;
    public Image VsMatchImg;
    public Image VsMatchImg1;
    public Image inforImg;
    public Sprite[] inforimg;
    public Image Playerimg;
    public Image Playerimg2;
    public Image loadingImg;
    public Text PossText;
    public Text KillText;
    public Text AbilityAverageText;
    public Text TimeAverageText;
    //public Image 
    private bool CreditState;
    //public bool MatchingState = true;
    private float timeCount;
    private float timeCount1;
    private float timeCount2;
    private int i = 0;
    private int a = 0;
    private int b = 0;
    private int j = 0;
    private int c = 0;
    private int temp = 0;
    private int itemNum = 0;
    private Vector3 targetpos;
    private Vector3 tempPos;
    public Vector3 tempPos1;
    private Vector3 ImgTempPos;
    private Vector3 ImgTempPos1;
    public Image Screen;
    IEnumerator MoveSoundOnOff()
    {
        float Timef = 1f;
        Screen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0,0,0,1), Timef);
        while (Screen.color.a > 0.0f)
        {
            Timef -= Time.deltaTime / 1.0f;
            Screen.color = Color.Lerp(new Color(0, 0, 0, 0), new Color(0, 0, 0, 1), Timef);
            yield return null;
        }
        Destroy(Screen);
    }
    // Use this for initialization
    void Start () {
        //for(int i = 0; i < 11; i++)
        //{
        //    BGMImg[i].enabled = false;
        //}

        StartCoroutine(MoveSoundOnOff());

        a = 11;
        for(int i = 0; i < 11; i++)
        {
            SFXImg[i].enabled = false;
        }
        MainMenumanager.Instance.MatchingState = false;
        MainMenumanager.Instance.loadingstate = false;
        VsMatchImg.enabled = false;
        VsMatchImg1.enabled = false;

        stats();
        EXP();
    }
	
	// Update is called once per frame
	void Update () {
      
        Credit();
        matching();
        Loading();
        timer();
    }

    void Credit()
    {
        Color targetcolor;
        targetcolor = PersonalImg.color;
        targetcolor.a = 1;

        targetpos = CreditTxt.transform.position;
        targetpos.y = 219;
        if (CreditState)
        {
            CreditTxt.transform.position = Vector3.MoveTowards(CreditTxt.transform.position, targetpos, 50 * Time.deltaTime);
        }
          
        if(CreditTxt.transform.position == targetpos)
        {
            CreditState = false;
            PersonalImg.color = Color.Lerp(PersonalImg.color, targetcolor, Time.deltaTime * 0.5f);
        }
    }

    void Loading()
    {
        if (MainMenumanager.Instance.loadingstate)
        {
            if (loadingImg.fillAmount != 1)
                loadingImg.fillAmount += Time.deltaTime;
            else
                loadingImg.fillAmount = 0;
        }
    }

    void timer()
    {
        if (MainMenumanager.Instance.loadingstate)
        {
            timeCount += Time.deltaTime;
            TimerTxt.text = "0" + timeCount2 + " : " + timeCount1 + string.Format("{0:f0}", timeCount);

            if (timeCount >= 9.25f)
            {
                timeCount = 0;
                timeCount1++;
            }

            if (timeCount1 == 6)
            {
                timeCount = 0;
                timeCount1 = 0;
                timeCount2++;
            }
        }
    }
    private void ParticleMatch()
    {
        vsparticle.SetActive(true);
        MainMenuAudioManager.instance.Play(AudioEnum.vs_eff);
    }
    bool match = true;
    private void matching()
    {
        if(VsMatchImg.transform.position.x == VsMatchImg1.transform.position.x && match)
        {
            match = false;
            Invoke("ParticleMatch", 0.5f);
        }

        if (Playerimg.transform.position == ImgTempPos)
        {
            VsMatchImg.enabled = true;
            VsMatchImg1.enabled = true;

            tempPos = VsMatchImg.transform.position;
            tempPos.x = 0.2f;
            VsMatchImg.transform.position = Vector3.MoveTowards(VsMatchImg.transform.position, tempPos, 60 * Time.deltaTime);

            tempPos1 = VsMatchImg1.transform.position;
            tempPos1.x = 0.2f;
            VsMatchImg1.transform.position = Vector3.MoveTowards(VsMatchImg1.transform.position, tempPos1, 60 * Time.deltaTime);
        }

        if (Playerimg2.transform.position == ImgTempPos1)
        {
            
            ImgTempPos = Playerimg.transform.position;
            ImgTempPos.x = -30.5f;
            Playerimg.transform.position = Vector3.MoveTowards(Playerimg.transform.position, ImgTempPos, 150 * Time.deltaTime);
        }

        if (MainMenumanager.Instance.MatchingState)
        {
            ImgTempPos1 = Playerimg2.transform.position;
            ImgTempPos1.x = 40.5f;
            Playerimg2.transform.position = Vector3.MoveTowards(Playerimg2.transform.position, ImgTempPos1, 150 * Time.deltaTime);
        }
    }
    public Text LevelNameText;
    public Text WinLoseText;

    public Text[] PlayerNameText;
    public Text[] PlayerWinloseText;

    public Image[] Gauege;
    void EXP()
    {
        if (PlayerPrefs.GetInt("EXP") == 50)
        {
            inforImg.sprite = inforimg[0];
            LevelNameText.text = "LV.1 June Yeop";
            WinLoseText.text = "0 WIN / 0 LOSE / 0%";

            Gauege[1].gameObject.SetActive(false);
            Gauege[0].gameObject.SetActive(false);

            PlayerNameText[0].text = "LV.1 June Yeop";
            PlayerNameText[1].text = "LV.1 Hyeon Seo";
            PlayerWinloseText[0].text = "0 WIN / 0 LOSE / 0 %";
            PlayerWinloseText[1].text = "0 WIN / 0 LOSE / 0 %";

        }

        if(PlayerPrefs.GetInt("EXP") == 100)
        {
            inforImg.sprite = inforimg[1];
            LevelNameText.text = "LV.2 June Yeop";
            WinLoseText.text = "1 WIN / 0 LOSE / 100%";

            Gauege[0].gameObject.SetActive(false);
            Gauege[1].rectTransform.sizeDelta = new Vector2(110.1f, 22);

            PlayerNameText[0].text = "LV.2 June Yeop";
            PlayerNameText[1].text = "LV.2 Hyeon Seo";
            PlayerWinloseText[0].text = "1 WIN / 0 LOSE / 100 %";
            PlayerWinloseText[1].text = "0 WIN / 1 LOSE / 0 %";
            

            AbilityAverageText.text = "2.0";
            TimeAverageText.text = "1.2";
        }
    }

    void stats()
    {
        PossText.text = PlayerPrefs.GetInt("Possession").ToString("N1");
        KillText.text = PlayerPrefs.GetInt("Kill").ToString("N1");
        AbilityAverageText.text = PlayerPrefs.GetFloat(" AbilityAverage").ToString("N1");
        TimeAverageText.text = PlayerPrefs.GetFloat("TimeAverage").ToString("N1");
    }

    void infor()
    {
        Scoretxt.text = MainMenumanager.Instance.WinScore.ToString() + "승 " + MainMenumanager.Instance.LoseScore.ToString() + "패 " + MainMenumanager.Instance.WinningRate.ToString() + "%";
        Player1Scoretxt.text = MainMenumanager.Instance.WinScore.ToString() + "승 " + MainMenumanager.Instance.LoseScore.ToString() + "패 " + MainMenumanager.Instance.WinningRate.ToString() + "%";
        player2Scoretxt.text = MainMenumanager.Instance.LoseScore.ToString() + "승 " + MainMenumanager.Instance.WinScore.ToString() + "패 " + MainMenumanager.Instance.WinningRate2.ToString() + "%";
    }

    public void BGMleftClick()
    {
        if (a >= 1)
        {
            BGMng.instance.bgmAudio.volume = (a * 0.1f) - 0.1f;
            a--;
            BGMImg[a + 1].enabled = false;
        }
    }

    public void BGMrightClick()
    {
        if (a <= 10)
        {
            BGMng.instance.bgmAudio.volume = a * 0.1f;
            a++;
            BGMImg[a - 1].enabled = true;
        }
    }

    public void SFXleftClick()
    {
        if (b >= 1)
        {
            MainMenuAudioManager.instance.audio.volume = (b * 0.1f) - 0.1f;
            b--;
            SFXImg[b + 1].enabled = false;
        }
    }

    public void SFXrightClick()
    {
        if (b <= 10)
        {
            MainMenuAudioManager.instance.audio.volume = b * 0.1f;
            b++;
            SFXImg[b - 1].enabled = true;
        }

        if(SFXImg[11].enabled == true)
        {
            MainMenuAudioManager.instance.audio.volume = 1;
        }
        Debug.Log(b);

    }

    public void BuyClick()
    {
        if (MainMenumanager.Instance.Money >= MainMenumanager.Instance.Weapon[i])
        {
            MainMenumanager.Instance.Money = MainMenumanager.Instance.Money - MainMenumanager.Instance.Weapon[i];
            MainMenumanager.Instance.Havenum[i]++;
        }
    }

    public void EquipClick()
    {

        if (!MainMenumanager.Instance.Equip[i])
        {
            MainMenumanager.Instance.Equip[i] = true;

        }

        else if (MainMenumanager.Instance.Equip[i])
        {
            MainMenumanager.Instance.Equip[i] = false;
        }
    }

    public void myinfor()
    {
        inform.SetActive(true);
    }

    public void exitbtnClick()
    {
        inform.SetActive(false);
    }

    public void CreditBtn()
    {
        CreditImg.SetActive(true);
        CreditState = true;
        BGMng.instance.bgmAudio.Stop();
        BGMng.instance.bgmAudio.PlayOneShot(BGMng.instance.bgmclip[5]);
    }
}

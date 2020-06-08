using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class KHS_GameEnd : MonoBehaviour
{
    public GameObject GameEndObject;
    public Text SetScoreText;
    public Image[] RoundImage;
    bool Roundbool = false;

    public GameObject RoundEndl;
    public GameObject win;  //here
    public GameObject lose; //here

    public Image loseimg;
    public Sprite losesprite;
    public Sprite losesprite2;

    public Image WinnerTexture;
    
    public GameObject round3win;

    int WinScore = 0;
    int LoseScore = 0;

    public Image YouWinImage;
    public Text YouWinText;

    public Image Image1p;
    public Image Image2p;


    public Image WinImage;
    public Image LoseImage;

    private void Start()
    {

    }
    [PunRPC]
    public void InGameGo()
    {
        Debug.Log("인게임 고고씽 고고씽");
        PhotonNetwork.LoadLevel("InGame");
    }
    public void MainMenuGo()
    {
        if (Roundbool)
        {
            PlayerPrefs.SetInt("EXP", 100);
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("MainMenu");
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<PhotonView>().RPC("InGameGo", PhotonTargets.All);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Roundbool)
            {
                BGMng.instance.bgmAudio.Stop();
                BGMng.instance.bgmAudio.PlayOneShot(BGMng.instance.bgmclip[0]);
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LoadLevel("MainMenu");
            }
        }
        //Debug.Log(WinImage.transform.position);
        //Debug.Log(LoseImage.transform.position);
    }
    [PunRPC]
    void GameEndCheck2(PhotonPlayer _player)
    {
        if (_player != PhotonNetwork.player)
        {
            Win();
        }
        else
        {
            Lose();
        }
        RoundRefresh();
    }
    [PunRPC]
    void GameEndCheck(PhotonPlayer _player)
    {
        if (_player == PhotonNetwork.player)
        {
            Win();
        }
        else
        {
            Lose();
        }
        RoundRefresh();
    }
    void RoundRefresh()
    {
        if (PlayerPrefs.GetInt("ROUND") == 3)
        {
            PlayerPrefs.SetInt("EXP", 100);
            PlayerPrefs.SetFloat("AbilityAverage",
                ((float)PlayerPrefs.GetInt("Ability") / (float)PlayerPrefs.GetInt("AbilityNum")));
            PlayerPrefs.SetFloat("TimeAverage",
             ((float)PlayerPrefs.GetInt("Time") / (float)PlayerPrefs.GetInt("TimeNum")));
            StartCoroutine("FinalGameEnd2");
        }
        else
        {
            Debug.Log("아따 시방 2222 그머시여 3라운드가 아니여");
            PlayerPrefs.SetInt("ROUND", (PlayerPrefs.GetInt("ROUND") + 1));
            Invoke("RoundEndGo", 3f);
        }
    }
    void RoundEndGo()
    {
        if (PhotonNetwork.isMasterClient)
        {
            Debug.Log("asdf");
            GetComponent<PhotonView>().RPC("InGameGo", PhotonTargets.All);
        }
    }
    void Win()
    {
        PlayerPrefs.SetInt("WIN", PlayerPrefs.GetInt("WIN") + 1);
        PlayerPrefs.SetInt("ROUNDTYPE" + (PlayerPrefs.GetInt("ROUND")), 1);
        RoundEndl.SetActive(true);
        win.SetActive(true);
        BGMng.instance.bgmAudio.Stop();
        BGMng.instance.bgmAudio.PlayOneShot(BGMng.instance.bgmclip[2]);
    }
    void Lose()
    {
        PlayerPrefs.SetInt("LOSE", PlayerPrefs.GetInt("LOSE") + 1);
        RoundEndl.SetActive(true);
        lose.SetActive(true);  //here
        StartCoroutine("RoundLose");
        BGMng.instance.bgmAudio.Stop();
        BGMng.instance.bgmAudio.PlayOneShot(BGMng.instance.bgmclip[3]);
    }

    IEnumerator RoundLose()
    {
            loseimg.sprite = losesprite;
            MainMenuAudioManager.instance.Play(AudioEnum.tang);
            yield return new WaitForSeconds(1f);
            loseimg.sprite = losesprite2;
            MainMenuAudioManager.instance.Play(AudioEnum.tang);
    }


    public Animator LineImage;
    IEnumerator FinalGameEnd2()
    {
        if (PlayerPrefs.GetInt("WIN") > PlayerPrefs.GetInt("LOSE"))
        {
            MainMenuAudioManager.instance.Play(AudioEnum.Firework);
            round3win.SetActive(true);

            yield return new WaitForSeconds(5f);


            Vector3 Temp = WinImage.rectTransform.position;
            WinImage.rectTransform.position = LoseImage.rectTransform.position;
            LoseImage.rectTransform.position = Temp;

            //WinImage.rectTransform.position =
            //new Vector3(WinImage.rectTransform.position.x-550, WinImage.rectTransform.position.y,
            //WinImage.rectTransform.position.z);
            //LoseImage.rectTransform.position =
            //    new Vector3(LoseImage.rectTransform.position.x+550, LoseImage.rectTransform.position.y,
            //    LoseImage.rectTransform.position.z);
            GameEndObject.SetActive(true);
            WinImage.gameObject.SetActive(true);
            LoseImage.gameObject.SetActive(true);
            RoundEndl.SetActive(false);
        }
        else
        {
            RoundEndl.SetActive(true);
            StartCoroutine("RoundLose");

            yield return new WaitForSeconds(5f);

            //WinImage.rectTransform.position =
            // new Vector3(WinImage.rectTransform.position.x+550, WinImage.rectTransform.position.y,
            // WinImage.rectTransform.position.z);
            //LoseImage.rectTransform.position =
            //    new Vector3(LoseImage.rectTransform.position.x-550, LoseImage.rectTransform.position.y,
            //    LoseImage.rectTransform.position.z);


            WinImage.gameObject.SetActive(true);
            LoseImage.gameObject.SetActive(true);
            RoundEndl.SetActive(false);
        }
        yield return new WaitForSeconds(1.2f); 
        round3win.SetActive(false);
        Image1p.sprite = KHS_ObjectManager.instance.
            GameEndStateBar[KHS_ObjectManager.instance.Player[0].
            GetComponent<KHS_StateScript>().StateNumber];
        Image2p.sprite = KHS_ObjectManager.instance.
          GameEndStateBar[KHS_ObjectManager.instance.Player[1].
          GetComponent<KHS_StateScript>().StateNumber];

        GameEndObject.SetActive(true);
        Roundbool = true;
        LineImage.SetBool("State", true);
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1.0f);
            Vector3 RoundImagePos = RoundImage[i].rectTransform.position;
            if (PlayerPrefs.GetInt("ROUNDTYPE" + (i + 1)) == 1)
            {
                Smile[i].enabled = true;
                //RoundImage[i].rectTransform.position = new Vector3(RoundImagePos.x - 180,
                //    RoundImagePos.y,
                //    RoundImagePos.z);
                WinScore += 1;

            }
            else
            {
                RoundImage[i].enabled = true;
                //RoundImage[i].rectTransform.position = new Vector3(RoundImagePos.x + 180,
                //  RoundImagePos.y,
                //  RoundImagePos.z);
                LoseScore += 1;
            }
            //RoundImage[i].enabled = true;
            SetScoreText.text = WinScore + " : " + LoseScore;

        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public Image[] Smile;
}
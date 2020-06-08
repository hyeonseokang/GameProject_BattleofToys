using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class NetwrokMatchMaker : Photon.MonoBehaviour {
    public static readonly string InGameScene = "InGame";
    public static readonly string MainMenuScene = "MainMenu";

    Coroutine _coroutine;

    public string Version;
    public Text NetworkState;
    public Text Loadingtxt;
    public GameObject MatchObject;
    public GameObject GameStartObject;
    private float currentAmount=0.0f;
    private float DirectionAmount = 1.0f;
    public Image Loadingbar;
	void Start () {
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.ConnectUsingSettings("0.1");
        PhotonNetwork.logLevel = PhotonLogLevel.Full;
    }
    private void Update()
    {
        //NetwrokStateRenewal();
    }
    void NetwrokStateRenewal()
    {
        NetworkState.text=PhotonNetwork.connectionStateDetailed.ToString();
    }

    public void OnJoinedLobby()
    {
     
    }

    public void MatchStart()
    {
        MainMenuAudioManager.instance.Play(AudioEnum.MatchingEff);
        MatchObject.SetActive(true);
        _coroutine = StartCoroutine(IMatchText());
        StartCoroutine(IMatch());
        MainMenumanager.Instance.loadingstate = true;
    }
    IEnumerator IMatchText()
    {
        while(true)
        {
            //yield return null;
            //bool Checkbool = false;
            //if (currentAmount > 0 && DirectionAmount < 0)
            //    Checkbool = true;
            //if (currentAmount < 100 && DirectionAmount > 0)
            //    Checkbool = true;
            //if (Checkbool)
            //{
            //    Debug.Log("스킬 쿨타임도는중...");
            //    float Speed = 100;
            //    if (currentAmount > 10 && currentAmount < 30)
            //        Speed = 500;
            //    if (currentAmount > 50 && currentAmount < 90)
            //        Speed = 500;
            //    currentAmount += Speed * Time.deltaTime * DirectionAmount;
            //}
            //else
            //{
            //    DirectionAmount *= -1f;

            //    if (DirectionAmount > 0)
            //        Loadingbar.fillClockwise = true;
            //    else if (DirectionAmount < 0)
            //        Loadingbar.fillClockwise = false;
            //}
            //Loadingbar.GetComponent<Image>().fillAmount = currentAmount / 100;
            yield return new WaitForSeconds(0.2f);
            Loadingtxt.text = "LOADING.";
            yield return new WaitForSeconds(0.2f);
            Loadingtxt.text = "LOADING..";
            yield return new WaitForSeconds(0.2f);
            Loadingtxt.text = "LOADING...";
        }
    }
    IEnumerator IMatch()
    {
        yield return null;
        yield return new WaitForSeconds(1.0f);
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
        PhotonNetwork.CreateRoom(null);
    }

    void OnJoinedRoom()
    {
        PhotonNetwork.room.MaxPlayers = 2;
        StartCoroutine("MatchGo");
    }

    IEnumerator MatchGo()
    {
        while (true)
        {
            if (PhotonNetwork.playerList.Length == PhotonNetwork.room.MaxPlayers)
            {
                Debug.Log(PhotonNetwork.countOfPlayers);
                StopCoroutine(_coroutine);
                BGMng.instance.bgmAudio.Stop();
                GameStartObject.SetActive(true);
                MatchObject.SetActive(false);
                MainMenumanager.Instance.MatchingState = true;
                //MatchObject.GetComponentInChildren<Text>().text = "게임이 곧 시작됩니다.";
                yield return new WaitForSeconds(5.5f);
                PhotonNetwork.LoadLevel("InGame");
                break;
            }
            yield return null;
        }
    }
}

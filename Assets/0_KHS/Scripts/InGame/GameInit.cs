using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
using UnityEngine.UI;
public class GameInit : Photon.MonoBehaviour {
    public KJY_CamBase CamBase;
    public KHS_TargetObj TargetObj;
    private GameObject Player;
    public Text GameStartText;
    public GameObject GameStartObject;
    void Start () {
        PlayerPrefs.SetInt("AbilityNum", PlayerPrefs.GetInt("AbilityNum")+1);
        PlayerPrefs.SetInt("TimeNum", PlayerPrefs.GetInt("TimeNum")+1);
        PlayerCreate();
    }
    private void Update()
    {
        if(PhotonNetwork.playerList.Length!=PhotonNetwork.room.MaxPlayers)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }
    }

    void MasterClientInit()
    {
        GetComponent<PhotonView>().ObservedComponents.
            Add(gameObject.AddComponent<MasterClient>());
    }
    void PlayerRandomCreate()
    {
        int Random_ = Random.Range(1, 5);
        if(Random_ == 1)
        {
            Player = PhotonNetwork.Instantiate("Yellow", Vector3.up * 20, Quaternion.identity, 0);
            KHS_ObjectManager.instance.StateBarFaceImage.sprite =
                KHS_ObjectManager.instance.StateBarFaceSprite[3];
            CamBase.GetComponent<KJY_Possession>().StateBarnum_ = 4;
        }
        else if (Random_ == 2)
        {
            Player = PhotonNetwork.Instantiate("Red", Vector3.up * 20, Quaternion.identity, 0);
            KHS_ObjectManager.instance.StateBarFaceImage.sprite =
                KHS_ObjectManager.instance.StateBarFaceSprite[2];
            CamBase.GetComponent<KJY_Possession>().StateBarnum_ = 3;
        }
        else if(Random_ == 3)
        {
            Player = PhotonNetwork.Instantiate("Blue", Vector3.up * 20, Quaternion.identity, 0);
            KHS_ObjectManager.instance.StateBarFaceImage.sprite = 
                KHS_ObjectManager.instance.StateBarFaceSprite[0];
            CamBase.GetComponent<KJY_Possession>().StateBarnum_ = 1;
        }
        else if(Random_ == 4)
        {
            Player = PhotonNetwork.Instantiate("Green", Vector3.up * 20, Quaternion.identity, 0);
            KHS_ObjectManager.instance.StateBarFaceImage.sprite =
                KHS_ObjectManager.instance.StateBarFaceSprite[1];
            CamBase.GetComponent<KJY_Possession>().StateBarnum_ = 2;
        }
        MyArrow = PhotonNetwork.Instantiate("Arrow", Vector3.zero, Quaternion.identity, 0);
        CamBase.Arrow = MyArrow;
        KHS_ObjectManager.instance.StateBarFaceImage.gameObject.SetActive(true);
    }
    void PlayerCreate()
    {
        PlayerRandomCreate();
        if (PhotonNetwork.isMasterClient)
        {
            Vector3 _poistion = new Vector3(46.19327f, 20, 13.67091f);
            Player.transform.position = _poistion;
            MasterClientInit();
        }
        else
        {
            Vector3 _poistion = new Vector3(-59.6f, 20, 74f);
            Player.transform.position = _poistion;
        }
        Debug.Log("11"+Player.transform.position);
        Player.layer = 8;
        Player.tag = "Player";
        GameReadyStart();
        TargetObj.targetObj = Player;
        Debug.Log("efefef" + TargetObj.targetObj);

        CamBase.StartInit();
        CamBase.GetComponent<KJY_Possession>().enabled = true;
        CamBase.enabled = true;
        Debug.Log("22" + Player.transform.position);
        // GetComponent<PhotonView>().RPC("PlayerOtherAdd", PhotonTargets.All, PhotonNetwork.player, Player);
    }

    void GameReadyStart()
    {
        StartCoroutine("GameStart");
    }

    private void setGameStartText(string _string)
    {
        GameStartObject.GetComponentInChildren<Text>().text = _string;
    }
    public GameObject MyArrow;
    void ObjectManagerPlayerAdd()
    {
        KHS_ObjectManager.instance.Player = GameObject.FindGameObjectsWithTag("Player");
        if(Player!=KHS_ObjectManager.instance.Player[0])
        {
            GameObject _object = KHS_ObjectManager.instance.Player[0];
            KHS_ObjectManager.instance.Player[0] = KHS_ObjectManager.instance.Player[1];
            KHS_ObjectManager.instance.Player[1] = _object;
        }
        KHS_ObjectManager.instance.Player[0].GetComponentInChildren<Renderer>().material.color = Color.white;

        KHS_ObjectManager.instance.Arrow_ = GameObject.FindGameObjectsWithTag("Arrow");
        if (MyArrow != KHS_ObjectManager.instance.Arrow_[0])
        {
            GameObject _object = KHS_ObjectManager.instance.Arrow_[0];
            KHS_ObjectManager.instance.Arrow_[0] = KHS_ObjectManager.instance.Arrow_[1];
            KHS_ObjectManager.instance.Arrow_[1] = _object;
        }
        KHS_ObjectManager.instance.Arrow_[1].GetComponentInChildren<Renderer>().material.color = new Color(0.25f,0.30f,0.95f,0.854f);
        KHS_ObjectManager.instance.Arrow_[1].SetActive(false);

        KHS_ObjectManager.instance.Arrow_[1].GetComponent<KHS_ArrowText>().ArrowText.text = "2P";
        KHS_ObjectManager.instance.Player2Target = KHS_ObjectManager.instance.Player[1];
    }


    IEnumerator GameStart()
    {
        GameStartObject.SetActive(true);
        GameStartObject.GetComponent<Animator>().SetTrigger("GameStart");
        BGMng.instance.bgmAudio.Stop();
        MainMenuAudioManager.instance.Play(AudioEnum.Countdown);
        yield return new WaitForSeconds(1.0f);
        setGameStartText("2");
        MainMenuAudioManager.instance.Play(AudioEnum.Countdown);
        yield return new WaitForSeconds(1.0f);
        setGameStartText("1");
        MainMenuAudioManager.instance.Play(AudioEnum.Countdown);
        yield return new WaitForSeconds(1.0f);
        setGameStartText("GAME START!");
        if (Player.GetComponent<KJY_PlayerCtrl>() != null)
            Player.GetComponent<KJY_PlayerCtrl>().enabled = true;
        else
            Player.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = true;
        BGMng.instance.bgmAudio.PlayOneShot(BGMng.instance.bgmclip[1]);
        yield return new WaitForSeconds(0.5f);
        if (PhotonNetwork.isMasterClient)
        {
            GetComponent<MasterClient>().StartCoroutine(GetComponent<MasterClient>().TimeGo());
            Debug.Log("시간은 시작되었따 캬캬캬컄캬캬");
        }
        ObjectManagerPlayerAdd();
        GetComponent<InGameManager>().enabled = true;
        Destroy(GameStartObject);
    }
}

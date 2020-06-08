using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InGameManager : MonoBehaviour {
    bool isObjectAlpha = true;
    public GameObject f1OVERWATCH;

    public Text oneText;
    public Text twoText;
    private void Update()
    {
        float distance = Vector3.Distance(KHS_ObjectManager.instance.targetobj.targetObj.transform.position
            , KHS_ObjectManager.instance.Player[1].transform.position);
       // Debug.Log(distance);
        if (!isObjectAlpha)
        {
            if (distance < 20)
            {
                Debug.Log("헤헤헤 들어갔따");
                isObjectAlpha = true;
                GetComponent<Alpha>().StartFade(KHS_ObjectManager.instance.Player[1], 1.5f, 1.0f);
            }
        }
        if(isObjectAlpha)
        {
            if(distance>20)
            {
                Debug.Log("헤헤헤 나왔땅헤헤헤헤헤헤ㅔ");
                isObjectAlpha = false;
                Invoke("Aplphaon", 1.0f);
            }
        }
        if(Input.GetKeyDown(KeyCode.F1))
        {
            f1OVERWATCH.SetActive(true);
        }
        else if(Input.GetKeyUp(KeyCode.F1))
        {
            f1OVERWATCH.SetActive(false);
        }
    }
    void Aplphaon()
    {
        GetComponent<Alpha>().StartFade(KHS_ObjectManager.instance.Player[1], 1.5f, 0.05f);
    }
    [PunRPC]
    void TimeSet(int _Time)
    {
        KHS_ObjectManager.instance.TimeText.text = _Time.ToString() + " / 300";
        if(_Time/60==0)
        {
            PlayerPrefs.SetInt("Time", PlayerPrefs.GetInt("Time")+1);
        }
    }
    public void ABC()
    {
        MainMenuAudioManager.instance.Play(AudioEnum.timelimit);
    }
    public void ABCDEF()
    {
        BGMng.instance.bgmAudio.Stop();
        BGMng.instance.bgmAudio.PlayOneShot(BGMng.instance.bgmclip[4]);
        oneText.gameObject.SetActive(false);
        twoText.gameObject.SetActive(true);
        twoText.GetComponent<Animator>().SetBool("State", true);
    }
    [PunRPC]
    void TimeEnd()
    {
        ABC();
        Invoke("ABC", 0.3f);
        Invoke("ABCDEF", 0.6f);
        
    }
    
    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    if (stream.isWriting)
    //    {
    //        stream.SendNext(GameTime);
    //    }
    //    else
    //    {
    //        this.GameTime = (float)stream.ReceiveNext();
    //    }
    //}
}

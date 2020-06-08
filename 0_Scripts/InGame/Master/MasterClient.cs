using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterClient : MonoBehaviour {
    private PhotonView MyPhotonView;
    private int Time = 300;
    
    private void Start()
    {
        MyPhotonView = GetComponent<PhotonView>();
        GetComponent<KJY_NpcSpawn>().enabled = true;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.U))
        {
            Time = 300;
        }
        if(Input.GetKeyDown(KeyCode.J))
        {
            KHS_ObjectManager.instance.Arrow_[1].SetActive(true);
            Time = 1;
        }
        
    }
    public IEnumerator TimeGo()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            Time--;
            this.MyPhotonView.RPC("TimeSet", PhotonTargets.All, Time);
            if (Time == 0)
            {
                this.MyPhotonView.RPC("TimeEnd", PhotonTargets.All);
                break;
            }
        }
    }
}
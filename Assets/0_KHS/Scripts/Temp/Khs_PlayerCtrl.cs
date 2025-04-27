using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Khs_PlayerCtrl : MonoBehaviour {
    public void activeOnOff(bool _active)
    {
        GetComponent<PhotonView>().RPC("DisableObject", PhotonTargets.AllBuffered, _active);
    }
    [PunRPC]
    private void DisableObject(bool _active)
    {
        gameObject.SetActive(_active);
    }


  
}

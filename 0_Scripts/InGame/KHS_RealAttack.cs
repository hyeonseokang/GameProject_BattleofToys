using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_RealAttack : MonoBehaviour {
    public KJY_Possession _possesion;
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
            GetComponent<PhotonView>().RPC("ProssesionDown", PhotonTargets.All, PhotonNetwork.player);
    }
    [PunRPC]
    void ProssesionDown(PhotonPlayer _player)
    {
        Debug.Log("재형했따다다다다");
        if (PhotonNetwork.player != _player)
        {
            Debug.Log("재형당햏다");
            _possesion.PossessionGo();
        }
    }
}

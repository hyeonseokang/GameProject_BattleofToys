using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KHS_StateScript : MonoBehaviour {
    public int StateNumber;

    [PunRPC]
    void ChangeStateNumber(PhotonPlayer _player,int _state)
    {
        if(_player != PhotonNetwork.player)
        {
            KHS_ObjectManager.instance.Player[1].GetComponent<KHS_StateScript>().StateNumber = _state;
        }
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(StateNumber);
        }
        else
        {
            this.StateNumber = (int)stream.ReceiveNext();
        }
    }
}

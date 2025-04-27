using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_Layer : MonoBehaviour
{
    public int _layerint;
    private void Awake()
    {
        _layerint = gameObject.layer;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(gameObject.layer);
        }
        else
        {
            this.gameObject.layer = (int)stream.ReceiveNext();
        }
    }
    private void Update()
    {
        gameObject.layer = _layerint;
    }
    [PunRPC]
    void ChangeLayer(int _layer , PhotonPlayer _player)
    {
        if (_player != PhotonNetwork.player)
        {
            _layerint = _layer;
            if (_layer == 11)
            {
                KHS_ObjectManager.instance.Player2Target = gameObject;
            }
        }
    }

}

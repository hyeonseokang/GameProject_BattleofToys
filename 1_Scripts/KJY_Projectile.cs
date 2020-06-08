using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_Projectile : MonoBehaviour
{
    Rigidbody projectileRB;
    SphereCollider projectileColl;
    public float launchPower;

    bool collisionCheck = true;
    bool launchAble = true;
    Transform camTrans;
    Vector3 originPos;
    Vector3 originAngle;

    private void Awake()
    {
        projectileRB = GetComponent<Rigidbody>();
        projectileColl = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        camTrans = Camera.main.transform;
        originPos = transform.position;
        originAngle = transform.eulerAngles;
        gameObject.SetActive(false);
    }

    public void Launch()
    {
        if (launchAble)
        {
            MainMenuAudioManager.instance.Play(AudioEnum.Shoot);
            Debug.Log("발사");
            launchAble = false;
            gameObject.SetActive(true);
            projectileRB.useGravity = true;
            projectileRB.AddForce(camTrans.TransformDirection(Vector3.forward) * launchPower, ForceMode.Impulse);
        }
     }

    private void OnCollisionEnter(Collision collision)
    {
        Invoke("DisableObj", 2f);
        if (collision.gameObject.layer == 8)
        {
            KHS_ObjectManager.instance.GetComponent<PhotonView>().
                RPC("GameEndCheck", PhotonTargets.All, PhotonNetwork.player);
        }
        if (collision.gameObject.layer == 9)
        {
            collision.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().KillNpc();
        }
        if (collision.gameObject.layer == 11)
        {
            KHS_ObjectManager.instance.CamBase.GetComponent<PhotonView>().
                RPC("ProssesionRelease", PhotonTargets.All, PhotonNetwork.player);
            Debug.Log("빙의 해제");
        }
        //if (collisionCheck)
        //{
        //    collisionCheck = false;
            
        //}
    }

    private void DisableObj()
    {
        Debug.Log("끔");
        projectileRB.angularVelocity = Vector3.zero; projectileRB.velocity = Vector3.zero;
        projectileRB.useGravity = false;
        transform.position = originPos;
        transform.eulerAngles = originAngle;
        collisionCheck = true;
        gameObject.SetActive(false);
    }
}

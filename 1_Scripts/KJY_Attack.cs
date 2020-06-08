using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_Attack : MonoBehaviour
{
    Rigidbody attackRB;
    Transform camTrans;
    public bool AttackAble { get; set; }
    bool isCrashed = false;
    bool Attacking = false;
    public float attackTime;
    public float attackSpeed;
    public ParticleSystem attackParticle;
    public ParticleSystem  crashParticle;

    private void Awake()
    {
        attackRB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        AttackAble = false;
        camTrans = Camera.main.transform;    
    }

    private void Update()
    {
        if(AttackAble)
        {
            if(Input.GetKeyDown(KeyCode.F))
            {
                StartCoroutine(Attack());
            }
        }
    }
    [PunRPC]
    void AttackParticlePlay()
    {
        attackParticle.Play();
    }
    IEnumerator Attack()
    {
        GetComponent<PhotonView>().RPC("AttackParticlePlay", PhotonTargets.All);
        GetComponent<KJY_PlayerCtrl>().StopPlayer(true);
        AttackAble = false;
        Attacking = true;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, camTrans.eulerAngles.y, transform.eulerAngles.z);
        float elapsedTime = 0f;
        while (elapsedTime < attackTime)
        {
            if (isCrashed)
            {
                
                break;
            }
            elapsedTime += Time.fixedDeltaTime;
            attackRB.MovePosition(transform.position + transform.forward * attackSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        GetComponent<KJY_PlayerCtrl>().StopPlayer(false);
        Attacking = false;
        isCrashed = false;
        Debug.Log("대쉬 끝끝끝끝끝끝끝끝끝");
        yield return new WaitForSeconds(2f);
        AttackAble = true;
    }
    public void KillPlayer()
    {
        KHS_ObjectManager.instance.GetComponent<PhotonView>().
                   RPC("GameEndCheck", PhotonTargets.All, PhotonNetwork.player);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.collider.CompareTag("Ground") && !isCrashed && Attacking)
        {
            crashParticle.Play();
            isCrashed = true;
            PlayerPrefs.SetInt("Kill", PlayerPrefs.GetInt("Kill") + 1);
         
            if(collision.gameObject.layer == 8)
            {
                MainMenuAudioManager.instance.Play(AudioEnum.beShot);
                Invoke("KillPlayer", 2.3f);
                collision.gameObject.GetComponent<PhotonView>().RPC("DiePlayer2",PhotonTargets.All, PhotonNetwork.player);
            }
            if(collision.gameObject.layer == 9)
            {
                MainMenuAudioManager.instance.Play(AudioEnum.beShot);
                collision.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().KillNpc();
            }
            if(collision.gameObject.layer == 11)
            {
                MainMenuAudioManager.instance.Play(AudioEnum.beShot);
                KHS_ObjectManager.instance.CamBase.GetComponent<PhotonView>().
                    RPC("ProssesionRelease", PhotonTargets.All, PhotonNetwork.player);
                Debug.Log("빙의 해제");
            }
        }
    }
}

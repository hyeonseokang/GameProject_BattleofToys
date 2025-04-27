using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KJY_Possession : MonoBehaviour
{
    public Animator CrossHair;
    private KHS_TargetObj CamBase;
    private GameObject targetObj;
    public float possessionRange;
    public float possessionDelay;
    public float playerTime = 30f;
    public Image stateBar;
    public Text stateText;

    GameObject originPlayer;
    Transform camTrans;

    cakeslice.Outline ChoiceObject;
    bool inputCheck = false;
    bool choiceCheck = false;
    bool possEnabled = false;
    bool PossFirst = true;
    bool isPossEnable = true;

    Vector3 PlayerPos;    //here

    public GameObject PossParticle;   //here
    public GameObject ChangePossPart; //here

    public Image[] PossessionImageLoading;
    public GameObject PossessionLoadingbar;

    public int StateBarnum_;
    void Start()
    {
        CamBase = GetComponent<KHS_TargetObj>();
        targetObj = CamBase.targetObj;
        originPlayer = targetObj;
        camTrans = Camera.main.transform;
    }
    [PunRPC]
    void ProssesionRelease(PhotonPlayer _player)
    {
        if (_player != PhotonNetwork.player && possEnabled)
        {
            PossessionGo();
            Debug.Log("하셨습니다");
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            CheatPoss = !CheatPoss;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            CrossHair.SetBool("Cross", false);
            inputCheck = !inputCheck;
            if (!inputCheck)
            {
                if (ChoiceObject != null)
                {
                    cakeslice.Outline[] _outlineobject = ChoiceObject.GetComponentInParent<PhotonView>().
                    GetComponentsInChildren<cakeslice.Outline>();
                    for (int i = 0; i < _outlineobject.Length; i++)
                    {
                        _outlineobject[i].enabled = false;
                    }
                    ChoiceObject.GetComponentInParent<khs_arrowI>().explan.gameObject.SetActive(false);
                ChoiceObject.enabled = false;
                }
                ChoiceObject = null;
            }
                changePossParticle();     //here
            
        }
        if (inputCheck && !possEnabled)
        {
            PossessionChoice();
        }
        if (inputCheck && Input.GetMouseButtonDown(0)&& isPossEnable)
        {
            inputCheck = false;
            if (ChoiceObject != null)
            {
                PossPart();
                StartCoroutine("PossessionLoadingbarIe");
                originPlayer.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().Controllable = false;
                Invoke("PossessionGo", possessionDelay);
            }
        }
        if (End)
            PlayerTimeUpdate();
        PlayerPos = transform.position;
    }
    public void isPossEnableCh()
    {
        isPossEnable = true;
    }
    IEnumerator PossessionLoadingbarIe()
    {
        PossessionLoadingbar.SetActive(true);
        PossessionImageLoading[0].gameObject.SetActive(true);
        PossessionImageLoading[1].gameObject.SetActive(true);
        while (true)
        {
            yield return new WaitForSeconds(0.08f);
            PossessionImageLoading[1].rectTransform.sizeDelta =
                new Vector2(PossessionImageLoading[1].rectTransform.rect.width + 8.9f,
                12.0f);
            if(PossessionImageLoading[1].rectTransform.rect.width>=178)
            {
                PossessionImageLoading[2].gameObject.SetActive(true);
                break;
            }
         
        }
        for(int i =0;i< PossessionImageLoading.Length;i++)
        {
            PossessionImageLoading[i].gameObject.SetActive(false);
        }
        PossessionImageLoading[1].rectTransform.sizeDelta = new Vector2(0, 12.0f);
        PossessionLoadingbar.SetActive(false);
    }


    void PossPart()   //here
    {
        GameObject Particle = Instantiate(PossParticle, transform.position, transform.rotation);
        Particle.transform.parent = transform;
        Destroy(Particle, 2f);
    }
    void changePossParticle()  //here
    {
        GameObject Particle = Instantiate(ChangePossPart, transform.position/*KHS_ObjectManager.instance.GetComponent<GameInit>().Player.transform.position*/, ChangePossPart.transform.rotation);
        Particle.transform.parent = transform;
        Destroy(Particle, 3f);
    }
    bool End = true;
    void PlayerTimeUpdate()
    {
        if (!possEnabled && !PossFirst)
        {
            if (playerTime > 0f)
            {
                if (!CheatPoss)
                    playerTime -= Time.deltaTime;
                stateBar.fillAmount = playerTime / 30f;
                stateText.text = ((int)playerTime).ToString() + " / 30";
            }
            else
            {
                End = false;
                KHS_ObjectManager.instance.gameObject.GetComponent<PhotonView>().RPC("GameEndCheck2", PhotonTargets.All,
                    PhotonNetwork.player);
                Debug.Log("플레이어 사망");
            }
        }
        else playerTime = 30f;
    }

    void PossessionChoice()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(camTrans.position
            , camTrans.TransformDirection(Vector3.forward), out hit
            , 1000, 1 << 9 | 1 << 10);
        if (isHit)
        {
            cakeslice.Outline[] _outlineobject;
            if (ChoiceObject != null)
            {
                _outlineobject = ChoiceObject.GetComponentInParent<PhotonView>().
                    GetComponentsInChildren<cakeslice.Outline>();
                for (int i = 0; i < _outlineobject.Length; i++)
                {
                    _outlineobject[i].enabled = false;
                }
                ChoiceObject.GetComponentInParent<khs_arrowI>().explan.gameObject.SetActive(false);
            }
                
            ChoiceObject = null;

            if (ChoiceObject == null)
            {
                ChoiceObject = hit.collider.GetComponentInChildren<cakeslice.Outline>();
            }
            _outlineobject = hit.collider.
                GetComponentsInChildren<cakeslice.Outline>();
            for(int i=0;i< _outlineobject.Length;i++)
            {
                _outlineobject[i].enabled = true;
            }
            ChoiceObject.enabled = true;
            CrossHair.SetBool("Cross", true);
            ChoiceObject.GetComponentInParent<khs_arrowI>().explan.gameObject.SetActive(true);
            ChoiceObject.GetComponentInParent<khs_arrowI>().explan.RotationExplan(CamBase.GetComponentInChildren<Transform>());
        }
        else
        {
            if (ChoiceObject != null)
            {
                ChoiceObject.GetComponentInParent<khs_arrowI>().explan.gameObject.SetActive(false);
                ChoiceObject.enabled = false;
                cakeslice.Outline[] _outlineobject = ChoiceObject.GetComponentInParent<PhotonView>().
                    GetComponentsInChildren<cakeslice.Outline>();
                for (int i = 0; i < _outlineobject.Length; i++)
                {
                    _outlineobject[i].enabled = false;
                }
                ChoiceObject = null;
                CrossHair.SetBool("Cross", false);
            }
        }
    }

    public void PossessionGo()
    {
        if (!possEnabled)
        {
            PlayerPrefs.SetInt("Possession", PlayerPrefs.GetInt("Possession") + 1);
            PossFirst = false;
            ChoiceObject.GetComponentInParent<khs_arrowI>().explan.gameObject.SetActive(false);
            Debug.Log("빙의");
            possEnabled = true;

            //플레이어 비활성화
            EnablePlayer(false);

            //타겟 초기화
            targetObj = ChoiceObject.GetComponentInParent<PhotonView>().gameObject;
            targetObj.GetComponent<PhotonView>().RPC("ChangeLayer", PhotonTargets.AllBuffered, 11,PhotonNetwork.player);
            targetObj.GetComponent<PhotonView>().RequestOwnership();
            CamBase.GetComponent<PhotonView>().RPC("CameraPlayerSetRef", PhotonTargets.All, PhotonNetwork.player);

            //  KHS_ObjectManager.instance.GetComponent<PhotonView>().RPC("Player2Go", PhotonTargets.All, targetObj);
            targetObj.GetComponent<KJY_Layer>()._layerint = 11;
            targetObj.layer = 11;
            if (targetObj.GetComponent<KJY_Ability>()!=null)
            {
                if (targetObj.GetComponent<KJY_Ability>().getIsAbilityUse() == false)
                    KHS_ObjectManager.instance.AbilitySpriteSet(targetObj.GetComponent<KJY_Ability>().AbilityNumber(), true);
            }
            CamBase.targetObj = targetObj;
            CamBase.GetComponent<KJY_CamBase>().Arrownum = CamBase.targetObj.GetComponent<khs_arrowI>().ArrowInt;
            targetObj.GetComponentInChildren<cakeslice.Outline>().enabled = false;
            cakeslice.Outline[] _outlineobject = targetObj.
                    GetComponentsInChildren<cakeslice.Outline>();
            for (int i = 0; i < _outlineobject.Length; i++)
            {
                _outlineobject[i].enabled = false;
            }

            if (targetObj.CompareTag("Npc"))
            {
                if (targetObj.GetComponent<KJY_PlayerCtrl>() != null)
                {
                    targetObj.GetComponent<KJY_PlayerCtrl>().enabled = true;
                   
                }
                if(targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>()!=null)
                {
                    targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = true;
                }
                if(targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>() != null)
                {
                    targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().EnbaleNpc(false);
                }
                targetObj.GetComponent<KJY_Attack>().AttackAble = true;
                if (targetObj.GetComponent<KJY_Ability>() != null)
                    targetObj.GetComponent<KJY_Ability>().enabled = true;

                KHS_ObjectManager.instance.StateBarChange(targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().StateBarNum+4);
                KHS_ObjectManager.instance.f1ImageChange(targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().StateBarNum);
                KHS_ObjectManager.instance.Player[0].
                    GetComponent<KHS_StateScript>().StateNumber =
                     targetObj.
                     GetComponent<UnityStandardAssets.Characters.ThirdPerson
                     .AICharacterControl>().StateBarNum + 3;
                KHS_ObjectManager.instance.Player[1].GetComponent<PhotonView>().
                    RPC("ChangeStateNumber", PhotonTargets.All,
                    PhotonNetwork.player,
                    targetObj.
                     GetComponent<UnityStandardAssets.Characters.ThirdPerson
                     .AICharacterControl>().StateBarNum + 3);

                CamBase.GetComponent<KJY_CamBase>().setCameraPos(1);

                if (targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().StateBarNum == 14)
                    CamBase.GetComponent<KJY_CamBase>().setCameraPos(3);


            }
            else if (targetObj.CompareTag("FixedNpc"))
            {
                GetComponentInChildren<KJY_CamCollision>().ControlDst(true);
                targetObj.GetComponent<KJY_FixedNpc>().enabled = true;

                KHS_ObjectManager.instance.StateBarChange(targetObj.GetComponent<KJY_FixedNpc>().StateNum + 4);
                KHS_ObjectManager.instance.f1ImageChange(targetObj.GetComponent<KJY_FixedNpc>().StateNum);
                KHS_ObjectManager.instance.Player[0].
                    GetComponent<KHS_StateScript>().StateNumber =
                     targetObj.
                     GetComponent<KJY_FixedNpc>().StateNum + 3;
                KHS_ObjectManager.instance.Player[1].GetComponent<PhotonView>().
                    RPC("ChangeStateNumber", PhotonTargets.All,
                    PhotonNetwork.player,
                    targetObj.
                     GetComponent<KJY_FixedNpc>().StateNum + 3);
                CamBase.GetComponent<KJY_CamBase>().setCameraPos(3);
            }
            CrossHair.SetBool("Cross", false);
            StartCoroutine(PossProcess());
        }
        else if (possEnabled)
        {
            Invoke("isPossEnableCh", 7);
            Debug.Log("해제");
            possEnabled = false;
            isPossEnable = false;
            //플레이어 활성화

            EnablePlayer(true);


            KHS_ObjectManager.instance.AbilitySpriteSet(-1, false);
            

            //타겟 복원
            targetObj.GetComponentInChildren<cakeslice.Outline>().enabled = false;
            if (targetObj.CompareTag("Npc"))
            {
                targetObj.GetComponent<PhotonView>().RequestOwnership();
                targetObj.GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = true;
                targetObj.GetComponent<PhotonView>().RPC("ChangeLayer", PhotonTargets.All, 9);
                targetObj.GetComponent<KJY_Layer>()._layerint = 9;
                targetObj.layer = 9;
                targetObj.GetComponent<KJY_Attack>().AttackAble = false;
                if (targetObj.GetComponent<KJY_Ability>() != null)
                    targetObj.GetComponent<KJY_Ability>().enabled = false;
                if (targetObj.GetComponent<KJY_PlayerCtrl>() != null)
                {
                    targetObj.GetComponent<KJY_PlayerCtrl>().enabled = false;
                }
                if (targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>() != null)
                {
                    targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().enabled = false;
                }
                if (targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>()!=null)
                {
                    targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().EnbaleNpc(true);
                    
                    if (targetObj.GetComponent<KJY_PossTime>().RealTime == 0f)
                    {
                        targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().KillNpc();
                    }
                }
            } 
            else if (targetObj.CompareTag("FixedNpc"))
            {
                setCAM = true;
                GetComponentInChildren<KJY_CamCollision>().ControlDst(false);
                targetObj.GetComponent<PhotonView>().RPC("ChangeLayer", PhotonTargets.All, 10);
                targetObj.GetComponent<KJY_Layer>()._layerint = 10;
                targetObj.layer = 10;
                targetObj.GetComponent<KJY_FixedNpc>().enabled = false;
                if (targetObj.GetComponent<KJY_PossTime>().RealTime == 0f)
                {
                    targetObj.GetComponent<KJY_PossTime>().RecoverTime();
                }
            }
            KHS_ObjectManager.instance.StateBarChange(StateBarnum_);
            KHS_ObjectManager.instance.Player[0].GetComponent<KHS_StateScript>().StateNumber = StateBarnum_;
            targetObj = originPlayer;
            KHS_ObjectManager.instance.GetComponent<PhotonView>().RPC("Player2Go", PhotonTargets.All,PhotonNetwork.player);
            CamBase.GetComponent<PhotonView>().RPC("CameraPlayerSetRef", PhotonTargets.All, PhotonNetwork.player);
            KHS_ObjectManager.instance.Player2Target = targetObj;
            CamBase.targetObj = originPlayer;
            CamBase.GetComponent<KJY_CamBase>().Arrownum = CamBase.targetObj.GetComponent<khs_arrowI>().ArrowInt;   
            ChoiceObject = null;
            CamBase.GetComponent<KJY_CamBase>().setCameraPos(2);

            //if (setCAM)
            //    CamBase.GetComponent<KJY_CamBase>().setCameraPos(3);

            //CamBase.GetComponent<KJY_CamBase>().setCameraPos(1);
        }
    }
    bool setCAM = false;
    private void EnablePlayer(bool state)
    {
        originPlayer.GetComponent<Khs_PlayerCtrl>().activeOnOff(state);
        if(state)
        {
            RaycastHit hit;
            float distance = 2f;
            bool isHit = false;
            originPlayer.transform.eulerAngles = new Vector3(originPlayer.transform.eulerAngles.x, camTrans.eulerAngles.y, originPlayer.transform.eulerAngles.z);
            while (true)
            {
                distance += 1f;
                Vector3 checkPoint = new Vector3(0f, 10f,-distance);
                isHit = Physics.Raycast(transform.TransformPoint(checkPoint), Vector3.down, out hit);
                if(hit.collider.CompareTag("Ground") && isHit)
                {
                    break;
                }
            }
            originPlayer.GetComponent<UnityStandardAssets.Characters.ThirdPerson.ThirdPersonUserControl>().Controllable = true;

            originPlayer.transform.position = new Vector3(targetObj.transform.position.x, originPlayer.transform.position.y, targetObj.transform.position.z);
            originPlayer.transform.position = originPlayer.transform.TransformPoint(Vector3.back * distance);
        }
        originPlayer.GetComponent<CapsuleCollider>().enabled = state;
    }
    int ttime;
    bool CheatPoss = false;
    IEnumerator PossProcess()
    {
        if (targetObj != null&&targetObj.GetComponent<KJY_PossTime>()!=null)
        {
            ttime = (int)targetObj.GetComponent<KJY_PossTime>().RealTime;
            while (targetObj.GetComponent<KJY_PossTime>() != null && targetObj.GetComponent<KJY_PossTime>().RealTime >= 0)
            {
                stateText.text = ((int)targetObj.GetComponent<KJY_PossTime>().RealTime).ToString() + " / " + ttime;
                if (!CheatPoss)
                    targetObj.GetComponent<KJY_PossTime>().RealTime -= Time.deltaTime;
                stateBar.fillAmount = targetObj.GetComponent<KJY_PossTime>().RealTime
                    / targetObj.GetComponent<KJY_PossTime>().OriginTime;
                if (!possEnabled)
                {
                    yield break;
                }
                yield return new WaitForEndOfFrame();

            }
            if (targetObj.GetComponent<KJY_PossTime>() != null)
                targetObj.GetComponent<KJY_PossTime>().RealTime = 0f;
        }
        if (possEnabled)
        {
            PossessionGo();
        }
    }
}

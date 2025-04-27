using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Ability
{
    None, Copy, Awakening, Rader, Control , Release
}

public class KJY_Ability : MonoBehaviour
{
    bool isEnabled = false;
    private GameObject targetImg;
    public Ability ability;
    public GameObject changeskillparticle;

    public int AbilityNumber()
    {
        if(ability == Ability.Copy)
        {
            return 4;
        }
        else if (ability == Ability.Awakening)
        {
            return 2;
        }
        else if (ability == Ability.Rader)
        {
            return 1;
        }
        else if (ability == Ability.Control)
        {
            return 3;
        }
        return -1;
    }
    //복사용
    public GameObject cloneObj;
    //레이더용
    public float checkRadius;
    public GameObject exclamation;
    private bool RaderOn = false;
    private GameObject exclamationObj = null;
    //
    public ParticleSystem AbilityParticle;   //here

    RaycastHit rayHit;
    Transform camTrans;
    GameObject targetObj;

    int clickCount = 0;
    cakeslice.Outline ChoiceObject;
    bool isAbilityUse = false;
    public bool getIsAbilityUse()
    {
        return isAbilityUse;
    }
    public int NpcNum;
    private void Start()
    {
        camTrans = Camera.main.transform;
        targetImg = KHS_ObjectManager.instance.CircleImage;
    }

    private void OnDisable()
    {
        Debug.Log("가가각가고고고고곡고고");
        isEnabled = false;
        targetImg.SetActive(false);
        AbilityParticle.Pause();
        Destroy(AbilityParticle.gameObject);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)&& !isAbilityUse) // 특수능력 활성화
        {
            isEnabled = !isEnabled;
            KHS_ObjectManager.instance.AbilitySpriteSet(isEnabled, AbilityNumber());
            changeSkillParti();
        }
        if (isEnabled)
        {
            if (ability == Ability.Copy)
            {
                CopyUpdate();
            }
            else if(ability == Ability.Awakening)
            {
                AwakeningUpdate();
            }
            else if(ability == Ability.Rader)
            {
                RaderUpdate();
            }
            else if(ability == Ability.Control)
            {
                ControlUpdate();
            }
        }
        else
        {
            targetImg.SetActive(false);
        }

        if(RaderOn)
        {
            Rader();
        }
    }

    void changeSkillParti()  //here
    {
        GameObject Particle = Instantiate(changeskillparticle, transform.position/*KHS_ObjectManager.instance.GetComponent<GameInit>().Player.transform.position*/, changeskillparticle.transform.rotation);
        Particle.transform.parent = transform;
        Destroy(Particle, 3f);
    }

    private void RaderUpdate()
    {
        if (Input.GetMouseButtonDown(0) && isEnabled && !RaderOn)
        {
            AbilityPrefs(1);
            AbilityParticle.Play();
            Debug.Log("레이더 온");
            RaderOn = true;
            isEnabled = false;
            isAbilityUse = true;
        }
    }
    private void SetExclamation(bool state)
    {
        if (exclamationObj == null)
        {
            exclamationObj = Instantiate(exclamation, transform.position + Vector3.up * 3f, Quaternion.identity, transform);
        }
        exclamationObj.SetActive(state);
    }
    private void Rader()
    {
        RaycastHit raderHit;
        Debug.Log("감자만쳐왔어요");
        if (Physics.SphereCast(transform.position, checkRadius, transform.forward, out raderHit, checkRadius, 1 << 11))
        {
            Debug.Log("감자고구마 왔어요");
            SetExclamation(true);
        }
        else
        {
            SetExclamation(false);
        }
    }




    public void AbilityPrefs(int _a)
    {
        PlayerPrefs.SetInt("Ability", PlayerPrefs.GetInt("Ability") + _a);
    }


    private void AwakeningUpdate()
    {
        
        if (Input.GetMouseButtonDown(0) && isEnabled)
        {
            AbilityPrefs(1);
            AbilityParticle.Play(); //here
            Debug.Log("각성");
            isEnabled = false;
            isAbilityUse = true;
            gameObject.GetComponent<KJY_PossTime>().RecoverTime();
            KHS_ObjectManager.instance.AbilitySpriteSet(false, AbilityNumber());
            KHS_ObjectManager.instance.AbilitySpriteSet(-1, true);
        }
    }
    private void CopyUpdate()
    {
        //복사
        if (Physics.Raycast(camTrans.position, camTrans.TransformDirection(Vector3.forward), out rayHit))
        {
            if (rayHit.collider.CompareTag("Ground"))
            {
                targetImg.SetActive(true);
                targetImg.transform.position = rayHit.point + Vector3.up * 0.01f;
                if (Input.GetMouseButtonDown(0) && isEnabled)
                {
                    AbilityPrefs(1);
                    AbilityParticle.transform.parent = null;
                    AbilityParticle.transform.position = rayHit.point;
                    AbilityParticle.Play(); //here
                    MainMenuAudioManager.instance.Play(AudioEnum.Copy);  //here
                    KHS_ObjectManager.instance.gameObject.GetComponent<KJY_NpcSpawn>().CreateNpc(NpcNum,
                        rayHit.point.x, rayHit.point.z);
                    KHS_ObjectManager.instance.AbilitySpriteSet(false, AbilityNumber());
                    KHS_ObjectManager.instance.AbilitySpriteSet(-1, true);
                    // KJY_Ability clone = Instantiate(cloneObj, rayHit.point, Quaternion.identity).GetComponent<KJY_Ability>();
                    // clone.cloneObj = cloneObj;
                    isEnabled = false;
                    isAbilityUse = true;
                }
            }
            else
            {
                targetImg.SetActive(false);
            }
        }
    }
    private void ControlUpdate()
    {
        //매혹?? 
        bool isHit = Physics.Raycast(camTrans.position, camTrans.TransformDirection(Vector3.forward), out rayHit);
        if (Input.GetMouseButtonDown(0) && isEnabled && isHit)
        {
            if (rayHit.collider.CompareTag("Npc") && clickCount == 0)
            {
                clickCount++;
                targetObj = rayHit.collider.gameObject;
                cakeslice.Outline[] _outline = targetObj.GetComponentsInChildren<cakeslice.Outline>();
                for (int i = 0; i < _outline.Length; i++)
                {
                    _outline[i].enabled = true;
                }
            }
            if (rayHit.collider.CompareTag("Ground") && clickCount == 1)
            {
                AbilityPrefs(1);
                AbilityParticle.transform.parent = null;
                AbilityParticle.transform.position = rayHit.point;
                AbilityParticle.Play();  //here
                MainMenuAudioManager.instance.Play(AudioEnum.Temptation);  //here
                targetObj.GetComponent<PhotonView>().RequestOwnership();
                targetObj.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().SetNpcDestination(rayHit.point);
                cakeslice.Outline[] _outline = targetObj.GetComponentsInChildren<cakeslice.Outline>();
                for (int i = 0; i < _outline.Length; i++)
                {
                    _outline[i].enabled = false;
                }
                isEnabled = false;
                isAbilityUse = true;
                clickCount = 0;

                KHS_ObjectManager.instance.AbilitySpriteSet(false, AbilityNumber());
                KHS_ObjectManager.instance.AbilitySpriteSet(-1, true);
            }
        }
        if (clickCount == 1)
        {
            if (rayHit.collider.CompareTag("Ground"))
            {
                targetImg.SetActive(true);
                targetImg.transform.position = rayHit.point + Vector3.up * 0.01f;
            }
            else
            {
                targetImg.SetActive(false);
            }
        }
    }
    private void ReleaseUpdate()
    {
        Debug.Log("릴리즈모드 활성화");
        if (Physics.Raycast(camTrans.position, camTrans.TransformDirection(Vector3.forward), out rayHit))
        {
            Debug.DrawRay(camTrans.position, camTrans.TransformDirection(Vector3.forward));
            if (rayHit.collider.CompareTag("Npc")||
                rayHit.collider.CompareTag("FixedNpc"))
            {
                Debug.Log("검사중 검사중 띠링띠링");
                if (Input.GetMouseButtonDown(0) && isEnabled)
                {
                    Debug.Log("고고씽1111");
                    KHS_ObjectManager.instance.CamBase.GetComponent<PhotonView>().RPC("ProssesionRelease", PhotonTargets.All, PhotonNetwork.player);

                    isEnabled = false;
                    isAbilityUse = true;
                }
            }
        }
    }
}

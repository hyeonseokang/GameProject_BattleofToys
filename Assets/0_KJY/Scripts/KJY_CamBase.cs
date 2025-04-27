using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_CamBase : MonoBehaviour
{
    private KHS_TargetObj targetObj;
    public float followSpeed;
    public float clampAngle;
    public float clampAngle2;
    public float Sensitivity;
    public float camHeight;
    float rotX;
    float rotY;


    public GameObject Arrow;
    public int Arrownum;
    public float[] ArrowY;

    public Vector2[] CamPos;

    public GameObject[] Camera;
    public void setCameraPos(int _is)
    {
        if (_is == 1)
        {
            GetComponentInChildren<Camera>().transform.localPosition =
                new Vector3(0, CamPos[1].x, CamPos[1].y);
            Debug.Log("카메라 셋팅 ㅗ고고고고고곡");
            GetComponentInChildren<KJY_CamCollision>().setCam(new Vector3(0, CamPos[1].x, CamPos[1].y));
        }
        else if (_is == 2)
        {
            GetComponentInChildren<Camera>().transform.localPosition =
                new Vector3(0, CamPos[0].x, CamPos[0].y);
            Debug.Log("카메라 셋팅 ㅗㅁㅈㅍㅁㅈㅍㅈㅁㅍ고고고고고곡");
            GetComponentInChildren<KJY_CamCollision>().setCam(new Vector3(0, CamPos[0].x, CamPos[0].y));
        }
        else if(_is ==3)
        {
            GetComponentInChildren<Camera>().transform.localPosition =
                new Vector3(0, CamPos[2].x, CamPos[2].y);
            GetComponentInChildren<KJY_CamCollision>().setCam(new Vector3(0, CamPos[2].x, CamPos[2].y));
        }
        



    }
    private void Start()
    {
        StartInit();
    }
    public void StartInit()
    {
        targetObj = GetComponent<KHS_TargetObj>();
        Arrownum = targetObj.targetObj.GetComponent<khs_arrowI>().ArrowInt;
    }
    public GameObject GetTargetObj()
    {
        return targetObj.targetObj;
    }
    private void Update()
    {
        CameraAngle();
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            KHS_ObjectManager.instance.Arrow_[0].GetComponent<KHS_ArrowText>().ArrowText.gameObject.SetActive(false);
            KHS_ObjectManager.instance.Arrow_[1].GetComponent<KHS_ArrowText>().ArrowText.gameObject.SetActive(false);
            KHS_ObjectManager.instance.Arrow_[0].GetComponentInChildren<Renderer>().material.color = new Color(0.941f, 0.25f, 0.25f, 0.854f);
            KHS_ObjectManager.instance.Arrow_[1].GetComponentInChildren<Renderer>().material.color = new Color(0.25f, 0.30f, 0.95f, 0.854f);
            KHS_ObjectManager.instance.Arrow_[0].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            KHS_ObjectManager.instance.Arrow_[1].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            CameraSet(0);  // 카메라 1번
        }
        else if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            KHS_ObjectManager.instance.Arrow_[0].GetComponent<KHS_ArrowText>().ArrowText.gameObject.SetActive(true);
            KHS_ObjectManager.instance.Arrow_[1].GetComponent<KHS_ArrowText>().ArrowText.gameObject.SetActive(true);
            KHS_ObjectManager.instance.Arrow_[0].GetComponentInChildren<Renderer>().material.color = new Color(0.941f, 0.25f, 0.25f, 1f);
            KHS_ObjectManager.instance.Arrow_[1].GetComponentInChildren<Renderer>().material.color = new Color(0.25f, 0.30f, 0.95f, 1f);
            KHS_ObjectManager.instance.Arrow_[0].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            KHS_ObjectManager.instance.Arrow_[1].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            CameraSet(1);  // 카메라 2번
        }
        else if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            KHS_ObjectManager.instance.Arrow_[0].GetComponent<KHS_ArrowText>().ArrowText.gameObject.SetActive(false);
            KHS_ObjectManager.instance.Arrow_[1].GetComponent<KHS_ArrowText>().ArrowText.gameObject.SetActive(false);
            KHS_ObjectManager.instance.Arrow_[0].GetComponentInChildren<Renderer>().material.color = new Color(0.941f, 0.25f, 0.25f, 1f);
            KHS_ObjectManager.instance.Arrow_[1].GetComponentInChildren<Renderer>().material.color = new Color(0.25f, 0.30f, 0.95f, 1f);

            KHS_ObjectManager.instance.Arrow_[0].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            KHS_ObjectManager.instance.Arrow_[1].transform.localScale = new Vector3(2.5f, 2.5f, 2.5f);
            CameraSet(2);    // 카메라 3번
        }
        else if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            KHS_ObjectManager.instance.Arrow_[0].GetComponentInChildren<Renderer>().material.color = new Color(0.941f, 0.25f, 0.25f, 0.854f);
            KHS_ObjectManager.instance.Arrow_[1].GetComponentInChildren<Renderer>().material.color = new Color(0.25f, 0.30f, 0.95f, 0.854f);
            KHS_ObjectManager.instance.Arrow_[0].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            KHS_ObjectManager.instance.Arrow_[1].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            CameraPlayerSet();   // 적 있는 카메라로 ㄱㄱ
        }
        //else if (Input.GetKeyUp(KeyCode.Alpha5))
        //    ArrowSizeSet();     // 화살표 사이즈 키우기
        else if (Input.GetKeyUp(KeyCode.Alpha6))
            PlayerOn();   // 적 화살표 보이기    
        //else if(Input.GetKeyUp(KeyCode.Alpha7))
        //{
        //    a += 0.005f;
        //    KHS_ObjectManager.instance.Arrow_[0].transform.localScale = new Vector3(a, a, a);
        //    KHS_ObjectManager.instance.Arrow_[1].transform.localScale = new Vector3(a, a, a);
        //}
        //else if (Input.GetKeyUp(KeyCode.Alpha8))
        //{
        //    a -= 0.005f;
        //    KHS_ObjectManager.instance.Arrow_[0].transform.localScale = new Vector3(a, a, a);
        //    KHS_ObjectManager.instance.Arrow_[1].transform.localScale = new Vector3(a, a, a);
        //}
    }
    float a = 0.1f;
    bool _is = false;

    bool isArrowSize = false;
    bool isPlayerOn = false;
    void PlayerOn()
    {
        KHS_ObjectManager.instance.Arrow_[1].SetActive(!isPlayerOn);
        isPlayerOn = !isPlayerOn;
    }
    void ArrowSizeSet()
    {
        if(!isArrowSize)
        {
            isArrowSize = true;
            KHS_ObjectManager.instance.Arrow_[0].transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
            KHS_ObjectManager.instance.Arrow_[1].transform.localScale = new Vector3(1.8f, 1.8f, 1.8f);
        }
        else
        {
            isArrowSize = false;
            KHS_ObjectManager.instance.Arrow_[0].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
            KHS_ObjectManager.instance.Arrow_[1].transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        }
    }
    [PunRPC]
    public void CameraPlayerSetRef(PhotonPlayer _player)
    {

        if (_is)
        {
            targetob2 = targetObj.targetObj;
            targetObj.targetObj = KHS_ObjectManager.instance.Player2Target;
            KHS_ObjectManager.instance.Arrow_[0].SetActive(false);
        }
    }
    public GameObject targetob2;
    void CameraPlayerSet()
    {
        if(!_is)
        {
            _is = true;
            targetob2 = targetObj.targetObj;
            targetObj.targetObj = KHS_ObjectManager.instance.Player2Target;
            KHS_ObjectManager.instance.Arrow_[0].SetActive(false);
        }
        else
        {
            _is = false;
            targetObj.targetObj = targetob2;
            KHS_ObjectManager.instance.Arrow_[0].SetActive(true);
        }
    }
    void CameraSet(int _i)
    {
        for(int i=1;i< Camera.Length;i++)
        {
            Camera[i].SetActive(false);
        }
        Camera[_i].SetActive(true);

      
    }
    private void CameraAngle()
    {
        rotY += Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        rotX -= Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;
        rotX = Mathf.Clamp(rotX, -clampAngle2 * 0.5f, clampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0f);
        transform.rotation = localRotation;
    }

    private void LateUpdate()
    {
        CameraFollow();
    }
    private void CameraFollow()
    {
        Vector3 targetPos = targetObj.targetObj.transform.position + Vector3.up * camHeight;
        transform.position = Vector3.MoveTowards(transform.position, targetPos, followSpeed * Time.deltaTime);
        Arrow.transform.position = targetObj.targetObj.transform.position;
        Vector3 addVec3 = Arrow.transform.position;
        Arrow.transform.position = new Vector3(addVec3.x, addVec3.y + ArrowY[Arrownum], addVec3.z);
    }
}

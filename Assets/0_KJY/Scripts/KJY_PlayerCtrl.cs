using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_PlayerCtrl : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float jumpPower;
    public float turnSmoothTime;
    public float speedSmoothTime;

    float currentSpeed;
    bool isGround = true;
    bool canControl = true;

    public AudioSource MoveSound;
    Animator playerAnimator;
    Rigidbody playerRB;
    Transform camTrans;
    Vector2 inputDir;
    public Animator chiledAnimator;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerRB = GetComponent<Rigidbody>();
        MoveSound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        camTrans = Camera.main.transform;
        MoveSound.volume = 0.0f;
        MoveSound.mute = false;
    }
    private void OnDisable()
    {
        SetAniState(0);
    }

    private void Update()
    {
        //플레이어 키 입력
        if (canControl)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
            inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        if (inputDir == Vector2.zero)
        {
            //멈출때
            SetAniState(0);
            if (isMove == true)
            {
                StopAllCoroutines();
                StartCoroutine(MoveSoundOnOff2());
            }
            isMove = false;
            // MoveSound.mute = true; //here
        }
        else if(inputDir != Vector2.zero)
        {
            //움직일때
            // MoveSound.mute = false;  //here
            if(isMove ==false)
            {
                StopAllCoroutines();
                StartCoroutine(MoveSoundOnOff());
            }
            isMove = true;
            SetAniState(1);
            MovePlayer(inputDir);
        }
    }
    bool isMove = false;
    private float MoveSoundTime = 1.2f;
    private float MoveSoundTime2 = 1.0f;
    private float Timef = 0;
    private float FadeTime;
    IEnumerator MoveSoundOnOff()
    {
        yield return new WaitForSeconds(0.4f);
        float MoveVolume = MoveSound.volume;
        Timef = 0;
        MoveVolume = Mathf.Lerp(0f, 1f, Timef);

        while(MoveVolume < 1f)
        {
            Timef += Time.deltaTime / MoveSoundTime;
            MoveVolume = Mathf.Lerp(0.0f, 1f, Timef);
            MoveSound.volume = MoveVolume;
            yield return null;
        }
    }
    IEnumerator MoveSoundOnOff2()
    {
        float MoveVolume = MoveSound.volume;
        Timef = 1;
        MoveVolume = Mathf.Lerp(0f, 1f, Timef);

        while (MoveVolume > 0f)
        {
            Timef -= Time.deltaTime / MoveSoundTime2;
            MoveVolume = Mathf.Lerp(0.0f, 1f, Timef);
            MoveSound.volume = MoveVolume;
            yield return null;
        }
    }
    public bool isChiled;
    public void SetAniState(int num)
    {
        if (isChiled)
        {
            chiledAnimator.SetInteger("State", num);
        }
        else
            playerAnimator.SetInteger("State", num);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            isGround = true;
        }
    }


    public void StopPlayer(bool tof)
    {
        SetAniState(0);
        if(tof)
        {
            canControl = false;
            inputDir = Vector2.zero;
        }
        else
        {
            canControl = true;
        }

    }


    private void Jump()
    {
        if (isGround)
        {
            isGround = false;
            playerRB.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void MovePlayer(Vector2 dir)
    {
        //플레이어 각도 변경
        float turnVelocity = 0f;
        float playerRotation = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg + camTrans.eulerAngles.y;
        transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle
            (transform.eulerAngles.y, playerRotation, ref turnVelocity, turnSmoothTime);

        //속도계산
        float speedVelocity = 0f;
        float targetSpeed = (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed) * dir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedVelocity, speedSmoothTime);

        //
        playerRB.MovePosition(transform.position + transform.forward * currentSpeed * Time.fixedDeltaTime);
    }


    public void activeOnOff(bool _active)
    {
        this.GetComponent<PhotonView>().RPC("DisableObject", PhotonTargets.All, _active);
    }
    [PunRPC]
    private void DisableObject(bool _active)
    {
        this.gameObject.SetActive(_active);
    }
}

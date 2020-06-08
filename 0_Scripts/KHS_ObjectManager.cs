using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KHS_ObjectManager : MonoBehaviour
{
    public static KHS_ObjectManager instance;
    private void Awake()
    {
        Debug.Log("오브젝트매니저 활성");

        if (instance == null)
            instance = this;

    }

    public Text TimeText;
    public GameObject[] Player;
    public GameObject CamBase;

    public Image StateBarFaceImage;
    public Sprite[] StateBarFaceSprite;

    public Sprite[] f1Sprite;
    public Image f1Image;
    public KHS_TargetObj targetobj;

    public GameObject[] Arrow_;
    public Sprite[] GameEndStateBar;
    public int GameEndNum;

    public ParticleSystem[] particlesystem;
    public void StateBarChange(int _num)
    {
        StateBarFaceImage.sprite = StateBarFaceSprite[_num - 1];
        GameEndNum = _num - 1;
    }
    public void f1ImageChange(int _num)
    {
        f1Image.sprite = f1Sprite[_num - 1];
    }
    public GameObject CircleImage;


    public Image[] SkillImage;
    public Sprite[] SkillSprite1;
    public Sprite[] SkillSprite2;
    int Num_ = 0;
    void ParticleOff()
    {
        particlesystem[Num_].Stop();
        particlesystem[Num_].gameObject.SetActive(false);
        particlesystem[Num_].gameObject.transform.position = new Vector3(0, 0, 0);
    }
    public void AbilitySpriteSet(int _number,bool _is)
    {
        if (_number != -1)
        {
            if (_is)
            {
                SkillImage[_number - 1].sprite = SkillSprite1[_number - 1];
                Debug.Log("뽀롱뽀롱뽀롱뽀롱뽀로로로");
                particlesystem[_number - 1].gameObject.SetActive(true);
                particlesystem[_number - 1].gameObject.transform.position = new Vector3(50, -53, 0);
                particlesystem[_number - 1].Play();
                particlesystem[_number - 1].GetComponent<Animator>().SetTrigger("State");
                Num_ = _number - 1;
                Invoke("ParticleOff", 2.2f);
            }
            else
                SkillImage[_number - 1].sprite = SkillSprite2[_number - 1];

            for (int i = 0; i < 4; i++)
            {
                if (_number - 1 != i)
                {
                    SkillImage[i].sprite = SkillSprite2[i];
                }
            }
        }
        if(_number == -1)
        {
            for(int i=0;i<4;i++)
            {
                SkillImage[i].sprite = SkillSprite2[i];
            }
        }
       
    }
    public void AbilitySpriteSet(bool _is , int _number)
    {
        if(_is)
        {
            SkillImage[_number - 1].rectTransform.sizeDelta = new Vector2(110, 110);
        }
        else
        {
            SkillImage[_number - 1].rectTransform.sizeDelta = new Vector2(130, 130);
        }

        for (int i = 0; i < 4; i++)
        {
            if (_number - 1 != i)
            {
                SkillImage[i].rectTransform.sizeDelta = new Vector2(130, 130);
            }
        }
    }

    public GameObject Player2Target;

    [PunRPC]
    void Player2Go(PhotonPlayer _PLAYER)
    {
        if(PhotonNetwork.player != _PLAYER)
        {
            Player2Target = KHS_ObjectManager.instance.Player[1];
        }
    }
}


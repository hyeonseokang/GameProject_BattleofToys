using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MainMenumanager : MonoBehaviour {
    public static MainMenumanager Instance { get; private set; }

    public int Money = 10000;
    public int WinScore = 3;
    public int LoseScore = 5;
    public int[] Weapon;
    public int[] Havenum;
    public bool[] Equip;
    public float WinningRate;
    public float WinningRate2;
    public int i = 0;
    public Image IngameImg;
    public Sprite[] IngameSpt;
    public bool MatchingState;
    public bool loadingstate;

    void Awake()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        else
        {
            Destroy(this.gameObject);
        }
    }
    private void Start()
    {

    }
    void Update()
    {

    }

    public void ButtonPointerEnter(RectTransform _button)
    {
        _button.localScale = new Vector3(1.05f, 1.05f, 1.05f);
        _button.GetComponent<Outline>().enabled = true;
    }
    public void ButtonPointerExit(RectTransform _button)
    {
        _button.localScale = new Vector3(1, 1, 1);
        _button.GetComponent<Outline>().enabled = false;
    }
}

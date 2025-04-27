using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alpha : MonoBehaviour
{
    //private Renderer rend;
    private GameObject a;
    private int i;
    //public float time;
    // Use this for initialization
    void Start()
    {
        //rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    StartFade(a, 1f, 1f);
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    StartFade(a, 1f, 0f);
        //}
    }

    public void StartFade(GameObject obj,float duration, float alpha)
    {
        Debug.Log("헤헤헤 시작됬당");
        a = obj;
        StartCoroutine(FadeCor(duration, alpha));
    }

    IEnumerator FadeCor(float duration, float alpha)
    {
        float time = 0f;
        Debug.Log(a);
        Color originColor = a.GetComponentInChildren<Renderer>().material.color;
        Color goalColor = new Color(originColor.r, originColor.g, originColor.b, alpha);
        while (time < duration)
        {
            time += Time.fixedDeltaTime;
            SetColor(Color.Lerp(originColor, goalColor, time / duration));
            yield return new WaitForFixedUpdate();
        }
        SetColor(goalColor);
    }

    void SetColor(Color c)
    {
        a.GetComponentInChildren<Renderer>().material.color = c;
    }
}

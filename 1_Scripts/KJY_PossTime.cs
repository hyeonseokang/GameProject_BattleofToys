using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_PossTime : MonoBehaviour
{
    public float minTime;
    public float maxTime;
    public float RealTime { get; set; }
    public float OriginTime { get; set; }

    private void OnEnable()
    {
        OriginTime = Random.Range(minTime, maxTime);
        RealTime = OriginTime;
    }

    public void RecoverTime()
    {
        RealTime = OriginTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_CamCollision : MonoBehaviour
{
    public float minDst;
    private float maxDst;

    public float Smooth;
    public float dstPercent;
    Vector3 dollyDir;
    float Distance;
    public Vector3 originPos;
    public Vector3 secPos;

    private void Awake()
    {
        originPos = transform.localPosition;
        dollyDir = transform.localPosition.normalized;
        Distance = transform.localPosition.magnitude;
        maxDst = transform.localPosition.magnitude;
    }
    public void setCam(Vector3 _tras)
    {
        originPos = _tras;
        dollyDir = _tras.normalized;
        Distance = _tras.magnitude;
        maxDst = _tras.magnitude;
    }
    public void ControlDst(bool isFixed)
    {
        if(isFixed)
        {
            transform.localPosition = secPos;
        }
        else
        {
            transform.localPosition = originPos;
        }
        dollyDir = transform.localPosition.normalized;
        Distance = transform.localPosition.magnitude;
        maxDst = transform.localPosition.magnitude;
    }

    private void Update()
    {
        Vector3 CamPos = transform.parent.TransformPoint(dollyDir * maxDst);
        RaycastHit hit;
        if(Physics.Linecast(transform.parent.position, CamPos, out hit))
        {
            Distance = Mathf.Clamp((hit.distance * dstPercent), minDst, maxDst);
        }
        else
        {
            Distance = maxDst;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, dollyDir * Distance, Smooth * Time.deltaTime);
    }
}

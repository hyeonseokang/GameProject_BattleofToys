using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KJY_FixedNpc : MonoBehaviour
{
    public KJY_Projectile projectile;
    public bool launchAble;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && launchAble)
        {
            projectile.Launch();
        }
    }

    public int StateNum;
}

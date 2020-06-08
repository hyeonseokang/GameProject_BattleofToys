using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KJY_Npc : MonoBehaviour
{
    Vector3 targetPos;
    Animator npcAnimator;
    NavMeshAgent npcNav;
    KJY_PlayerCtrl player;
    bool isMoving;
    public float minRange;
    public float maxRange;
    public float checkHeight;

    private void Awake()
    {
        npcAnimator = GetComponent<Animator>();
        player = GetComponent<KJY_PlayerCtrl>();
        npcNav = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        EnableNpc();
    }

    public void EnableNpc()
    {
        //npcNav.enabled = true;
        npcNav.ResetPath();
        npcNav.isStopped = true;
        StartCoroutine(MovePattern());
    }
    public void DisableNpc()
    {
        npcNav.ResetPath();
        npcNav.isStopped = true;
        //npcNav.enabled = false;
        StopAllCoroutines();
    }

    private void Update()
    {
        if (npcNav.isStopped)
        {
            npcAnimator.SetInteger("State", 0);
        }
        else
        {
            npcAnimator.SetInteger("State", 1);
        }
    }

    public void SetNpcDestination(Vector3 pos)
    {
        Debug.Log("매혹됨");
        npcNav.isStopped = false;
        npcNav.ResetPath();
        npcNav.SetDestination(pos);
    }

    IEnumerator MovePattern()
    {
        yield return new WaitForSeconds(Random.Range(3f, 6f));
        RaycastHit navHit;

        while (npcNav.enabled)
        {
            targetPos = RandomTargetPos();
            if (Physics.Raycast(targetPos + Vector3.up * checkHeight, Vector3.down, out navHit))
            {
                if(navHit.collider.CompareTag("Ground"))
                {
                    npcNav.isStopped = false;
                    npcNav.SetDestination(targetPos);
                    yield return new WaitForSeconds(1f);
                    yield return new WaitUntil(() => npcNav.remainingDistance <= npcNav.stoppingDistance);
                    npcNav.isStopped = true;
                    yield return new WaitForSeconds(Random.Range(3f, 6f));
                }
                else
                {
                    yield return false;
                    continue;
                }
            }
        }
    }
    public void KillNpc()
    {
        DisableNpc();
        gameObject.layer = 2;
        GetComponentInChildren<Renderer>().material.color = Color.black;
    }
    private Vector3 RandomTargetPos()
    {
        float X = Random.Range(minRange, maxRange);
        float Z = Random.Range(minRange, maxRange);
        int dir = Random.Range(0, 4);
        switch (dir)
        {
            case 0:
                return transform.position + new Vector3(X, 0f, Z);
            case 1:
                return transform.position + new Vector3(X, 0f, -Z);
            case 2:
                return transform.position + new Vector3(-X, 0f, Z);
            case 3:
                return transform.position + new Vector3(-X, 0f, -Z);
            default:
                return Vector3.zero;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class KJY_NpcSpawn : MonoBehaviour
{
    public GameObject[] npcObj;
    public Vector3[] createRange;
    public float checkHeight;
    public string[] NpcString;
    private void Start()
    {
        if (PhotonNetwork.isMasterClient)
        {
            for (int i = 0; i < 2; i++)
            {
                CreateAtRandomPos();
            }
            StartCoroutine(createCor());
        }
    }

    //코르틴 알아서 바꾸도록
    IEnumerator createCor()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            CreateAtRandomPos();
        }
    }

    public void CreateNpc(int index, float x, float z)
    {
        float _y = 0.5166644f;
        if(index==9)
        {
            _y = 2.0f;
        }
        Vector3 createPos = new Vector3(x, _y, z);
        //Instantiate(npcObj[index], createPos, Quaternion.identity);
        PhotonNetwork.Instantiate(NpcString[index], createPos, Quaternion.identity,0);
    }

  public Vector3 GetRandomPos()
    {
        while (true)
        {
            float x = Random.Range(createRange[0].x, createRange[1].x);
            float z = Random.Range(createRange[0].z, createRange[1].z);
            Vector3 checkPos = new Vector3(x, checkHeight, z);
            RaycastHit hit;
            if (Physics.Raycast(checkPos, Vector3.down, out hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    return new Vector3(x, 0f, z);
                }
                else
                {
                    continue;
                }
            }
        }
    }


    void CreateAtRandomPos()
    {
        Vector3 randomPos = GetRandomPos();
        CreateNpc(Random.Range(0, npcObj.Length), randomPos.x, randomPos.z);
    }
}

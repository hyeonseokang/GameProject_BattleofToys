using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class khs_arrowI : MonoBehaviour {
    public int ArrowInt;

    public KHS_Explan explan;

    [PunRPC]
    public void DiePlayer2(PhotonPlayer _player)
    {
        if (PhotonNetwork.player != _player)
            StartCoroutine("Die");
    }

    IEnumerator Die()
    {
        GetComponent<Animator>().enabled = false;
        yield return new WaitForSeconds(0.1f);
        int rand = Random.Range(0, 2);
        int turnCount = 0;
        while (turnCount < 16)
        {
            turnCount++;
            if (rand == 0)
                transform.eulerAngles += Vector3.forward * 5f;
            else
                transform.eulerAngles -= Vector3.forward * 5f;
            yield return false;
        }
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<CapsuleCollider>().isTrigger = true;
        yield return new WaitForSeconds(4f);
        Renderer[] renderers;
        renderers = GetComponentsInChildren<Renderer>();
        float time = 0f;
        float duration = 4f;
        while (time < duration)
        {
            time += Time.fixedDeltaTime;

            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].material.HasProperty("_Color"))
                {
                    Color originColor = renderers[i].material.GetColor("_Color");
                    renderers[i].material.SetColor("_Color", Color.Lerp(originColor, Color.clear, time / duration));
                }
            }
            yield return new WaitForFixedUpdate();
        }
    }
}


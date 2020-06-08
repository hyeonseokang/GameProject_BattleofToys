using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class KHS_Explan : MonoBehaviour {
    public Image ExplanImage;


    Quaternion GetRotFromVectors(Vector2 posStart, Vector2 posend)
    {
        return Quaternion.Euler(0.0f, 0.0f, -Mathf.Atan2(posStart.x, posend.y - posStart.y) * Mathf.Rad2Deg);
    }




    public void RotationExplan(Transform _vec3)
    {
        Debug.Log("바뀌고는 있는데 ");


        transform.LookAt(transform.position + _vec3.rotation * Vector3.back,
        _vec3.rotation * new Vector3(0, 1, 0));
        transform.Rotate(new Vector3(0, 180, 0));

        //ExplanImage.rectTransform.LookAt(_vec3);
        //Debug.Log(ExplanImage.transform.rotation);
        //ExplanImage.rectTransform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        //Debug.Log("안녕난 " + _vec3.rotation);
        //ExplanImage.transform.LookAt(transform.position + _vec3.rotation * Vector3.back,
        //    _vec3.transform.rotation * Vector3.down);
        //ExplanImage.rectTransform.rotation = Quaternion.Euler(0, ExplanImage.rectTransform.rotation.y, 0);
        //Debug.Log(ExplanImage.rectTransform.rotation);
    }
}

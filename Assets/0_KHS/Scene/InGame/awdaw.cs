using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class awdaw : MonoBehaviour {
    public Canvas _image;
    public GameObject _camera;

    private void Update()
    {
        _image.transform.LookAt(_image.transform.position + _camera.transform.rotation * Vector3.back,
            _camera.transform.rotation * new Vector3(0, 1, 0));
        _image.transform.Rotate(new Vector3(0, 180, 0));
        //_image.transform.LookAt(_camera.transform.position);
    }
}

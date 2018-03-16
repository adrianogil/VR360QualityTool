using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraParams : MonoBehaviour {

    private Material material;
    private Camera cameraRender;

	// Use this for initialization
	void Start () {
		material = GetComponent<MeshRenderer>().sharedMaterial;
        cameraRender = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        material.SetVector("_CameraPosition", cameraRender.transform.position);
        material.SetVector("_UpDirection", cameraRender.transform.up);
		material.SetVector("_ForwardDirection", cameraRender.transform.forward);
        material.SetFloat("_FoV", cameraRender.fieldOfView * Mathf.Deg2Rad);
	}
}

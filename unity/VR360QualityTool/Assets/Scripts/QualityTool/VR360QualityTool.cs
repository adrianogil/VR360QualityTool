using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VR360QualityTool : MonoBehaviour {

    public float fieldOfView = 100;

    public int screenshotWidth, screenshotHeight;
    public List<Vector3> screenshotDirections;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
    #if UNITY_EDITOR
    public void GenerateScreenshots()
    {
        if (screenshotDirections == null || screenshotWidth <= 0 || screenshotHeight <= 0) return;

        GameObject cameraObject = new GameObject("Camera");
        cameraObject.transform.parent = transform;
        cameraObject.transform.localPosition = Vector3.zero;

        Camera cameraComponent = cameraObject.AddComponent<Camera>();
        cameraComponent.fieldOfView = fieldOfView;

        int index = 0;

        for (int d = 0; d < screenshotDirections.Count; d++)
        {
            Vector3 direction = screenshotDirections[d].normalized;

            cameraObject.transform.forward = direction;

            RenderTexture rt = new RenderTexture(screenshotWidth, screenshotHeight, 24);
            cameraComponent.targetTexture = rt; //Create new renderTexture and assign to camera
            Texture2D screenShot = new Texture2D(screenshotWidth, screenshotWidth, TextureFormat.RGB24, false); //Create new texture

            cameraComponent.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, screenshotWidth, screenshotHeight), 0, 0); //Apply pixels from camera onto Texture2D
            byte[] textureBytes = screenShot.EncodeToJPG();
            File.WriteAllBytes("Screenshot_" + index + ".jpg", textureBytes);

            cameraComponent.targetTexture = null;
            RenderTexture.active = null; //Clean
            DestroyImmediate(rt); //Free memory

            index++;
        }

        DestroyImmediate(cameraObject);
    }
    #endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(VR360QualityTool))]
public class VR360QualityToolEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    
        VR360QualityTool editorObj = target as VR360QualityTool;
    
        if (editorObj == null) return;

        if (GUILayout.Button("Generate screenshots"))
        {
            editorObj.GenerateScreenshots();
        }
    }

}
#endif

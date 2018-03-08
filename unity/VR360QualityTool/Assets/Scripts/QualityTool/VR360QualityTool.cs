using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum QualityMetric
{
    MSE,
    SSIM
}

public class VR360QualityTool : MonoBehaviour {


    [Header("Screenshot Settings")]
    public float fieldOfView = 100;

    public int screenshotWidth, screenshotHeight;
    public List<Vector3> screenshotDirections;

    [Header("360 Format Settings")]
    public List<Transform> imageFormatObjects;

    [HideInInspector]
    public List<QualityMetric> metrics;

    [HideInInspector]
    public bool showPlots;

    [HideInInspector]
    public bool generateReport;

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

        if (imageFormatObjects == null || imageFormatObjects.Count < 2) return;

        for (int f = 0; f < imageFormatObjects.Count; f++)
        {
            Transform currentFormatObj = imageFormatObjects[f];

            Vector3 originalPosition = currentFormatObj.position;
            currentFormatObj.position = transform.position;

            index = 0;

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
                File.WriteAllBytes("Screenshots/Screenshot_" + index + "_" + currentFormatObj.name  + ".jpg", textureBytes);

                cameraComponent.targetTexture = null;
                RenderTexture.active = null; //Clean
                DestroyImmediate(rt); //Free memory

                index++;
            }

            currentFormatObj.position = originalPosition;
        }

        DestroyImmediate(cameraObject);

        string image1 = "";
        string image2 = "";

        for (int f = 1; f < imageFormatObjects.Count; f++)
        {
            for (int d = 0; d < screenshotDirections.Count; d++)
            {
                image1 = "Screenshots/Screenshot_" + d + "_" + imageFormatObjects[0].name + ".jpg";
                image2 = "Screenshots/Screenshot_" + d + "_" + imageFormatObjects[f].name + ".jpg";
                ExecuteCommand (image1, image2);
            }
        }
    }

    public void ExecuteCommand (string image1, string image2)
    {
        Debug.Log("GilLog - VR360QualityTool::ExecuteCommand");

        string commandArguments = "../../python/qualityassessment.py " + image1 + " " + image2 + " ";

        if (metrics == null) return;

        if (showPlots)
        {
            commandArguments += "--graph ";
        } else {
            commandArguments += "--no-graph ";
        }

        if (generateReport)
        {
            commandArguments += "--report ";
        } else {
            commandArguments += "--no-report ";
        }

        for (int m = 0; m < metrics.Count; m++)
        {
            commandArguments += metrics[m].ToString() + " ";
        }

        Debug.Log("GilLog - VR360QualityTool::ExecuteCommand - commandArguments " + commandArguments + " ");

        var processInfo = new System.Diagnostics.ProcessStartInfo("python", commandArguments);
        processInfo.CreateNoWindow = true;
        processInfo.UseShellExecute = false;

        var process = System.Diagnostics.Process.Start(processInfo);

        process.WaitForExit();
        process.Close();

         // var thread = new Thread(delegate () {Command(inputVideo);});
         // thread.Start();
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

        EditorGUILayout.Space();

        EditorGUI.indentLevel++;

        EditorGUILayout.LabelField("Metrics", EditorStyles.boldLabel);

        if (editorObj.metrics != null) {
            for (int m = 0; m < editorObj.metrics.Count; m++)
            {
                editorObj.metrics[m] = (QualityMetric)EditorGUILayout.EnumPopup("Quality Metric " + m + ":",
                    editorObj.metrics[m]);
            }
        }

        if (GUILayout.Button("Add Metric"))
        {
            if (editorObj.metrics == null) {
                editorObj.metrics = new List<QualityMetric>();
            }
            editorObj.metrics.Add(QualityMetric.MSE);
        }

        EditorGUI.indentLevel--;

        if (GUILayout.Button("Generate screenshots"))
        {
            editorObj.GenerateScreenshots();
        }
    }

}
#endif

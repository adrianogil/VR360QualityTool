using UnityEngine;

using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CubemapTextureGeneration : MonoBehaviour
{
    public int cubemapFacePixelSize;

    public void GenerateCubemap()
    {
        float cubeSize = 1;

        // float fieldOfView = 2f * Mathf.Atan2(cubeSize, cubeSize);
        float fieldOfView = 90f;

        GameObject cameraObject = new GameObject("Camera");
        cameraObject.transform.parent = transform;
        cameraObject.transform.localPosition = Vector3.zero;

        Camera cameraComponent = cameraObject.AddComponent<Camera>();
        cameraComponent.fieldOfView = fieldOfView;

        Vector3[] directions = new Vector3[]
        {
            Vector3.forward,
            Vector3.up,
            Vector3.right,
            (-1f) * Vector3.forward,
            (-1f) * Vector3.up,
            (-1f) * Vector3.right,
        };

        for (int i = 0; i < 6; i++)
        {
            cameraObject.transform.forward = directions[i];

            RenderTexture rt = new RenderTexture(cubemapFacePixelSize, cubemapFacePixelSize, 24);
            cameraComponent.targetTexture = rt; //Create new renderTexture and assign to camera
            Texture2D screenShot = new Texture2D(cubemapFacePixelSize, cubemapFacePixelSize, TextureFormat.RGB24, false); //Create new texture

            cameraComponent.Render();

            RenderTexture.active = rt;
            screenShot.ReadPixels(new Rect(0, 0, cubemapFacePixelSize, cubemapFacePixelSize), 0, 0); //Apply pixels from camera onto Texture2D
            byte[] textureBytes = screenShot.EncodeToJPG();
            File.WriteAllBytes("Cubemap/Cubemap_Face_" + i + ".jpg", textureBytes);

            cameraComponent.targetTexture = null;
            RenderTexture.active = null; //Clean
            DestroyImmediate(rt); //Free memory
        }
        

        DestroyImmediate(cameraObject);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(CubemapTextureGeneration))]
public class CubemapTextureGenerationEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    
        CubemapTextureGeneration editorObj = target as CubemapTextureGeneration;
    
        if (editorObj == null) return;
        
        if (GUILayout.Button("Generate Cubemap"))
        {
            editorObj.GenerateCubemap();
        }
    }

}
#endif
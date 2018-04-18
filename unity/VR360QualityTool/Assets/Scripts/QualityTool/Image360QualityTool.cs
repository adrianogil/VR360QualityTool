using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public enum ImageType
{
    Equirectangular,
    Cubemap
}

public enum ViewerType
{
    ProceduralSphere,
    ProceduralCubemap
}

[System.Serializable]
public class ImageData
{
    public ImageType imageType;
    public String path;
    public int resolutionX, resolutionY;
}

[System.Serializable]
public class ViewerData
{
    public ViewerType viewerType;
    public int totalLatitude;
    public int totalLongitude;
    public int totalSubdivision;
}

public class Image360QualityTool : MonoBehaviour {

    [Header("Screenshot Settings")]
    public float fieldOfView = 100;

    public int screenshotWidth, screenshotHeight;
    public List<Vector3> screenshotDirections;

    [HideInInspector]
    public ImageData imageData;

    [HideInInspector]
    public ViewerData viewerData;

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
}

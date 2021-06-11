using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class CameraSensor : MonoBehaviour, ISensor
{
    public Camera sensorCam;
	public int width = 256;
	public int height = 256;
	Texture2D tex;
	RenderTexture ren;
    Rect ImageRect;

    void Awake()
	{
		tex = new Texture2D(width, height, TextureFormat.RGB24, false);
		ren = new RenderTexture(width, height, 16, RenderTextureFormat.ARGB32);
		sensorCam.targetTexture = ren;
	}

    void UpdateResolution(int w, int h)
    {
        width = w;
        height = h;
        ImageRect = new Rect(0, 0, width, height);
    }

	Texture2D RTImage() 
	{
		var currentRT = RenderTexture.active;
		RenderTexture.active = sensorCam.targetTexture;

		sensorCam.Render();

		tex.ReadPixels(ImageRect, 0, 0);
		tex.Apply();
		RenderTexture.active = currentRT;

		return tex;
	}

	public byte[] GetImageBytes()
	{
		return RTImage().EncodeToPNG();
	}

    public JSONObject RequestObs(JSONObject json)
    {
        json.AddField(transform.name, Convert.ToBase64String(GetImageBytes()));
        return json;
    }
}

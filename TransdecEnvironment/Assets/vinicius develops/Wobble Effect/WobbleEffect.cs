using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class needs to be attached to the camera in order to receive OnRenderImage message
[ExecuteInEditMode]
public class WobbleEffect : MonoBehaviour
{
    public Material material;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (material != null && enabled)
        {
            Graphics.Blit(source, destination, material);
        } else
        {
            Graphics.Blit(source, destination);
        }
    }
}

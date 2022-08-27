using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFX : MonoBehaviour
{
    public Material material;

    private void Awake()
    {
        material = new Material(Shader.Find("Proyecto/Sweep"));
     
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit (source, destination, material);
    }
}

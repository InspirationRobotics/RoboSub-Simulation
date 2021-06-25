using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class DrawSonar : MonoBehaviour
{
    public SonarSensor[] sonarSensors;
    public GameObject subObj;
    static Material lineMaterial;

    void Awake()
    {
        RenderPipelineManager.endCameraRendering += OnEndCameraRendering;
    }

    void OnDestroy()
    {
        RenderPipelineManager.endCameraRendering -= OnEndCameraRendering;
    }

    void OnEndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        if (subObj == null) { return; }
        if (sonarSensors == null)
        {
            sonarSensors = subObj.GetComponentsInChildren<SonarSensor>();
            return;
        }

        Draw();
    }

    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    public void Draw()
    {
        CreateLineMaterial();
        lineMaterial.SetPass(0); // Apply the line material

        GL.Begin(GL.LINES); // Draw lines

        foreach (SonarSensor sonarSensor in sonarSensors)
        {
            if (sonarSensor.point != Vector3.zero)
            {
                Vector3 sensorPos = sonarSensor.transform.position;
                Vector3 point = sonarSensor.point;

                // I don't know why but HDRP doens't seeps to support GL drawing
                GL.Color(new Color(1, 0, 0, 0.6F));

                GL.Vertex3(sensorPos.x, sensorPos.y, sensorPos.z);
                GL.Vertex3(point.x, point.y, point.z);

                Debug.DrawLine(sensorPos, point, new Color(1, 0, 0, 0.6f));
            }
        }
        GL.End();
    }
}

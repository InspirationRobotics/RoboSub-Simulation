using UnityEngine;

public class CameraSetup : MonoBehaviour
{
    private Transform cameraHeadTransform;
    private GameObject monoCamera;
    private GameObject leftEyeCamera;
    private GameObject rightEyeCamera;

    public float EyeSeparation = 0.08f;
    public float NearClipPlane = 0.01f;
    public float FarClipPlane = 100.0f;
    public float FieldOfView = 60.0f;

    private bool isStereo;

    void Start()
    {
        // Get the transform of the CameraHead gameobject
        cameraHeadTransform = gameObject.transform;

        // create mono camera
        monoCamera = new GameObject("monoCamera");
        monoCamera.transform.SetParent(cameraHeadTransform.transform);
        var camera = cameraHeadTransform.gameObject.AddComponent<Camera>();
        camera.nearClipPlane = NearClipPlane;
        camera.farClipPlane = FarClipPlane;

        // create stereo camera setup
        leftEyeCamera = new GameObject("leftEyeCamera");
        leftEyeCamera.transform.SetParent(cameraHeadTransform.transform);
        leftEyeCamera.transform.localPosition -= new Vector3(0.04f, 0, 0);
        var cameraLE = leftEyeCamera.AddComponent<Camera>();
        cameraLE.rect = new Rect(0, 0, 0.5f, 1);
        cameraLE.fieldOfView = FieldOfView;
        cameraLE.aspect *= 2;
        cameraLE.nearClipPlane = NearClipPlane;
        cameraLE.farClipPlane = FarClipPlane;
        leftEyeCamera.SetActive(false);

        rightEyeCamera = new GameObject("rightEyeCamera");
        rightEyeCamera.transform.SetParent(cameraHeadTransform.transform);
        rightEyeCamera.transform.localPosition += new Vector3(0.04f, 0, 0);
        var cameraRE = rightEyeCamera.AddComponent<Camera>();
        cameraRE.rect = new Rect(0.5f, 0, 0.5f, 1);
        cameraRE.fieldOfView = FieldOfView;
        cameraRE.aspect *= 2;
        cameraRE.nearClipPlane = NearClipPlane;
        cameraRE.farClipPlane = FarClipPlane;
        rightEyeCamera.SetActive(false);

        isStereo = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isStereo = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            isStereo = true;
        }

        monoCamera.SetActive(!isStereo);
        leftEyeCamera.SetActive(isStereo);
        rightEyeCamera.SetActive(isStereo);
    }
}
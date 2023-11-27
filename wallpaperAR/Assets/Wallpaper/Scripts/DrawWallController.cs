using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

public class DrawWallController : MonoBehaviour
{

    public GameObject WallPrefab;

    private GameObject WallInstance;
    private WallController wallcontroller;

    private Vector2 newTiling;

    private Vector3 point1 = Vector3.zero;
    private Vector3 point2 = Vector3.zero;

    public float rotationScaleFactor = 0.5f;
    public float maxScale = 10f;

    private ARCameraManager arCameraManager;


    private float IntialWallHeight = 0.1f;
    private float WallHeight;
    public float LastAngle { get; private set; }
    public float IntialAngle { get; private set; }

    public float metterheight;

    public TextMeshPro HeightDisplayPrefab;
    public Transform HeightMeterPos;

    ARSessionOrigin ARSession;

    private void Start()
    {
        arCameraManager = FindObjectOfType<ARCameraManager>();
        ARSession = ARController.Instance.arSessionOrigin;
    }


    public void Setpoints(Vector3 pointone, Vector3 pointtwo)
    {
        point1 = pointone;
        point2 = pointtwo;

    }

    private void Update()
    {
        if (point1 == Vector3.zero || point2 == Vector3.zero || WallPrefab == null)
        {
            return;
        }

        // Calculate midpoint between points
        Vector3 midpoint = (point1 + point2) / 2;

        if (WallInstance == null)
        {
            // Instantiate plane prefab at midpoint if not already instantiated
            WallInstance = Instantiate(WallPrefab, midpoint, Quaternion.identity);
            wallcontroller = WallInstance.GetComponent<WallController>();
            WallInstance.SetActive(false);
            LastAngle = ARController.Instance.ConvertUnityAngle(Camera.main.transform.eulerAngles.x);
            IntialAngle = LastAngle;
            WallInstance.transform.localScale = new Vector3(1,0.01f,IntialWallHeight);
        }
        else
        {
            // Update plane position to be below midpoint
            WallInstance.transform.position = midpoint;

            // Calculate rotation to face the direction between the points
            Vector3 direction = point2 - point1;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.back);

            // Apply rotation to the plane

            WallInstance.transform.rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            WallInstance.transform.eulerAngles = new Vector3(WallInstance.transform.eulerAngles.x + 90, WallInstance.transform.eulerAngles.y, WallInstance.transform.eulerAngles.z);

            // Calculate scale along x-axis to fit exactly between points
            float distance = Vector3.Distance(point1, point2);

            // Adjust plane scale to fit between points
            Vector3 planeScale = WallInstance.transform.localScale;
            planeScale.x = distance;
            newTiling.x = distance / 2;
            WallInstance.transform.localScale = planeScale;

            SetWallSize();

            Bounds localBounds = WallInstance.GetComponent<MeshRenderer>().bounds;
            Bounds worldBounds = new Bounds(ARSession.transform.TransformPoint(localBounds.center), ARSession.transform.TransformVector(localBounds.size));
            metterheight = worldBounds.size.y;

            if (HeightDisplayPrefab == null)
            {
                HeightDisplayPrefab = Instantiate(ARController.Instance.distanceTextPrefab,Vector3.zero, Quaternion.identity);
            }

            //Vector3 parentSize = WallInstance.GetComponent<MeshRenderer>().bounds.size;
            //float offsetX = parentSize.x / 2f;
            //float offsetY = parentSize.y / 2f;

            //Vector3 displayPosition = new Vector3(WallInstance.transform.position.x - offsetX, WallInstance.transform.position.y + offsetY + 0.1f, WallInstance.transform.position.z);
            HeightDisplayPrefab.transform.position = wallcontroller.HeightMeterPos.position;

            Vector3 lookDirection = ARController.Instance.arCamera.transform.position - HeightDisplayPrefab.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            Quaternion finalRotation = lookRotation * Quaternion.Euler(0f, 180f, 0f);
            HeightDisplayPrefab.transform.rotation = finalRotation;
            HeightDisplayPrefab.text = $"{metterheight:F2}m";


        }



    }

    public void SetWallSize()
    {
        if (arCameraManager == null || !arCameraManager.enabled)
            return;


        float Angle = ARController.Instance.ConvertUnityAngle(Camera.main.transform.eulerAngles.x);

        Vector3 scale = WallInstance.transform.localScale;

        float ditance =  LastAngle - Angle;

        // WallHeight = 1 + (0.03f * ditance);

        WallHeight = Mathf.Clamp(0.5f + (0.05f * ditance), 0.1f, 8f);

        ScalePlaneZ(WallHeight);
    }

    public void ScalePlaneZ(float value)
    {

        Vector3 scale = WallInstance.transform.localScale;

        if (value < IntialWallHeight)
            value = IntialWallHeight;

        float distanceToCube = Vector3.Distance(Camera.main.transform.position, WallInstance.transform.position);

        float distanceFactor = Mathf.InverseLerp(1, 5, distanceToCube);

        value = Mathf.Lerp(value, value * distanceFactor, 0.5f);

        scale.z = value;// Mathf.Lerp(scale.z, newscale, lerpFactor);


        WallInstance.transform.localScale = scale;

        Vector3 pivotOffset = Vector3.up * (scale.z / 2);
        WallInstance.transform.position += pivotOffset;

        Renderer renderer = WallInstance.GetComponent<Renderer>();
        Material material = renderer.material;

        Vector2 newTiling = material.mainTextureScale;
        newTiling.y = scale.z / 2f;
        material.mainTextureScale = newTiling;

        WallInstance.SetActive(true);

    }


}

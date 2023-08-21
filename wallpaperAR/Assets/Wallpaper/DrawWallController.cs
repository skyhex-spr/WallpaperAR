using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.XR.ARFoundation;

public class DrawWallController : MonoBehaviour
{

    public GameObject WallPrefab;

    private GameObject planeInstance;

    public float scaleval;
    private Vector2 newTiling;

    private Vector3 point1 = Vector3.zero;
    private Vector3 point2 = Vector3.zero;

    public Transform objectToScale;
    public float rotationScaleFactor = 0.5f;
    public float maxScale = 10f;

    private ARCameraManager arCameraManager;


    private float IntialWallHeight = 0.1f;
    private float WallHeight;
    public float LastAngle { get; private set; }
    public float IntialAngle { get; private set; }

    // Wall height Lerp 
    public float lerpDuration = 30.0f;
    private float currentTime = 0.0f;

    private void Start()
    {
        arCameraManager = FindObjectOfType<ARCameraManager>();
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

        if (planeInstance == null)
        {
            // Instantiate plane prefab at midpoint if not already instantiated
            planeInstance = Instantiate(WallPrefab, midpoint, Quaternion.identity);
            planeInstance.SetActive(false);
            LastAngle = ARController.Instance.ConvertUnityAngle(Camera.main.transform.eulerAngles.x);
            IntialAngle = LastAngle;
            planeInstance.transform.localScale = new Vector3(1,0.01f,IntialWallHeight);
        }
        else
        {
            // Update plane position to be below midpoint
            planeInstance.transform.position = midpoint;

            // Calculate rotation to face the direction between the points
            Vector3 direction = point2 - point1;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.back);

            // Apply rotation to the plane

            planeInstance.transform.rotation = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
            planeInstance.transform.eulerAngles = new Vector3(planeInstance.transform.eulerAngles.x + 90, planeInstance.transform.eulerAngles.y, planeInstance.transform.eulerAngles.z);

            // Calculate scale along x-axis to fit exactly between points
            float distance = Vector3.Distance(point1, point2);

            // Adjust plane scale to fit between points
            Vector3 planeScale = planeInstance.transform.localScale;
            planeScale.x = distance;
            newTiling.x = distance / 2;
            planeInstance.transform.localScale = planeScale;

            SetWallSize();
        }



    }

    public void SetWallSize()
    {
        if (arCameraManager == null || !arCameraManager.enabled)
            return;


        float Angle = ARController.Instance.ConvertUnityAngle(Camera.main.transform.eulerAngles.x);

        Vector3 scale = planeInstance.transform.localScale;

        float ditance = Mathf.Abs(Angle - LastAngle);

        if (Mathf.Abs((Angle - IntialAngle)) < 2)
        {
            LastAngle = IntialAngle;
            WallHeight = IntialWallHeight;
            ARController.Instance.deb.text = "Near To Intial";
        }
        else
        {
            if (ditance >= 1f)
            {
                if(Angle < LastAngle)
                    WallHeight = WallHeight + (0.03f * ditance);
                else
                    WallHeight = WallHeight - (0.03f * ditance);

                LastAngle = Angle;
                ARController.Instance.deb.text = LastAngle.ToString();
            }
        }

        ScalePlaneZ(WallHeight);
    }

    public void ScalePlaneZ(float value)
    {

        Vector3 scale = planeInstance.transform.localScale;

        if (value < IntialWallHeight)
            value = IntialWallHeight;

        currentTime += Time.deltaTime;
        float lerpFactor = Mathf.Clamp01(currentTime / lerpDuration);

        float newscale = Mathf.Lerp(IntialWallHeight, 90f, value / 90f);
        scale.z = Mathf.Lerp(scale.z, newscale, lerpFactor);

        if (lerpFactor >= 1.0f)
        {
            currentTime = 0.0f;
        }

        planeInstance.transform.localScale = scale;

        Vector3 pivotOffset = Vector3.up * (scale.z / 2);
        planeInstance.transform.position += pivotOffset;

        Renderer renderer = planeInstance.GetComponent<Renderer>();
        Material material = renderer.material;

        Vector2 newTiling = material.mainTextureScale;
        newTiling.y = scale.z / 2f;
        material.mainTextureScale = newTiling;

        planeInstance.SetActive(true);

    }


}

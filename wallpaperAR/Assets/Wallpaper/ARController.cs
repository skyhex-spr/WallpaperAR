using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.UI;

public class ARController : MonoBehaviour
{

    public DrawWallController DrawWallControlker;

    public GameObject PointPrefab;
    public GameObject previewObject;
    public TextMeshPro distanceTextPrefab; 
    public Camera arCamera;
    public ARSessionOrigin arSessionOrigin;

    public GameObject PlaneInfinity;

    public GameObject LinePrefab;
    
    
    public LayerMask mask;
    public Text deb;


    private GameObject LineInstance;
    private ARRaycastManager raycastManager;
    private bool isPlacingObject = false;
    private Vector3 firstPoint = Vector3.zero;
    private Vector3 SecondPoint = Vector3.zero;
    private GameObject distanceTextObject;

    private bool initialPlane = true;

    private List<Transform> points;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        points = new List<Transform>();
    }

    private void Update()
    {
        if (initialPlane)
        {
            PlacePreviewObject();
        }
        else
        {
            placePReViewObjectOnPlan();
        }
        

        if (Input.touchCount > 0)
        {

            if (SecondPoint != Vector3.zero) return;

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                if (firstPoint == Vector3.zero)
                {
                    firstPoint = previewObject.transform.position;
                    PlaceObject();
                }
                else
                {
                    SecondPoint = previewObject.transform.position;
                    PlaceObject();
                }
            }
        }

        UpdateDistanceText();
    }

    private void PlacePreviewObject()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            previewObject.SetActive(true);
            previewObject.transform.position = hitPose.position;
            previewObject.transform.rotation = hitPose.rotation;

            PlaneInfinity.SetActive(true);
            PlaneInfinity.transform.position = hitPose.position;
            PlaneInfinity.transform.rotation = hitPose.rotation;

            initialPlane = false;
        }
    }

    private void placePReViewObjectOnPlan()
    {
        RaycastHit hit;
        if (Physics.Raycast(arCamera.transform.position, arCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, mask))
        {
            previewObject.SetActive(true);
           // previewObject.transform.position = hit.point;

            previewObject.transform.position = Vector3.Lerp(previewObject.transform.position, hit.point, Time.deltaTime * 8);
            deb.text = "HIT" + hit.transform.gameObject.name;
        }
        else
        {
            Debug.DrawRay(arCamera.transform.position, arCamera.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
            deb.text = "NOT HIT";
        }
    }

    private void PlaceObject()
    {
            Instantiate(PointPrefab, previewObject.transform.position, previewObject.transform.rotation);
            previewObject.SetActive(false);
            points.Add(previewObject.transform);
    }

    private void UpdateDistanceText()
    {
        if (firstPoint != Vector3.zero && distanceTextPrefab != null)
        {
            Vector3 currentSecondPoint = SecondPoint == Vector3.zero ? previewObject.transform.position : SecondPoint;

            Vector3 worldPosition1 = arSessionOrigin.transform.TransformPoint(firstPoint);
            Vector3 worldPosition2 = arSessionOrigin.transform.TransformPoint(currentSecondPoint);

            float distance = Vector3.Distance(worldPosition1, worldPosition2);


            if (distanceTextObject == null)
            {
                distanceTextObject = Instantiate(distanceTextPrefab.gameObject, Vector3.zero, Quaternion.identity);
            }

            Vector3 lookDirection = arCamera.transform.position - distanceTextObject.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            Quaternion finalRotation = lookRotation * Quaternion.Euler(0f, 180f, 0f);
            distanceTextObject.transform.rotation = finalRotation;


            Vector3 midpoint = (firstPoint + currentSecondPoint) / 2f;

            if (SecondPoint == Vector3.zero)
              distanceTextObject.transform.position = new Vector3(previewObject.transform.position.x, previewObject.transform.position.y + 0.1f, previewObject.transform.position.z);
            else
              distanceTextObject.transform.position = new Vector3(midpoint.x, midpoint.y + 0.1f, midpoint.z); ;

            TextMeshPro tmpText = distanceTextObject.GetComponent<TextMeshPro>();
            tmpText.text = $"{distance:F2}m";

            // LINE
            if (LineInstance == null)
            {
                LineInstance = Instantiate(LinePrefab, midpoint, Quaternion.identity);
            }
            else
            {
                LineInstance.transform.position = midpoint;

                Vector3 forward = currentSecondPoint - firstPoint;
                Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);

                LineInstance.transform.rotation = rotation * Quaternion.Euler(90f, 0f, 0f);

                float Linedistance = Vector3.Distance(firstPoint, currentSecondPoint);
                LineInstance.transform.localScale = new Vector3(LineInstance.transform.localScale.x, Linedistance / 2, LineInstance.transform.localScale.z);
            }

            //LINE
            if(points.Count == 2)
             DrawWallControlker.Setpoints(firstPoint,currentSecondPoint);


        }
        else if (distanceTextObject != null)
        {
            Destroy(distanceTextObject);
        }
    }
}

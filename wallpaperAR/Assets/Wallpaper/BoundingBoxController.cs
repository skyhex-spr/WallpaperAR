using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections.Generic;
using UnityEngine.XR.ARSubsystems;
using TMPro;

public class BoundingBoxController : MonoBehaviour
{
    public GameObject cylinderPrefab;
    public GameObject previewObject;
    public TextMeshPro distanceTextPrefab; // 3D text prefab for distance display
    public Camera arCamera;

    private ARRaycastManager raycastManager;
    public ARSessionOrigin arSessionOrigin;
    private bool isPlacingObject = false;
    private Vector3 firstPoint = Vector3.zero;
    private Vector3 SecondPoint = Vector3.zero;
    private GameObject distanceTextObject;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {

        PlacePreviewObject();
        

        if (Input.touchCount > 0)
        {

            if (SecondPoint != Vector3.zero) return;

            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began && isPlacingObject)
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
            isPlacingObject = true;
        }
    }

    private void PlaceObject()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            Instantiate(cylinderPrefab, hitPose.position, hitPose.rotation);
            previewObject.SetActive(false);
            isPlacingObject = false;
        }
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

            // Apply an additional rotation of 180 degrees around the y-axis
            Quaternion finalRotation = lookRotation * Quaternion.Euler(0f, 180f, 0f);
            distanceTextObject.transform.rotation = finalRotation;


            Vector3 midpoint = (firstPoint + currentSecondPoint) / 2f;
            distanceTextObject.transform.position = midpoint;

            TextMeshPro tmpText = distanceTextObject.GetComponent<TextMeshPro>();
            tmpText.text = $"{distance:F2}meter"; // Display the calculated real-world distance
        }
        else if (distanceTextObject != null)
        {
            Destroy(distanceTextObject);
        }
    }
}

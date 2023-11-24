using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARControllerGround : MonoBehaviour
{
    public static ARControllerGround Instance;

   
    public Camera arCamera;
    public ARSessionOrigin arSessionOrigin;
    public GameObject previewObject;
    public GameObject PlaneInfinity;

    public LayerMask mask; 
    private ARRaycastManager raycastManager;

    private bool initialPlane = true;

    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        Instance = this;
    }

    private void Start()
    {
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

 
    }

    private void PlacePreviewObject()
    {
        Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(screenCenter, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

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
        }
        else
        {
            Debug.DrawRay(arCamera.transform.position, arCamera.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }

    public float ConvertUnityAngle(float unityAngle)
    {
        // Convert the angle to a representation where angles after 0 show up as -1
        float convertedAngle = unityAngle % 360f;
        if (convertedAngle > 180f)
        {
            convertedAngle -= 360f;
        }
        return convertedAngle;
    }
}

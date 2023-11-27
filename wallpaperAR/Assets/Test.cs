using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Test : MonoBehaviour
{
    ARSessionOrigin ARSession;
    TextMeshPro HeightDisplayPrefab;
    float metterheight;
    public Transform HeightMeterPos;
    // Start is called before the first frame update
    void Start()
    {
        ARSession = ARController.Instance.arSessionOrigin;
        HeightDisplayPrefab = Instantiate(ARController.Instance.distanceTextPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Bounds localBounds = GetComponent<MeshRenderer>().bounds;
        //Bounds worldBounds = new Bounds(ARSession.transform.TransformPoint(localBounds.center), ARSession.transform.TransformVector(localBounds.size));
        //metterheight = worldBounds.size.y;

        //Vector3 textPosition = localBounds.center + new Vector3(localBounds.extents.x, localBounds.extents.y, 0f);

        //HeightDisplayPrefab.transform.position = new Vector3(textPosition.x, textPosition.y + 0.05f, textPosition.z);

        //Vector3 lookDirection = ARController.Instance.arCamera.transform.position - HeightDisplayPrefab.transform.position;
        //Quaternion lookRotation = Quaternion.LookRotation(lookDirection, Vector3.up);

        //Quaternion finalRotation = lookRotation * Quaternion.Euler(0f, 180f, 0f);
        ////HeightDisplayPrefab.transform.rotation = finalRotation;
        //HeightDisplayPrefab.text = $"{metterheight:F2}m";

        // Get the size of the parent GameObject (assuming it has a Renderer component)
        Vector3 parentSize = GetComponent<Renderer>().bounds.size;

        // Calculate the position for the child GameObject to be in the top-left corner
        float offsetX = parentSize.x / 2f;
        float offsetY = parentSize.y / 2f;
        Vector3 childPosition = new Vector3(transform.position.x - offsetX, transform.position.y + offsetY + 0.1f, transform.position.z);

        HeightDisplayPrefab.transform.position = HeightMeterPos.position;

        // Set the position of the child GameObject
        HeightDisplayPrefab.transform.position = childPosition;
    }
}

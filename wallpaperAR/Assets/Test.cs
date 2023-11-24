using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class Test : MonoBehaviour
{
    ARSessionOrigin ARSession;
    TextMeshPro HeightDisplayPrefab;
    // Start is called before the first frame update
    void Start()
    {
        ARSession = ARController.Instance.arSessionOrigin;
        HeightDisplayPrefab = Instantiate(ARController.Instance.distanceTextPrefab, Vector3.zero, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        Bounds localBounds = GetComponent<MeshRenderer>().bounds;
        Bounds worldBounds = new Bounds(ARSession.transform.TransformPoint(localBounds.center), ARSession.transform.TransformVector(localBounds.size));
        float metterheight = worldBounds.size.y;
        Debug.Log(metterheight);

        Vector3 textPosition = localBounds.center - new Vector3(localBounds.extents.x, 0f, 0f);
        HeightDisplayPrefab.transform.position = textPosition;
    }
}

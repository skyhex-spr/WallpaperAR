using UnityEngine;

public class ConnectPointsWithPlane : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public GameObject planePrefab;

    private GameObject planeInstance;

    public float scaleval;

    private Vector2 newTiling;

    private void Update()
    {
        if (point1 == null || point2 == null || planePrefab == null)
        {
            Debug.LogError("Please assign the required references in the inspector.");
            return;
        }

        // Calculate midpoint between points
        Vector3 midpoint = (point1.position + point2.position) / 2;

        if (planeInstance == null)
        {
            // Instantiate plane prefab at midpoint if not already instantiated
            planeInstance = Instantiate(planePrefab, midpoint, Quaternion.identity);
        }
        else
        {
            // Update plane position to be below midpoint
            planeInstance.transform.position = new Vector3(midpoint.x, planeInstance.transform.position.y, midpoint.z);

            // Calculate rotation to face the direction between the points
            Vector3 direction = point2.position - point1.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.back);

            // Apply rotation to the plane
            planeInstance.transform.rotation = new Quaternion(rotation.x, rotation.y , rotation.z , rotation.w);
            planeInstance.transform.eulerAngles = new Vector3(planeInstance.transform.eulerAngles.x + 90, planeInstance.transform.eulerAngles.y, planeInstance.transform.eulerAngles.z);

            // Calculate scale along x-axis to fit exactly between points
            float distance = Vector3.Distance(point1.position, point2.position);

            // Adjust plane scale to fit between points
            Vector3 planeScale = planeInstance.transform.localScale;
            planeScale.x = distance ;  
            newTiling.x = distance / 2;
            planeInstance.transform.localScale = planeScale;
        }

        ScalePlaneZ(scaleval);
    }
    public void ScalePlaneZ(float value)
    {
        if (planeInstance != null)
        {
            Vector3 scale = planeInstance.transform.localScale;
            scale.z = Mathf.Lerp(0.1f, 100, value / 100); 
            planeInstance.transform.localScale = scale;
            planeInstance.transform.position = new Vector3(planeInstance.transform.position.x, scale.z / 2 , planeInstance.transform.position.z);

            Renderer renderer = planeInstance.GetComponent<Renderer>();
            Material material = renderer.material;
            newTiling.y = scale.z / 2;
            material.mainTextureScale = newTiling;
        }
    }
}

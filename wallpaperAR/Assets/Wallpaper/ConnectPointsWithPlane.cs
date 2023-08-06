using UnityEngine;

public class ConnectPointsWithPlane : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public GameObject planePrefab;

    private GameObject planeInstance;

    public float scaleval;

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
            planeInstance.transform.rotation = rotation;

            // Calculate scale along x-axis to fit exactly between points
            float distance = Vector3.Distance(point1.position, point2.position);

            // Adjust plane scale to fit between points
            Vector3 planeScale = planeInstance.transform.localScale;
            planeScale.z = distance ;  // Divide by 2 to account for both sides of the plane
            planeInstance.transform.localScale = planeScale;
        }

        ScalePlaneX(scaleval);
    }
    public void ScalePlaneX(float value)
    {
        if (planeInstance != null)
        {
            Vector3 scale = planeInstance.transform.localScale;
            scale.x = Mathf.Lerp(0.3f, 100, value / 100); // Lerp between 0.5 and 1 based on the input value
            planeInstance.transform.localScale = scale;
            planeInstance.transform.position = new Vector3(planeInstance.transform.position.x, scale.x / 2 , planeInstance.transform.position.z);
        }
    }
}

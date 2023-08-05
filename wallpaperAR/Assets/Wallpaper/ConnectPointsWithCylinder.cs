using UnityEngine;

public class ConnectPointsWithCylinder : MonoBehaviour
{
    public Transform point1;
    public Transform point2;
    public GameObject cylinderPrefab;

    private GameObject cylinderInstance;

    private void Update()
    {
        if (point1 == null || point2 == null || cylinderPrefab == null)
        {
            Debug.LogError("Please assign the required references in the inspector.");
            return;
        }

        // Calculate midpoint
        Vector3 midpoint = (point1.position + point2.position) / 2;

        if (cylinderInstance == null)
        {
            // Instantiate cylinder prefab at midpoint if not already instantiated
            cylinderInstance = Instantiate(cylinderPrefab, midpoint, Quaternion.identity);
        }
        else
        {
            // Update cylinder position
            cylinderInstance.transform.position = midpoint;

            // Calculate rotation to connect points
            Vector3 forward = point2.position - point1.position;
            Quaternion rotation = Quaternion.LookRotation(forward, Vector3.up);

            // Set rotation to connect points and rotate around x-axis by 90 degrees
            cylinderInstance.transform.rotation = rotation * Quaternion.Euler(90f, 0f, 0f);

            // Calculate scale along the Y-axis
            float distance = Vector3.Distance(point1.position, point2.position);
            cylinderInstance.transform.localScale = new Vector3(cylinderInstance.transform.localScale.x, distance / 2, cylinderInstance.transform.localScale.z);
        }
    }
}

using UnityEngine;

public class DrawWallController : MonoBehaviour
{

    public GameObject WallPrefab;

    private GameObject planeInstance;

    public float scaleval;
    private Vector2 newTiling;

    private Vector3 point1 = Vector3.zero;
    private Vector3 point2 = Vector3.zero;

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
        }
        else
        {
            // Update plane position to be below midpoint
            planeInstance.transform.position = midpoint;

            // Calculate rotation to face the direction between the points
            Vector3 direction = point2 - point1;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.back);

            // Apply rotation to the plane
          
            planeInstance.transform.rotation = new Quaternion(rotation.x, rotation.y , rotation.z , rotation.w);
            planeInstance.transform.eulerAngles = new Vector3(planeInstance.transform.eulerAngles.x + 90, planeInstance.transform.eulerAngles.y, planeInstance.transform.eulerAngles.z);

            // Calculate scale along x-axis to fit exactly between points
            float distance = Vector3.Distance(point1, point2);

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
            scale.z = Mathf.Lerp(0.1f, 100f, value / 100f);
            planeInstance.transform.localScale = scale;

            Vector3 pivotOffset = Vector3.up * (scale.z / 2);
            planeInstance.transform.position += pivotOffset;

            Renderer renderer = planeInstance.GetComponent<Renderer>();
            Material material = renderer.material;

            Vector2 newTiling = material.mainTextureScale;
            newTiling.y = scale.z / 2f;
            material.mainTextureScale = newTiling;
        }
    }

  
}

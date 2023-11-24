using UnityEngine;

public class TilingAdjuster : MonoBehaviour
{
    private Material material;
    private Vector3 previousScale;

    void Start()
    {
        // Assuming the material is assigned to the renderer of the object
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
            previousScale = transform.localScale;
            UpdateTiling();
        }
        else
        {
            Debug.LogError("Renderer not found on the object.");
        }
    }

    void Update()
    {
        // Check if the scale has changed
        if (transform.localScale != previousScale)
        {
            UpdateTiling();
            previousScale = transform.localScale;
        }
    }

    void UpdateTiling()
    {
        // Adjust the tiling based on the inverse of the scale
        float xTiling = 6f * transform.localScale.x;
        float zTiling = 6f * transform.localScale.z;

        material.mainTextureScale = new Vector2(xTiling, zTiling);
    }
}

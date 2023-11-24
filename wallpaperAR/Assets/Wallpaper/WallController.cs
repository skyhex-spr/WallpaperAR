using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallController : MonoBehaviour
{
    [HideInInspector]
    public Renderer renderer;
    private Material material;
    private Vector3 previousScale;

    public bool IsSquare = true;
    public int TileX = 2;
    public int TileY = 2;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<MeshRenderer>().material.renderQueue = 3002;

        renderer = GetComponent<Renderer>();
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

    // Update is called once per frame
    void Update()
    {
        SetTile();
    }

    void SetTile()
    {
        if (transform.localScale != previousScale)
        {
            UpdateTiling();
            previousScale = transform.localScale;
        }
    }

    void UpdateTiling()
    {
        // Adjust the tiling based on the inverse of the scale
        float xTiling = TileX * (transform.localScale.x);

        float zTiling = 0;
        if (!IsSquare)
          zTiling = TileY * (transform.localScale.z + 1);
        else
          zTiling = TileY * (transform.localScale.z);


        material.mainTextureScale = new Vector2(xTiling, zTiling);
    }

    public void ChangeMaterial(Material mat)
    {
        GetComponent<Renderer>().material = mat;
        SetTile();
    }
}

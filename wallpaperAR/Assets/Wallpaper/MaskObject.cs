using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskObject : MonoBehaviour
{

    public List<GameObject> Maskobjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in Maskobjects)
        {
            item.GetComponent<MeshRenderer>().material.renderQueue = 3002;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

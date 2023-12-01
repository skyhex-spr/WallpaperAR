using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public SettingDefination setting;
    public StonesDefinition stones;

    public ARController Wallcontroller;
    public ARControllerGround WallcontrollerGround;

    public ARPlaneManager planemanager;

    // Start is called before the first frame update
    void Start()
    {
         planemanager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;

        if (setting.serface == Surface.Wall)
        {
            Wallcontroller.enabled = true;
        }
        else
        {
            WallcontrollerGround.enabled= true;
        }
    }

    Material mat;
    public void Switchmat()
    {
        WallController wall = FindObjectOfType<WallController>();
        mat = wall.renderer.material;

        string matname = mat.name.Replace(" (Instance)","");


        if (matname == stones.stones[0].Material.name)
            mat = stones.stones[1].Material;
        else
            mat = stones.stones[0].Material;

        wall.ChangeMaterial(mat);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

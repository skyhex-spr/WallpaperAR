using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Setting", menuName = "ScriptableObjects/Setting", order = 1)]
public class SettingDefination : ScriptableObject
{
    public Surface serface;
}

public enum Surface 
{
    Wall,
    Ground
}
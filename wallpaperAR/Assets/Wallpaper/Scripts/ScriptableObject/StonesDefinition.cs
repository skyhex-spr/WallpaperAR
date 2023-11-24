using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "stonesDefinition", menuName = "ScriptableObjects/stonesDefinition", order = 1)]
public class StonesDefinition : ScriptableObject
{
   public List<Stone> stones;    
}

[Serializable]
public class Stone
{
    [PreviewField]
    public Sprite Icon;
    public string Name;
    public string Description;
    public Material Material;
    public bool IsSquare = true;
    public Surface Surface;
}
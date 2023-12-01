using RTLTMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Switchbtns : MonoBehaviour
{
    public Category category;
    public Button button;
    public RTLTextMeshPro Rtext;
    // Start is called before the first frame update
    void Awake()
    {
        button = GetComponent<Button>();
        Rtext = GetComponent<RTLTextMeshPro>();
    }

    public void Select()
    {
            Color newColor = HexToColor("04838E"); ;
            Rtext.color = newColor;    
    }

    public void Deselect()
    {
        Rtext.color = Color.white;
    }

    Color HexToColor(string hex)
    {
        // Parse hex values to Color
        Color color = new Color();
        ColorUtility.TryParseHtmlString("#" + hex, out color);
        return color;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

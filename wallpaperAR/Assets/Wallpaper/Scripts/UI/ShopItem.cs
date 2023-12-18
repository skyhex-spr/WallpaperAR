using RTLTMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    // Start is called before the first frame update
    public Image Image;
    public RTLTextMeshPro textmesh;

    public Stone stone;

    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
    }
    void Start()
    {
        
    }

    public void SetData(Stone stone)
    { 
        this.stone = stone;
        Image.sprite = stone.Icon;
        textmesh.text = stone.Description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(delegate { SingleItem.Instance.SetAndshowData(stone); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

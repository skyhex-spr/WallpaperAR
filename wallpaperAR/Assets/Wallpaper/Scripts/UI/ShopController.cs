using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    public SettingDefination setting;
    public StonesDefinition StonesDef;

    public List<Switchbtns> categories;

    public GameObject ItemPrefab;
    public GameObject contentshop;
    // Start is called before the first frame update
    void Start()
    {
        foreach (Switchbtns item in categories)
        {
            item.button.onClick.AddListener(delegate{ SetCategory(item.category); });
        }

        SetCategory(Category.wall);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCategory(Category category)
    {
        Debug.Log(category);
        foreach (Switchbtns item in categories)
        {
            if (item.category == category)
                item.Select();
            else
                item.Deselect();
        }

        List<Stone> stones = new List<Stone>();
        if (category == Category.wall)
            stones = StonesDef.stones.FindAll((stone) => stone.Surface == Surface.Wall);
        else
        {
            if (category == Category.parking)
                stones = StonesDef.stones.FindAll((stone) => stone.Surface == Surface.Ground && stone.parking);
            else
                stones = StonesDef.stones.FindAll((stone) => stone.Surface == Surface.Ground && !stone.parking);
        }

        foreach (Transform child in contentshop.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Stone child in stones)
        {
            GameObject obj = Instantiate(ItemPrefab, contentshop.transform);
            ShopItem item = obj.GetComponent<ShopItem>();
            item.SetData(child);
        }

    }
}

public enum Category
{
    floor,
    wall,
    parking
}

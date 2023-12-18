using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleItem : MonoBehaviour
{
    public static SingleItem Instance { get; private set; }
    private EasyTween tween;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        tween = GetComponent<EasyTween>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAndshowData(Stone stone)
    {
        tween.OpenCloseObjectAnimation();
    }
    public void close()
    {
        if (!tween.IsObjectOpened())
            tween.OpenCloseObjectAnimation();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButton : MonoBehaviour
{
    public Sprite firstImage;
    public Sprite secondImage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchSprite()
    {
        if (GameManager.Instance.inGame)
        {
            GetComponent<Image>().sprite = firstImage;

            if (GameManager.Instance.zoomed && this.name == "ZoomInOut")
            {
                GetComponent<Image>().sprite = secondImage;
            }
        }
        else
        {
            GetComponent<Image>().sprite = secondImage;
        }
    }
}

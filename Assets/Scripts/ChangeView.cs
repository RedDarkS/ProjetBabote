using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeView : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera VCAvant;
    [SerializeField] CinemachineVirtualCamera VCApres;

    bool viewOn = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!viewOn)
        {
            viewOn = true;

            GameManager.Instance.zoomed = true;
            
            VCAvant.enabled = false;
            VCApres.enabled = true;
        }
        else
        {
            viewOn = false;

            GameManager.Instance.zoomed = false;

            VCAvant.enabled = true;
            VCApres.enabled = false;
        }
        
    }
}

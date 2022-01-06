using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindPaper : MonoBehaviour
{
    [SerializeField] Transform topTable;
    [SerializeField] Transform dansVase;
    [SerializeField] Transform dansMur;
    [SerializeField] Transform dansLivre;

    [SerializeField] Material validation;

    bool isFouilled = false;

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
        if (!isFouilled)
        {
            isFouilled = true;
            this.GetComponent<MeshRenderer>().material = validation;

            switch(this.name)
            {
                case "vase":
                    dansVase.GetComponent<Animator>().SetTrigger("vaseOUT");
                    break;

                case "livreOuvert":
                    dansLivre.GetComponent<Animator>().SetTrigger("livreOUT");
                    break;

                case "GrossePierre":
                    dansMur.GetComponent<Animator>().SetTrigger("murOUT");
                    break;
            }

            for(int i = 0; i < GameManager.Instance.numPaper + 3; i++)
            {
                GameManager.Instance.listPapier[i].transform.position = topTable.position;
            }
            GameManager.Instance.numPaper += 3;
        }
    }
}

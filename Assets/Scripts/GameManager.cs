using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform CatGameObject;
    [SerializeField] Transform topTable;

    [SerializeField] CinemachineVirtualCamera VCWest;
    [SerializeField] CinemachineVirtualCamera VCSouth;
    [SerializeField] CinemachineVirtualCamera VCEast;
    [SerializeField] CinemachineVirtualCamera VCNorth;

    [SerializeField] CinemachineVirtualCamera VCTable;
    [SerializeField] CinemachineVirtualCamera VCCadenas;
    [SerializeField] CinemachineVirtualCamera VCPostDoor;

    [SerializeField] List<GameObject> Rooms;
    [SerializeField] public List<GameObject> listPapier;

    [SerializeField] Canvas HUD;
    [SerializeField] Canvas menuPrincipal;
    [SerializeField] Canvas carnetUI;
    [SerializeField] Canvas digicodeUI;

    [SerializeField] Button zoomInOut;

    [SerializeField] Text digicodeText;

    [SerializeField] Image fondPause;
    [SerializeField] Image fonduNoir;
    [SerializeField] Image carnetImage;

    [SerializeField] Sprite playSprite;
    [SerializeField] Sprite pauseSprite;
    [SerializeField] Sprite journalDef;
    [SerializeField] Sprite journalMed;
    [SerializeField] Sprite journalObs;

    [SerializeField] ParticleSystem particuleVase;
    [SerializeField] ParticleSystem particuleLivre;
    [SerializeField] ParticleSystem particulePierre;

    public int numCam = 3;
    public int numRoom = 0;
    public int numPaper = 0;

    public int codeMed = 1204;
    public int codeObs = 1745;
    public int codeTel = 1832;

    public int CodeEnter = -1;
    public int indexDigicode = 0;

    public bool inGame;
    public bool zoomed = false;
    public bool drag = false;
    public bool movable = false;

    public static GameManager Instance;

    public RaycastHit hit;
    Ray ray;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen

        HUD.enabled = false;
        menuPrincipal.enabled = true;
        carnetUI.enabled = false;
        digicodeUI.enabled = false;

        zoomInOut.enabled = false;

        fondPause.enabled = false;
        fonduNoir.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (inGame)
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            bool isHit = Physics.Raycast(ray, out hit, 100);
            Debug.DrawRay(Camera.main.transform.position, ray.direction * 20f, Color.red);

            if (!drag)
            {
                CheckInput();
            }

            if(VCCadenas.enabled == true && carnetUI.enabled == false)
            {
                digicodeUI.enabled = true;
            }
            else
            {
                digicodeUI.enabled = false;
            }

            if (zoomed)
            {
                movable = true;
            }
            else
            {
                movable = false;
            }

            if(numCam == 2 || numCam == 3)
            {
                zoomInOut.gameObject.SetActive(true);
            }
            else
            {
                zoomInOut.gameObject.SetActive(false);
            }
        }
    }
    void LoadRoom()
    {
        updateCarnet();
        Instantiate(Rooms[numRoom], CatGameObject);

        if (numRoom == 0)
        {
            for (int i = 0; i < numPaper + 3; i++)
            {
                listPapier[i].transform.position = topTable.position;
            }
            numPaper += 3;
        }
    }
    public void ChangeRoom()
    {
        foreach (Transform t in CatGameObject)
        {
            if(t.name != "Walls" && t.name != "topTable")
            {
                Destroy(t.gameObject);
            }
        }
        numRoom++;
        Instantiate(Rooms[numRoom], CatGameObject);
        updateCarnet();
    }
    public void EndRoom()
    {
        fonduNoir.enabled = true;

        fonduNoir.GetComponent<Animator>().SetTrigger("fondu");

        VCCadenas.enabled = false;
        VCPostDoor.enabled = true;

        Invoke("ChangeRoom", 0.5f);
        Invoke("ResetConst", 2f);
    }
    public void PlayGame()
    {
        menuPrincipal.enabled = false;
        HUD.enabled = true;

        LoadRoom();
        SwitchCam();

        inGame = true;
    }
    public void PauseGame()
    {
        if (inGame)
        {
            inGame = false;
            fondPause.enabled = true;
        }
        else
        {
            inGame = true;
            fondPause.enabled = false;
        }
    }

    public void ZoomInOut()
    {
        if (inGame)
        {
            switch (numCam)
            {
                case 2:
                    if (!zoomed)
                    {
                        zoomed = true;

                        VCTable.enabled = true;
                        VCSouth.enabled = false;
                    }
                    else if (zoomed)
                    {
                        zoomed = false;

                        VCTable.enabled = false;
                        VCSouth.enabled = true;
                    }
                    break;

                case 3:
                    if (!zoomed)
                    {
                        zoomed = true;

                        VCCadenas.enabled = true;
                        VCEast.enabled = false;
                    }
                    else if (zoomed)
                    {
                        zoomed = false;

                        VCCadenas.enabled = false;
                        VCEast.enabled = true;
                    }
                    break;
            }
        }
    }
    public void ecranTitreUI()
    {
        if (menuPrincipal.enabled)
        {
            menuPrincipal.enabled = false;
            HUD.enabled = true;
        }
        else
        {
            menuPrincipal.enabled = true;
            HUD.enabled = false;
        }
    }
    public void CarnetUI()
    {
        if (carnetUI.enabled)
        {
            inGame = true;
            carnetUI.enabled = false;
        }
        else
        {
            inGame = false;
            carnetUI.enabled = true;
            digicodeUI.enabled = false;
        }
    }
    void updateCarnet()
    {
        //carnetUI.enabled = true;
        switch (numRoom)
        {
            case 0:
                carnetImage.GetComponent<Image>().sprite = journalMed;
                break;

            case 1:
                carnetImage.GetComponent<Image>().sprite = journalObs;
                break;

            default:
                carnetImage.GetComponent<Image>().sprite = journalDef;
                break;
        }
        //carnetUI.enabled = false;
    }
    public void DigicodeUI()
    {
        if (digicodeUI.enabled)
        {
            digicodeUI.enabled = false;
        }
        else
        {
            digicodeUI.enabled = true;
        }
    }
    public void digicode(int input)
    {
        switch (indexDigicode)
        {
            case 0:
                CodeEnter++;
                CodeEnter += 1000 * input;
                indexDigicode++;
                digicodeText.text += input;
                break;

            case 1:
                CodeEnter += 100 * input;
                indexDigicode++;
                digicodeText.text += input;
                break;

            case 2:
                CodeEnter += 10 * input;
                indexDigicode++;
                digicodeText.text += input;
                break;

            case 3:
                CodeEnter += input;
                indexDigicode++;
                digicodeText.text += input;
                Invoke("verifDigicode", 0.5f);
                break;
        }
    }
    void verifDigicode()
    {
        switch (numRoom)
        {
            case 0:
                verifcodeCadenas(codeMed);
                break;
            case 1:
                verifcodeCadenas(codeObs);
                break;
            case 2:
                verifcodeCadenas(codeTel);
                break;
        }
    }
    void verifcodeCadenas(int code)
    {
        if (CodeEnter == codeMed)
        {
            digicodeText.color = Color.green;

            Invoke("resetDigicode", 0.5f);
            Invoke("EndRoom", 0.7f);
        }
        else
        {
            digicodeText.color = Color.red;

            Invoke("resetDigicode", 0.5f);
        }
    }
    public void resetDigicode()
    {
        CodeEnter = -1;
        indexDigicode = 0;
        digicodeText.text = "";
        digicodeText.color = Color.white;
    }
    void SwitchCam()
    {
        switch (numCam)
        {
            case 1:
                VCWest.enabled = true;
                VCSouth.enabled = false;
                VCEast.enabled = false;
                VCNorth.enabled = false;

                zoomInOut.enabled = false;

                break;

            case 2:
                VCWest.enabled = false;
                VCSouth.enabled = true;
                VCEast.enabled = false;
                VCNorth.enabled = false;

                zoomInOut.enabled = true;

                break;

            case 3:
                VCWest.enabled = false;
                VCSouth.enabled = false;
                VCEast.enabled = true;
                VCNorth.enabled = false;

                zoomInOut.enabled = true;

                break;

            case 4:
                VCWest.enabled = false;
                VCSouth.enabled = false;
                VCEast.enabled = false;
                VCNorth.enabled = true;

                zoomInOut.enabled = false;

                break;

            default:
                VCWest.enabled = false;
                VCSouth.enabled = true;
                VCEast.enabled = false;
                VCNorth.enabled = false;

                break;
        }
    }
    void CheckInput()
    {
        if (Input.touchCount == 1) //user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {
                    //It's a drag
                    //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {
                        //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            //Debug.Log("Right Swipe");

                            if (!zoomed)
                            {
                                numCam--;
                                if (numCam < 1)
                                {
                                    numCam = 4;
                                }
                                SwitchCam();
                            }
                        }
                        else
                        {   //Left swipe
                            //Debug.Log("Left Swipe");

                            if (!zoomed)
                            {
                                numCam++;
                                if (numCam > 4)
                                {
                                    numCam = 1;
                                }
                                SwitchCam();
                            }
                        }
                    }
                    else
                    {
                        //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            //Debug.Log("Up Swipe");

                        }
                        else
                        {   //Down swipe
                            //Debug.Log("Down Swipe");

                        }
                    }
                }
                else
                {
                    //It's a tap as the drag distance is less than 20% of the screen height
                    //Debug.Log("Tap");
                }
            }
        }
    }
    void ResetConst()
    {
        numCam = 2;
        VCPostDoor.enabled = false;
        VCSouth.enabled = true;

        fonduNoir.enabled = false;

        zoomed = false;

        resetDigicode();
    }
}

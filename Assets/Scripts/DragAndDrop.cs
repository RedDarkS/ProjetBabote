using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    //[SerializeField] Transform target;

    float mzCoord;
    Vector3 mOffset;

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
        if (GameManager.Instance.movable)
        {
            mzCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
            mOffset = gameObject.transform.position - GetMouseWorldPos();
        }
    }

    private void OnMouseUp()
    {
        GameManager.Instance.drag = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = mzCoord;

        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void OnMouseDrag()
    {
        if (GameManager.Instance.movable)
        {
            GameManager.Instance.drag = true;
            GetComponent<Rigidbody>().MovePosition(GetMouseWorldPos() + mOffset);
            transform.position = GameManager.Instance.hit.point;
        }
    }
}

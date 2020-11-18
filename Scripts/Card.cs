using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Card : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Camera MainCamera;
    private Vector3 offset;
    [HideInInspector]
    public Transform defParent, defemptyCP;
    private GameObject emptyCP;
    public bool IsDraggable;

    public GameManager gameManager;
    private void Awake()
    {
        MainCamera = Camera.allCameras[0];
        emptyCP = GameObject.Find("emptyCP");

        gameManager = FindObjectOfType<GameManager>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        
        if (!IsDraggable)
        {
            return;
        }

        Vector3 pos = MainCamera.ScreenToWorldPoint(eventData.position);
        transform.position = pos+offset;

        if (emptyCP.transform.parent != defemptyCP) 
        {
            emptyCP.transform.SetParent(defemptyCP);
        }


        if (defParent.GetComponent<DropPnl>().Type != FieldType.MY_PLGRD)
        {
            Pos();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offset = transform.position - MainCamera.ScreenToWorldPoint(eventData.position);
        defParent = defemptyCP = transform.parent;


        IsDraggable = (defParent.GetComponent<DropPnl>().Type == FieldType.MY_PNL
            || defParent.GetComponent<DropPnl>().Type == FieldType.MY_PLGRD) && gameManager.IsMyStep;

        if (!IsDraggable) 
        {
            return;
        }

        emptyCP.transform.SetParent(defParent);
        emptyCP.transform.SetSiblingIndex(transform.GetSiblingIndex());

        transform.SetParent(defParent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (!IsDraggable)
        {
            return;
        }

        transform.SetParent(defParent);
        GetComponent<CanvasGroup>().blocksRaycasts = true;

        transform.SetSiblingIndex(emptyCP.transform.GetSiblingIndex());

        emptyCP.transform.SetParent(GameObject.Find("Canvas").transform);
        emptyCP.transform.localPosition = new Vector3(2949, 0);
    }


    void Pos() 
    {
        int idx = defemptyCP.childCount;

        for (int i = 0; i < defemptyCP.childCount; i++) 
        {
            if (transform.position.x < defemptyCP.GetChild(i).position.x) 
            {
                idx = i;
                if (emptyCP.transform.GetSiblingIndex() < idx) 
                {
                    idx--;
                }

                break;
            }
                
        }

        emptyCP.transform.SetSiblingIndex(idx);
    }
}

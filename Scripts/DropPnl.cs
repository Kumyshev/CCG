using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public enum FieldType 
    {
        MY_PNL,
        MY_PLGRD,
        ENEMY_PNL,
        ENEMY_PLGRD
    }
public class DropPnl : MonoBehaviour, IDropHandler,IPointerEnterHandler,IPointerExitHandler
{

    public FieldType Type;
    public void OnDrop(PointerEventData eventData)
    {

        if (Type != FieldType.MY_PLGRD) 
        {
            return;
        }

        Card card = eventData.pointerDrag.GetComponent<Card>();

        if (card && card.gameManager.PlrPlgrCards.Count <= 4)  
        {
            card.gameManager.PlrPlnCards.Remove(card.GetComponent<CardInfo>());
            card.gameManager.PlrPlgrCards.Add(card.GetComponent<CardInfo>());
            card.defParent = transform;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null||Type==FieldType.ENEMY_PLGRD || Type == FieldType.ENEMY_PNL||
            Type == FieldType.MY_PNL) 
        {
            return;
        }
        Card card = eventData.pointerDrag.GetComponent<Card>();

        if (card)
        {
            card.defemptyCP = transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
        {
            return;
        }
        Card card = eventData.pointerDrag.GetComponent<Card>();

        if (card && card.defemptyCP == transform) 
        {
            card.defemptyCP = card.defParent;
        }
    }
}

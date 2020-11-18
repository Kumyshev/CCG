using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardAttack : MonoBehaviour,IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        CardInfo card = eventData.pointerDrag.GetComponent<CardInfo>();

        if (card && card.card.Attacking && 
            transform.parent == GetComponent<Card>().gameManager.EnemyPlgr) 
        {
            card.card.AttackState(false);
            GetComponent<Card>().gameManager.Damage(card, GetComponent<CardInfo>());
        }
    }

}

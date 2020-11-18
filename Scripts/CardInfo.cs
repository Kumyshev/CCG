using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardInfo : MonoBehaviour
{
    public CardStr card;
    public TextMeshProUGUI Name,Attack,Defense;
    public Image Logo;


    public void CloseCardInfo(CardStr _card)
    {
        card = _card;
        //Logo.sprite = null;
        //Name.text = "";

        CardInfoShow(card);
    }
    public void CardInfoShow(CardStr _card) 
    {
        card = _card;

        Logo.sprite = card.Logo;
        Logo.preserveAspect = true;
        Name.text = card.Name;

        RefreshData();
    }

    public void RefreshData() 
    {
        Attack.text = card.Attack.ToString();
        Defense.text = card.Defense.ToString();
    }

    private void Start()
    {
        //CardInfoShow(CardMR.allCards[transform.GetSiblingIndex()]);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Game 
{
    public List<CardStr> EnemyDeck, PlrDeck;

    public Game() 
    {
        EnemyDeck = GiveCard();


        PlrDeck = GiveCard();

    }

    List<CardStr> GiveCard() 
    {
        List<CardStr> cards = new List<CardStr>();
        for (int i = 0; i < 10; i++) 
        {
            cards.Add(CardMR.allCards[Random.Range(0, CardMR.allCards.Count)]);
        }

        return cards;
    }
}

public class GameManager : MonoBehaviour
{

    public Game GetGame;
    public Transform EnemyPnl, EnemyPlgr, PlrPnl, PlrPlgr;
    public GameObject CardPref;
    int Turn, TimeTurn = 30;
    public TextMeshProUGUI gameTime;
    public Button btnTime;


    public List<CardInfo> PlrPlnCards = new List<CardInfo>(),
        PlrPlgrCards = new List<CardInfo>(),
        EnemyPlnCards = new List<CardInfo>(),
        EnemyPlgrCards = new List<CardInfo>();

    public bool IsMyStep 
    {
        get 
        {
            return Turn % 2 == 0;
        }
    }


    void Start()
    {
        Turn = 0;
        GetGame = new Game();

        PnlCards(GetGame.EnemyDeck, EnemyPnl);
        PnlCards(GetGame.PlrDeck, PlrPnl);

        StartCoroutine(TurnFunc());

    }

    void PnlCards(List<CardStr> cards, Transform pnl)  
    {
        int i = 0;
        while (i++ < 4) 
        {
            CardsOnPnl(cards, pnl);
        }
    }

    void CardsOnPnl(List<CardStr> cards, Transform pnl) 
    {
        if (cards.Count == 0) 
        {
            return;
        }

        CardStr card = cards[0];

        GameObject instCard = Instantiate(CardPref, pnl, false);

        if (pnl == EnemyPnl)
        {
            instCard.GetComponent<CardInfo>().CloseCardInfo(card);
            EnemyPlnCards.Add(instCard.GetComponent<CardInfo>());
        }  
        else
        {
            instCard.GetComponent<CardInfo>().CardInfoShow(card);
            PlrPlnCards.Add(instCard.GetComponent<CardInfo>());

            instCard.GetComponent<CardAttack>().enabled = true;
        }

        cards.RemoveAt(0);
    }

    public void ChangeTurn() 
    {
        //StopCoroutine(TurnFunc());
        StopAllCoroutines();
        Turn++;

        btnTime.interactable = IsMyStep;

        if (IsMyStep) 
        {
            NewCard();
        }
        StartCoroutine(TurnFunc());
    }

    public void NewCard() 
    {
        CardsOnPnl(GetGame.EnemyDeck, EnemyPnl);
        CardsOnPnl(GetGame.PlrDeck, PlrPnl);
    }

    IEnumerator TurnFunc() 
    {
        TimeTurn = 30;

        gameTime.text = TimeTurn.ToString();

        if (IsMyStep)
        {
            foreach (var card in PlrPlgrCards) 
            {
                card.card.AttackState(true);
            }

            while (TimeTurn-- > 0)
            { 
                gameTime.text = TimeTurn.ToString();
                yield return new WaitForSeconds(1);
            }
        }
        else 
        {
            foreach (var card in EnemyPlgrCards)
            {
                card.card.AttackState(true);
            }

            while (TimeTurn-- > 27)
            {
                gameTime.text = TimeTurn.ToString();
                yield return new WaitForSeconds(1);
            }

            if (EnemyPlnCards.Count > 0) 
            {
                EnemyStep(EnemyPlnCards);
            }
        }

        ChangeTurn();
    }

    void EnemyStep(List<CardInfo> cards) 
    {
        int count = cards.Count == 1 ? 1 : Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++) 
        {
            if (EnemyPlnCards.Count > 4) 
            {
                return;
            }

            cards[0].CardInfoShow(cards[0].card);
            cards[0].transform.SetParent(EnemyPlgr);


            EnemyPlgrCards.Add(cards[0]);
            EnemyPlnCards.Remove(cards[0]);
        }
        
    }

    public void Damage(CardInfo plrInfo, CardInfo enemyInfo)  
    {
        plrInfo.card.GetDamage(enemyInfo.card.Attack);
        enemyInfo.card.GetDamage(plrInfo.card.Attack);


        if (!plrInfo.card.IsAlive)
        {
            DestroyCard(plrInfo);
        }
        else
        {
            plrInfo.RefreshData();
        }

        if (!enemyInfo.card.IsAlive)
        {
            DestroyCard(enemyInfo);
        }
        else
        {
            enemyInfo.RefreshData();
        }
    }

    void DestroyCard(CardInfo card) 
    {
        card.GetComponent<Card>().OnEndDrag(null);

        if (EnemyPlgrCards.Exists(x => x == card)) 
        {
            EnemyPlgrCards.Remove(card);
        }

        if (PlrPlgrCards.Exists(x => x == card))
        {
            PlrPlgrCards.Remove(card);
        }


        Destroy(card.gameObject);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct CardStr
{
    public string Name;
    public Sprite Logo;
    public int Attack, Defense;
    public bool Attacking;

    public bool IsAlive 
    {
        get 
        {
            return Defense > 0;
        }
    }

    public CardStr(string name, string logo, int attack, int defense) 
    {
        Name = name;
        Logo = Resources.Load<Sprite>(logo);
        Attack = attack;
        Defense = defense;
        Attacking = false;
    }

    public void AttackState(bool state) 
    {
        Attacking = state;
    }


    public void GetDamage(int dmg) 
    {
        Defense -= dmg;
    }
}

public static class CardMR 
{
    public static List<CardStr> allCards = new List<CardStr>();
}
public class CardManager : MonoBehaviour
{
    private void Awake()
    {
        CardMR.allCards.Add(new CardStr("1gg", "Sprite/1", 2, 7));
        CardMR.allCards.Add(new CardStr("2gg", "Sprite/2", 4, 5));
        CardMR.allCards.Add(new CardStr("gg3", "Sprite/3", 3, 6));
    }
}

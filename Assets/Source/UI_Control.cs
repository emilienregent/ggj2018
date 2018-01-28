using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Control : MonoBehaviour
{

    public Image player1health;
    public Image player2health;
    public Image player3health;
    public Image player4health;

    public GameObject LifeOne;
    public GameObject LifeTwo;
    public GameObject LifeThree;
    public GameObject LifeFour;
    public GameObject LifeFive;
    public GameObject LifeSix;

    private PlayerController Player1;
    private PlayerController Player2;
    private PlayerController Player3;
    private PlayerController Player4;

    private float p1hp;
    private float p1hpMax;
    private float p2hp;
    private float p2hpMax;
    private float p3hp;
    private float p3hpMax;
    private float p4hp;
    private float p4hpMax;

    public Text KillScore;


    int num = 1;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Player1 != null)
        {
            p1hp = Player1.hp;
            p1hpMax = Player1.fullHp;
            player1health.fillAmount = p1hp / p1hpMax;
        }
        if (Player2 != null)
        {
            p2hp = Player2.hp;
            p2hpMax = Player2.fullHp;
            player2health.fillAmount = p2hp / p2hpMax;
        }
        if (Player3 != null)
        {
            p3hp = Player3.hp;
            p1hpMax = Player3.fullHp;
            player3health.fillAmount = p3hp / p3hpMax;
        }
        if (Player4 != null)
        {
            p4hp = Player4.hp;
            p4hpMax = Player4.fullHp;
            player4health.fillAmount = p4hp / p4hpMax;
        }

       // KillScore.text = Game.GetKillCounter();


    }
    public void registerPlayer(PlayerController Player)
    {
        if (Player.playerId == 1)
        {
            Player1 = Player;
        }
        if (Player.playerId == 2)
        {
            Player2 = Player;
        }
        if (Player.playerId == 3)
        {
            Player3 = Player;
        }
        if (Player.playerId == 4)
        {
            Player4 = Player;
        }
    }
    public void LoseLife(int i)
    {
        if (i == 5)
        {
            LifeOne.SetActive(false);
        }
        if (i == 4)
        {
            LifeTwo.SetActive(false);
        }
        if (i == 3)
        {
            LifeThree.SetActive(false);
        }
        if (i == 2)
        {
            LifeFour.SetActive(false);
        }
        if (i == 1)
        {
            LifeSix.SetActive(false);
        }

    }
}

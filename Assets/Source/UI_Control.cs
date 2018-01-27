using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Control : MonoBehaviour {

    public Text player1health;
    private PlayerController Player1;
    
    private float p1hp;
    int num = 1;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        if(Player1 != null)
        {
            p1hp = Player1.hp;
        player1health.text = "" + p1hp;
        }
    }
    public void registerPlayer(PlayerController Player)
    {
        if(Player.playerId == 1)
        {
            Player1 = Player;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectionManager : MonoBehaviour {

    public PlayerSelectionController[] players;
    public CanvasRenderer pressStart;
    public CanvasRenderer pressStartTuto;
    public CanvasRenderer tutorial;
    public CanvasRenderer[] pressAButtonLabels;
    private int _cJoysticks;
    private int _playersReady = 0;
    private bool _isTutorial = false;
    private bool _hasAtLeastOnePlayerReady = false;

	// Use this for initialization
	void Start ()
    {
        for(int i = 0; i < players.Length; i++)
        {
            PlayerPrefs.DeleteKey("Player_" + players[i].playerId + "_controller");
        }

        _cJoysticks = Mathf.Min(Input.GetJoystickNames().Length, 4);
        for (int j = 0; j < _cJoysticks; j++)
        {
            players[j].gameObject.SetActive(true);
            pressAButtonLabels[j].gameObject.SetActive(true);
        }

        #if NO_CONTROLLER
        players[0].gameObject.SetActive(true);
        pressAButtonLabels[0].gameObject.SetActive(true);
        #endif

    }
	
	// Update is called once per frame
	void Update () {

        int count = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].ready == true)
                count++;
        }

        if (_hasAtLeastOnePlayerReady == false && _playersReady > 1)
        {
            pressStart.gameObject.SetActive(true);

            Debug.Log("Player 1 use controler : " + PlayerPrefs.GetInt("Player_1_controller"));
            Debug.Log("Player 2 use controler : " + PlayerPrefs.GetInt("Player_2_controller"));
            Debug.Log("Player 3 use controler : " + PlayerPrefs.GetInt("Player_3_controller"));
            Debug.Log("Player 4 use controler : " + PlayerPrefs.GetInt("Player_4_controller"));

            _hasAtLeastOnePlayerReady = true;
        }

        _playersReady = count;

        if (Input.GetButtonDown("Submit"))
        {
            #if NO_CONTROLLER
            SelectPlayer(players[0], 0, 1);
            #endif

            for (int joystickId = 1; joystickId < _cJoysticks + 1; joystickId++)
            {
                //Debug.Log("Is controller " + joystickId + " is pressing A ?");
                if (Input.GetButtonDown("Joystick" + joystickId + "Submit"))
                {
                    //Debug.Log("Controller " + joystickId + " is pressing A");
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (PlayerPrefs.HasKey("Player_" + players[i].playerId + "_controller") == false)
                        {
                            SelectPlayer(players[i], i, joystickId);
                            break;
                        }
                        else if (PlayerPrefs.GetInt("Player_" + players[i].playerId + "_controller") == joystickId)
                        {
                            break;
                        }
                    }
                }
            }

            PlayerPrefs.Save();
        }

        if((Input.GetButtonDown("JoystickStart") || Input.GetButtonDown("Submit")) && _playersReady > 1)
        {
            if (_isTutorial == false)
            {
                tutorial.gameObject.SetActive(true);
                _isTutorial = true;
            }
            else
            {
                pressStartTuto.GetComponentInChildren<Text>().text = "Loading ...";
                SceneManager.LoadScene(2);
            }
        }
    }

    private void SelectPlayer(PlayerSelectionController player, int playerIndex, int joystickId)
    {
        PlayerPrefs.SetInt("Player_" + player.playerId + "_controller", joystickId);

        player.Select();
    }
}
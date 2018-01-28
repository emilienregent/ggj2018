using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectionManager : MonoBehaviour {

    public PlayerSelectionController[] players;
    public CanvasRenderer pressStart;
    public CanvasRenderer tutorial;
    public CanvasRenderer[] pressAButtonLabels;
    private int _cJoysticks;
    private int _playersReady = 0;
    private bool _isTutorial = false;

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

        if (Input.GetButtonDown("Submit"))
        {
            #if NO_CONTROLLER
            _playersReady += 2;

            PlayerPrefs.SetInt("Player_" + players[0].playerId + "_controller", 1);
            players[0].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
            pressAButtonLabels[0].GetComponentInChildren<Text>().text = "Ready !";
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
                            PlayerPrefs.SetInt("Player_" + players[i].playerId + "_controller", joystickId);
                            pressAButtonLabels[i].GetComponentInChildren<Text>().text = "Ready !";
                            _playersReady++;
                            if (_playersReady > 1)
                            {
                                pressStart.gameObject.SetActive(true);

                                Debug.Log("Player 1 use controler : " + PlayerPrefs.GetInt("Player_1_controller"));
                                Debug.Log("Player 2 use controler : " + PlayerPrefs.GetInt("Player_2_controller"));
                                Debug.Log("Player 3 use controler : " + PlayerPrefs.GetInt("Player_3_controller"));
                                Debug.Log("Player 4 use controler : " + PlayerPrefs.GetInt("Player_4_controller"));
                            }
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
                SceneManager.LoadScene(2);
            }
        }
    }
}
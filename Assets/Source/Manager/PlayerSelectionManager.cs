using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelectionManager : MonoBehaviour {

    public PlayerSelectionController[] players;
    public CanvasRenderer pressStart;
    public CanvasRenderer[] pressAButtonLabels;
    private int _cJoysticks;
    private int _playersReady = 0;

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

    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Submit"))
        {

            for (int joystickId = 1; joystickId < _cJoysticks + 1; joystickId++)
            {
                Debug.Log("Check controller " + joystickId);
                if (Input.GetButtonDown("Joystick" + joystickId + "Submit"))
                {
                    Debug.Log("Controller " + joystickId);
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (PlayerPrefs.HasKey("Player_" + players[i].playerId + "_controller") == false)
                        {
                            PlayerPrefs.SetInt("Player_" + players[i].playerId + "_controller", joystickId);
                            players[i].GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
                            pressAButtonLabels[i].GetComponentInChildren<Text>().text = "Ready !";
                            _playersReady++;
                            if (_playersReady > 1)
                            {
                                pressStart.gameObject.SetActive(true);

                                Debug.Log(PlayerPrefs.GetInt("Player_1_controller"));
                                Debug.Log(PlayerPrefs.GetInt("Player_2_controller"));
                                Debug.Log(PlayerPrefs.GetInt("Player_3_controller"));
                                Debug.Log(PlayerPrefs.GetInt("Player_4_controller"));
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

        if(Input.GetButtonDown("JoystickStart") && _playersReady > 1)
        {
            SceneManager.LoadScene(2);
        }

    }
}

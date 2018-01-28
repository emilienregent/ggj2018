using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatController : MonoBehaviour {

    public Text[] texts = null;
    public Frequency[] frequencies = null;
    public GameObject[] players = null;

	// Use this for initialization
	private void Start () 
    {
        int killPlayerId = PlayerPrefs.GetInt("killPlayerId", 0);
        int kickPlayerId = PlayerPrefs.GetInt("kickPlayerId", 0);
        int shotPlayerId = PlayerPrefs.GetInt("shotPlayerId", 0);
        int hitPlayerId = PlayerPrefs.GetInt("hitPlayerId", 0);

        int killCounter = PlayerPrefs.GetInt("killCounter", 0);
        int kickCounter = PlayerPrefs.GetInt("kickCounter", 0);
        int shotCounter = PlayerPrefs.GetInt("shotCounter", 0);
        int hitCounter = PlayerPrefs.GetInt("hitCounter", 0);

        if (killPlayerId != 0)
        {
            texts[0].text = "KILLS " + killCounter.ToString();
            texts[0].GetComponent<CanvasColorBehaviour>().SetFrequency(frequencies[killPlayerId - 1]);
            players[killPlayerId - 1].SetActive(true);
        }
        else
            texts[0].transform.parent.gameObject.SetActive(false);

        if (kickPlayerId != 0)
        {
            texts[1].text = "KICKS " + kickCounter.ToString();
            texts[1].GetComponent<CanvasColorBehaviour>().SetFrequency(frequencies[kickPlayerId - 1]);
            players[kickPlayerId - 1].SetActive(true);
        }
        else
            texts[1].transform.parent.gameObject.SetActive(false);

        if (shotPlayerId != 0)
        {
            texts[2].text = "SHOTS " + shotCounter.ToString();
            texts[2].GetComponent<CanvasColorBehaviour>().SetFrequency(frequencies[shotPlayerId - 1]);
            players[shotPlayerId - 1].SetActive(true);
        }
        else
            texts[2].transform.parent.gameObject.SetActive(false);

        if (hitPlayerId != 0)
        {
            texts[3].text = "HITS " + hitCounter.ToString();
            texts[3].GetComponent<CanvasColorBehaviour>().SetFrequency(frequencies[hitPlayerId - 1]);
            players[hitPlayerId - 1].SetActive(true);
        }
        else
            texts[3].transform.parent.gameObject.SetActive(false);
	}
}
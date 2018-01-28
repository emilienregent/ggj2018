using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionController : MonoBehaviour {

    public int playerId;
    public Animator doors;
    public Text playerText = null;
    [HideInInspector] public bool ready = false;

    public void Select()
    {
        ready = true;
        playerText.text = "Ready !";
        doors.SetTrigger("OpenDoor");
    }
}
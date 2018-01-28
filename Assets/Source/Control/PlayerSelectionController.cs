using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionController : MonoBehaviour {

    public int playerId;
    public Animator doors;
    public ColorBehaviour colorBehaviour;
    public Text playerText = null;
    [HideInInspector] public bool ready = false;

    public void Select()
    {
        doors.SetTrigger("OpenDoor");

        StartCoroutine("OnCompleteAnimation");
    }

    IEnumerator OnCompleteAnimation()
    {
        while(doors.GetCurrentAnimatorStateInfo(0).IsName("Empty"))
            yield return null;
        
        while(doors.GetCurrentAnimatorStateInfo(0).normalizedTime < .5f)
            yield return null;

        ready = true;
        playerText.text = "Ready !";
    }
}
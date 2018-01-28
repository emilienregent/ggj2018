using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionController : MonoBehaviour {

    public int playerId;
    public Animator doors;
    public Text playerText = null;
    [HideInInspector] public bool ready = false;

    public Frequency frequency;
    public GameObject[] characterModels = null;

    public void Start()
    {
        GameObject character = null;
        switch (frequency)
        {
            case Frequency.Hz1:
                character = GameObject.Instantiate(characterModels[0], transform);
                break;
            case Frequency.Hz2:
                character = GameObject.Instantiate(characterModels[1], transform);
                break;
            case Frequency.Hz3:
                character = GameObject.Instantiate(characterModels[2], transform);
                break;
            case Frequency.Hz4:
                character = GameObject.Instantiate(characterModels[3], transform);
                break;
        }

        ColorBehaviour[] colors = character.GetComponentsInChildren<ColorBehaviour>();
        for (int i = 0; i < colors.Length; i++)
            colors[i].SetFrequency(frequency);

        character.transform.localPosition = Vector3.zero;
        character.transform.localEulerAngles = Vector3.zero;
    }

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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorGiver : MonoBehaviour
{
    [SerializeField] Frequency _frequency = Frequency.Hz1;
    private Frequency _oldFrequency;

    void Start ()
    {
        UpdateFrequency();
    }

    private void Update()
    {
        if (_oldFrequency != _frequency)
            UpdateFrequency();
    }

    private void UpdateFrequency()
    {
        ColorBehaviour[] colorBehaviours = GetComponentsInChildren<ColorBehaviour>();
        for (int i = 0; i < colorBehaviours.Length; ++i)
            colorBehaviours[i].SetFrequency(_frequency);

        _oldFrequency = _frequency;
    }
}

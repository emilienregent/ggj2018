using System.Collections;
using UnityEngine;

public class MutliColorSwitcher : MonoBehaviour
{
    [SerializeField] private float _delay = 0.25f;
    private ColorBehaviour[] _colorBehaviours = null;
    Frequency _currentFrequency;

    private void Start()
    {
        _colorBehaviours = GetComponentsInChildren<ColorBehaviour>();
        _currentFrequency = _colorBehaviours[0].frequency;
        StartCoroutine(SwitchColor());
    }

    private IEnumerator SwitchColor()
    {
        while (true)
        {
            yield return new WaitForSeconds(_delay);

            Frequency nextFrequency = Frequency.Hz1;
            if (_currentFrequency == Frequency.Hz1) nextFrequency = Frequency.Hz2;
            else if (_currentFrequency == Frequency.Hz2) nextFrequency = Frequency.Hz3;
            else if (_currentFrequency == Frequency.Hz3) nextFrequency = Frequency.Hz4;
            //else if (_currentFrequency == Frequency.Hz4) nextFrequency = Frequency.Hz1;

            for (int i = 0; i < _colorBehaviours.Length; ++i)
                _colorBehaviours[i].SetFrequency(nextFrequency);

            _currentFrequency = nextFrequency;
        }
    }
}

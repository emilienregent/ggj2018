using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Graphic))]
public class CanvasColorBehaviour : ColorBehaviour
{
    private Graphic _graphic = null;

    public override void SetFrequency(Frequency frequency)
    {
        if (_graphic == null)
            _graphic = GetComponent<Graphic>();

        Color color = Color.white;
        if (frequency == Frequency.Hz1)         color = YELLOW * _color;
        else if (frequency == Frequency.Hz2)    color = RED * _color;
        else if (frequency == Frequency.Hz3)    color = GREEN * _color;
        else if (frequency == Frequency.Hz4)    color = BLUE * _color;

        _graphic.color = color;
    }
}

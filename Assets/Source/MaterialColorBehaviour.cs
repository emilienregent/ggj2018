using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class MaterialColorBehaviour : ColorBehaviour
{
    [SerializeField] private int _matIndex = 0;


    public override void SetFrequency(Frequency frequency)
    {
        if (_renderer == null)
            _renderer = GetComponent<Renderer>();

        Color color = Color.white;
        if (frequency == Frequency.Hz1)         color = YELLOW * _color;
        else if (frequency == Frequency.Hz2)    color = RED * _color;
        else if (frequency == Frequency.Hz3)    color = GREEN * _color;
        else if (frequency == Frequency.Hz4)    color = BLUE * _color;

        _renderer.materials[_matIndex].SetColor("_Color", color);
    }
}

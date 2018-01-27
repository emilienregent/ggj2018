using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class ColorBehaviour : MonoBehaviour
{
    [SerializeField] private Color _color = Color.white;

    private Renderer _renderer = null;
    private MaterialPropertyBlock _block = null;

    [HideInInspector] public Frequency frequency;


    public void SetFrequency(Frequency frequency)
    {
        this.frequency = frequency;

        if (_renderer == null)
        {
            _block = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();
        }
        Color color = Color.white;
        if (frequency == Frequency.Hz1)         color = Color.yellow * _color;
        else if (frequency == Frequency.Hz2)    color = Color.red * _color;
        else if (frequency == Frequency.Hz3)    color = Color.green * _color;
        else if (frequency == Frequency.Hz4)    color = Color.blue * _color;

        _renderer.GetPropertyBlock(_block);
        _block.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_block);
    }
}

using UnityEngine;

//[RequireComponent(typeof(Renderer))]
public class ColorBehaviour : MonoBehaviour
{
    public static readonly Color YELLOW = Color.yellow;
    public static readonly Color RED    = Color.red;
    public static readonly Color GREEN  = Color.green;
    public static readonly Color BLUE   = Color.blue;

    [SerializeField] protected Color _color = Color.white;

    protected Renderer _renderer = null;
    private MaterialPropertyBlock _block = null;

    [HideInInspector] public Frequency frequency;


    public virtual void SetFrequency(Frequency frequency)
    {
        this.frequency = frequency;

        if (_renderer == null)
        {
            _block = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();
        }
        Color color = Color.white;
        if (frequency == Frequency.Hz1)         color = YELLOW * _color;
        else if (frequency == Frequency.Hz2)    color = RED * _color;
        else if (frequency == Frequency.Hz3)    color = GREEN * _color;
        else if (frequency == Frequency.Hz4)    color = BLUE * _color;

        _renderer.GetPropertyBlock(_block);
        _block.SetColor("_Color", color);
        _renderer.SetPropertyBlock(_block);
    }
}

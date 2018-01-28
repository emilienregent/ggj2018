using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraColorBehaviour : MonoBehaviour
{
    [SerializeField] public Frequency frequency;

    private Camera _camera = null;
    private Color _color = new Color(0.5f, 0.5f, 0.5f, 1f);


    public void Start()
    {
        Color color = Color.white;
        if (frequency == Frequency.Hz1)         color = ColorBehaviour.YELLOW * _color;
        else if (frequency == Frequency.Hz2)    color = ColorBehaviour.RED * _color;
        else if (frequency == Frequency.Hz3)    color = ColorBehaviour.GREEN * _color;
        else if (frequency == Frequency.Hz4)    color = ColorBehaviour.BLUE * _color;

        _camera.backgroundColor = color;
    }
}

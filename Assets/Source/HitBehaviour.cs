using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class HitBehaviour : MonoBehaviour
{
    private static readonly string HIT_FEATURE = "HIT";
    private static readonly string GHOST_FEATURE = "GHOST";

    private static readonly int HIT = Shader.PropertyToID("_Hit");

    [SerializeField] public float _duration = 1f;
    [SerializeField] public bool hit = false;
    [SerializeField] public bool ghost = false;

    private Renderer _renderer = null;
    private bool _oldHit = false;
    private float _elapsedTime = 0f;
    private MaterialPropertyBlock _block = null;

	void Start ()
    {
        _renderer = GetComponent<Renderer>();
        _block = new MaterialPropertyBlock();
	}

	void Update ()
    {
        if (ghost) Shader.EnableKeyword(GHOST_FEATURE);
        else Shader.DisableKeyword(GHOST_FEATURE);

        if (_oldHit != hit)
        {
            _oldHit = hit;
            if (hit)
            {
                Shader.EnableKeyword(HIT_FEATURE);
                _elapsedTime = 0f;
            }
            else
                Shader.DisableKeyword(HIT_FEATURE);
        }
        if (hit)
        {
            if (_elapsedTime >= _duration)
            {
                _elapsedTime = _duration;
                hit = false;
            }

            _renderer.GetPropertyBlock(_block);
            _block.SetFloat(HIT, _elapsedTime / _duration);
            _renderer.SetPropertyBlock(_block);
            _elapsedTime += Time.deltaTime;
        }
	}
}

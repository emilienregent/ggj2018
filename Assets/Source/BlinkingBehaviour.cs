using System.Collections;
using UnityEngine;

public class BlinkingBehaviour : MonoBehaviour
{
    public float delay = 0.25f;

    private Renderer[] _renderers = null;
    private bool _enabled = true;


    public void StartBlink()
    {
        StartCoroutine("Blink");
    }

    public void StopBlink()
    {
        StopCoroutine("Blink");
        EnableRenderers(true);
    }

    private IEnumerator Blink()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            EnableRenderers(!_enabled);
        }
    }

    private void EnableRenderers(bool enabled)
    {
        if (_renderers == null)
            _renderers = GetComponentsInChildren<Renderer>();

        for (int i = 0; i < _renderers.Length; ++i)
            _renderers[i].enabled = enabled;

        _enabled = enabled;
    }
}

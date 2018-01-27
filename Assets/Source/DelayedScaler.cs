using System.Collections;
using UnityEngine;

public class DelayedScaler : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _delay = 0f;
    private float _elapsedTime = 0f;
    private bool _init = false;
    private bool _running = false;


    private void Start()
    {
        transform.localScale = Vector3.zero;
        StartCoroutine(DelayedStart());
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(_delay);
        _init = true;
    }

    private void Update()
    {
        if (_init)
        {
            if (_elapsedTime >= 1f)
            {
                transform.localScale = Vector3.one;
                _init = false;
                _running = true;
                _elapsedTime = 0f;
            }

            transform.localScale = Vector3.one * _elapsedTime;
            _elapsedTime += Time.deltaTime;
        }
        else if (_running)
        {
            if (_elapsedTime >= Mathf.PI * 2f)
            {
                _elapsedTime -= Mathf.PI * 2f;
            }

            transform.localScale = Vector3.one - Vector3.one * 0.15f * Mathf.Sin(_elapsedTime);
            _elapsedTime += Time.deltaTime * _speed;
        }
    }
}

using UnityEngine;

public class Scaler : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    private float _elapsedTime = 0f;


    private void Update()
    {
        if (_elapsedTime >= Mathf.PI * 2f)
        {
            _elapsedTime -= Mathf.PI * 2f;
        }

        transform.localScale = Vector3.one * 2f * Mathf.Sin(_elapsedTime);
        _elapsedTime += Time.deltaTime * _speed;
    }
}

using UnityEngine;

public class Rotater : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;

	private void Update ()
    {
        transform.rotation *= Quaternion.Euler(0f, _speed, 0f);
	}
}

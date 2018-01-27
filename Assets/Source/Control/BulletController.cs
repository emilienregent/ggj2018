using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed = 5.0f;
    public PlayerController originPlayer;

    // Use this for initialization
    private void Start () {
		
	}

    // Update is called once per frame
    private void FixedUpdate () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        Vector3 viewPos = originPlayer.playerCamera.WorldToViewportPoint(transform.position);
        if(viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
            Destroy(gameObject);
        }
    }

	private void OnTriggerEnter (Collider collider)
	{
		if (collider.gameObject.tag == "monster")
		{
			collider.gameObject.GetComponent<EnemyController>().HitByBullet(this);
				
			GameObject.Destroy (this.gameObject);
		}
	}
}

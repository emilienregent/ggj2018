using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

    public float speed = 5.0f;
    public Camera originCamera;
	public int currentDungeonId = 0;

    private GunController parentGun;

	public string hitTag = "";
	public float damage = 0f;
	public bool special = false;
    private Game _game = null;
    public bool isWarping = false;

    //public BulletController(GunController gun) {
    public void initParentGun(GunController gun) { 
        parentGun = gun;
    }

    // Use this for initialization
    private void Start () {
        _game = GameObject.FindObjectOfType<Game>();
    }

    // Update is called once per frame
    private void FixedUpdate () {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
		Vector3 viewPos = originCamera.WorldToViewportPoint(transform.position);
        if(viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1)
        {
			if (special == false)
			{
				parentGun.DisableBullet (this);
			}
			else
			{
				DirectionEnum direction = _game.GetDirection(currentDungeonId, transform.position);

                if (isWarping == true && direction == DirectionEnum.NONE)
                {
                    isWarping = false;
                }
                else if (isWarping == false && direction != DirectionEnum.NONE)
                {
                    if (_game.IsAvailableDirection(currentDungeonId, direction) == true)
                    {
                        int dungeonId = _game.GetDungeonId(currentDungeonId, direction);

                        Vector3 newPosition = _game.GetPosition(transform.position, direction, currentDungeonId, dungeonId);

                        transform.position = newPosition;

                        isWarping = true;
                    }
                    else
                    {
                        parentGun.DisableBullet(this);
                    }
				}
			}
        }
    }

	private void OnTriggerEnter (Collider collider)
	{
		if (collider.gameObject.tag == hitTag)
		{
			if (hitTag == "monster")
			{
				if (parentGun.gunFrequency == collider.GetComponent<EnemyController> ().frequency)
				{
					collider.gameObject.GetComponent<EnemyController> ().HitByBullet (this);
				}				
			}

			else if (hitTag == "player")
				collider.gameObject.GetComponent<PlayerController> ().Hit (damage);
			
			GameObject.Destroy (this.gameObject);
		}
	}
}

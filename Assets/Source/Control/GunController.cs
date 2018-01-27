using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool isFiring = false;

    public BulletController bullet;
    public float timeBetweenShot;
    private float shotCounter;

    private GameObject player;

    public Transform firePoint;


	// Use this for initialization
	void Start () {

        player = transform.root.gameObject;

    }
	
	// Update is called once per frame
	void Update () {
		if(isFiring)
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShot;
                BulletController newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                newBullet.originPlayer = player.GetComponent<PlayerController>();
            }
        } else
        {
            shotCounter = 0;
        }
	}
}

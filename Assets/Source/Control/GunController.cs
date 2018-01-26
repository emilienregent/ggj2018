using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public bool isFiring = false;

    public BulletController bullet;
    public float timeBetweenShot;
    private float shotCounter;

    public Transform firePoint;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isFiring)
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShot;
                Instantiate(bullet, firePoint.position, firePoint.rotation);
            }
        } else
        {
            shotCounter = 0;
        }
	}
}

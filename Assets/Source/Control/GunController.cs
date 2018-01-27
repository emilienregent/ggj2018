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

    public Stack<BulletController> inactiveBullets = new Stack<BulletController>();
    public List<BulletController> activeBullets = new List<BulletController>();
    public int startingBullets;

    // Use this for initialization
    private void Start () {

        player = transform.root.gameObject;

        for(int i = 0; i < startingBullets; i++)
        {
            BulletController newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            newBullet.initParentGun(this);
            newBullet.gameObject.SetActive(false);
            newBullet.name = player.name + "_bullet_" + i;
            inactiveBullets.Push(newBullet);
        }

    }

    // Update is called once per frame
    private void Update () {
		if(isFiring)
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShot;
                BulletController newBullet = ActiveBullet();
                 newBullet.originPlayer = player.GetComponent<PlayerController>();
            }
        } else
        {
            shotCounter = 0;
        }
	}

    public BulletController ActiveBullet() {
        BulletController newBullet = null;
        if(inactiveBullets.Count > 0)
        {
            newBullet = inactiveBullets.Pop();
            newBullet.gameObject.SetActive(true);
            newBullet.transform.position = firePoint.position;
            newBullet.transform.rotation = firePoint.rotation;
            activeBullets.Add(newBullet);
        } else
        {
            newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            newBullet.initParentGun(this);
            newBullet.name = player.name + "_bullet_" + activeBullets.Count;
            activeBullets.Add(newBullet);
        }

        return newBullet;
    }

    public void DisableBullet(BulletController bullet) {
        activeBullets.Remove(bullet);
        inactiveBullets.Push(bullet);
        bullet.gameObject.SetActive(false);
    }
}

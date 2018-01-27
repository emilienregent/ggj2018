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

	public Frequency gunFrequency;
	public Camera gunOriginCamera;

	public float bulletDamage = 0f;

    // Use this for initialization
    private void Start () {

        player = transform.root.gameObject;

        for(int i = 0; i < startingBullets; i++)
        {
            BulletController newBullet = InstantiateNewBullet(bullet, gunFrequency, firePoint.position, firePoint.rotation);

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
				Shoot ();
            }
        } else
        {
            shotCounter = 0;
        }
	}

	public void Shoot()
	{
		BulletController newBullet = ActiveBullet();
		newBullet.originCamera = gunOriginCamera;
	}

	public BulletController InstantiateNewBullet (BulletController prefab, Frequency frequency, Vector3 position, Quaternion rotation)
	{
		BulletController newBullet  = Instantiate(prefab, position, rotation);

		ColorBehaviour[] colors= newBullet.GetComponentsInChildren<ColorBehaviour> ();
		for (int j = 0; j < colors.Length; j++)
		{
			colors [j].SetFrequency (frequency);
		}

		newBullet.damage = bulletDamage;

		return newBullet;
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
			newBullet = InstantiateNewBullet(bullet, gunFrequency, firePoint.position, firePoint.rotation);
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

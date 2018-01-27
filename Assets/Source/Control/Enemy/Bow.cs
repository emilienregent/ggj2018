using UnityEngine;

public class Bow : Attack
{
	public override void Launch (EnemyController enemy, PlayerController player)
	{
		enemy.gun.isFiring = false;
		enemy.gun.Shoot ();
	}
}
using UnityEngine;

public class Sword : Attack
{
	public override void Launch (EnemyController enemy, PlayerController player)
	{
		player.Hit (enemy.dmg);
	}
}
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
	public abstract void Launch (EnemyController enemy, PlayerController player);
}
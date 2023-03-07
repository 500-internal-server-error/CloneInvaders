using UnityEngine;

public class EnemyController : MonoBehaviour {
	private GameController gameController;

	[SerializeField]
	private GameObject enemyLaserPrefab;

	[SerializeField]
	private GameObject explosionPrefab;

	[SerializeField]
	private GameObject powerupPrefab;

	[SerializeField, Range(0, 1)]
	private float powerupDropChance = 0.5f;

	private void Start() {
		gameController = GetComponentInParent<GameController>();
		if (gameController == null) {
			Debug.LogError("Cannot find GameController parent!");
		}
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "PlayerLaser") {
			GameObjectPool.instance.Instantiate(explosionPrefab, transform.position, Quaternion.identity);

			if (Random.value <= powerupDropChance) {
				GameObjectPool.instance.Instantiate(powerupPrefab, transform.position, Quaternion.identity);
			}

			Destroy(gameObject);
			GameObjectPool.instance.Destroy(collider.gameObject);
			gameController.OnEnemyDie(this);
		}
	}

	public void Shoot(float laserSpeed) {
		GameObject laser = GameObjectPool.instance.Instantiate(enemyLaserPrefab, transform.position, Quaternion.identity);
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -laserSpeed);
		GameObjectPool.instance.Destroy(laser, 2.0f);
	}
}

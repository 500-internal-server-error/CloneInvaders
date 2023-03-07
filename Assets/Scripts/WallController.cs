using UnityEngine;

public class WallController : MonoBehaviour {
	private GameController gameController;

	private void Start() {
		gameController = transform.parent.GetComponent<GameController>();

		if (gameController == null) {
			Debug.LogError("Parent is not a GameController!");
		}
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "Enemy") {
			gameController?.FlipEnemyMovementDirection();
		}
	}
}

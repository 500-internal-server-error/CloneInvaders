using UnityEngine;

public class Powerup : MonoBehaviour {
	[SerializeField, Min(0)]
	private float lifetime = 30.0f;

	private float lifetimeTimer = 0;

	[SerializeField]
	private float fallSpeed;

	private void Start() {
		lifetimeTimer = lifetime;
	}

	private void Update() {
		lifetimeTimer -= Time.deltaTime;
		if (lifetimeTimer <= 0) Destroy(gameObject);

		transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed * Time.deltaTime, 0);
	}
}

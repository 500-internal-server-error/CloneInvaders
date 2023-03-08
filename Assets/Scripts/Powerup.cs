using UnityEngine;

public class Powerup : SelfDestructableGameObject {
	[SerializeField, Min(0)]
	private float lifetime = 30.0f;

	private float lifetimeTimer = 0;

	[SerializeField]
	private float fallSpeed;

	private void Start() {
		lifetimeTimer = lifetime;
	}

	protected override void Update() {
		base.Update();

		lifetimeTimer -= Time.deltaTime;
		if (lifetimeTimer <= 0) Destroy(gameObject);

		transform.position = new Vector3(transform.position.x, transform.position.y - fallSpeed * Time.deltaTime, 0);
	}

	protected override void OnSelfDestruct() {
		gameObject.SetActive(false);
	}
}

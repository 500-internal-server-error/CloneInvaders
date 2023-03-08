using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
	[Header("Scene Management")]

	[SerializeField]
	private int menuSceneIndexInBuildSettings;

	[SerializeField]
	private float sceneChangeDelay = 1.2f;

	private float sceneChangeTimer = 0;
	private bool sceneChangeTimerActive = false;

	[Header("Player Movement")]

	[SerializeField]
	private new Rigidbody2D rigidbody;

	[SerializeField, Min(0)]
	private float speed = 1.5f;

	[SerializeField, Min(0)]
	private float horizontalLimit = 2.5f;

	[SerializeField]
	private PlayerInput playerInput;

	private InputAction moveAction;
	private InputAction shootAction;

	[Header("Player Shooting")]

	[SerializeField]
	private SelfDestructableGameObject playerLaserPrefab;

	[SerializeField]
	private float laserSpeed = 3.0f;

	[SerializeField, Min(0)]
	private float shootCooldown = 1.0f;

	private float shootCooldownTimer = 0;

	private bool hasPowerup = false;

	[SerializeField]
	private SelfDestructableGameObject explosionPrefab;

	[SerializeField]
	private AudioSource audioSource;

	private void Start() {
		moveAction = playerInput.actions["Move"];

		shootAction = playerInput.actions["Shoot"];
		shootAction.started += OnShootStarted;
	}

	private void FixedUpdate() {
		if (sceneChangeTimerActive) return;

		float moveDirection = moveAction.ReadValue<float>();
		rigidbody.velocity = new Vector2(moveDirection * speed, 0);

		if (rigidbody.position.x > horizontalLimit) {
			rigidbody.position = new Vector3(horizontalLimit, rigidbody.position.y, 0);
			rigidbody.velocity = Vector2.zero;
		}

		if (rigidbody.position.x < -horizontalLimit) {
			rigidbody.position = new Vector3(-horizontalLimit, rigidbody.position.y, 0);
			rigidbody.velocity = Vector2.zero;
		}
	}

	private void Update() {
		if (shootCooldownTimer > 0) shootCooldownTimer -= Time.deltaTime;

		if (sceneChangeTimerActive) {
			sceneChangeTimer -= Time.deltaTime;
			if (sceneChangeTimer <= 0) SceneManager.LoadScene(menuSceneIndexInBuildSettings);
		}
	}

	private void OnTriggerEnter2D(Collider2D collider) {
		if (collider.tag == "EnemyLaser" || collider.tag == "Enemy") {
			GameObjectPool.instance.Instantiate(explosionPrefab, transform.position, Quaternion.identity);

			if (!collider.TryGetComponent<LaserController>(out LaserController laser)) return;
			GameObjectPool.instance.Destroy(laser);

			if (hasPowerup) {
				hasPowerup = false;
			} else {
				Destroy(GetComponent<SpriteRenderer>());
				speed = 0;

				if (!sceneChangeTimerActive) {
					sceneChangeTimer = sceneChangeDelay;
					sceneChangeTimerActive = true;
				}
			}
		} else if (collider.tag == "Powerup") {
			hasPowerup = true;

			GameObjectPool.instance.Destroy(collider.GetComponent<Powerup>());
		}
	}

	private void OnShootStarted(InputAction.CallbackContext context) {
		if (shootCooldownTimer > 0) return;

		GameObject laserCenter = GameObjectPool.instance.Instantiate(playerLaserPrefab, transform.position, Quaternion.identity);
		laserCenter.GetComponent<Rigidbody2D>().velocity = new Vector2(0, laserSpeed);
		GameObjectPool.instance.Destroy(laserCenter.GetComponent<LaserController>(), 2.0f);


		if (hasPowerup) {
			GameObject laserLeft = GameObjectPool.instance.Instantiate(playerLaserPrefab, transform.position, Quaternion.identity);
			laserLeft.transform.rotation = Quaternion.Euler(0, 0, 30.0f);
			laserLeft.GetComponent<Rigidbody2D>().velocity = laserLeft.transform.rotation * new Vector2(0, laserSpeed);
			GameObjectPool.instance.Destroy(laserLeft.GetComponent<LaserController>(), 2.0f);

			GameObject laserRight = GameObjectPool.instance.Instantiate(playerLaserPrefab, transform.position, Quaternion.identity);
			laserRight.transform.rotation = Quaternion.Euler(0, 0, -30.0f);
			laserRight.GetComponent<Rigidbody2D>().velocity = laserRight.transform.rotation * new Vector2(0, laserSpeed);
			GameObjectPool.instance.Destroy(laserRight.GetComponent<LaserController>(), 2.0f);
		}

		audioSource.PlayOneShot(audioSource.clip);

		shootCooldownTimer = shootCooldown;
	}
}

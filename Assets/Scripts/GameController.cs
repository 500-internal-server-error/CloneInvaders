using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {
	[Header("Scene Management")]

	[SerializeField]
	private int menuSceneIndexInBuildSettings;

	[SerializeField]
	private float sceneChangeDelay = 1.2f;

	private float sceneChangeTimer = 0;
	private bool sceneChangeTimerActive = false;

	[Header("Enemy Shooting")]

	[SerializeField]
	private float laserSpeed = 2.0f;

	[SerializeField]
	private float shootCooldown = 3.0f;

	private float shootCooldownTimer;

	[Header("Enemy Movement")]

	[SerializeField]
	private GameObject enemyContainer;

	[SerializeField]
	private float enemyHorizontalMovementSpeed = 0.25f;

	[SerializeField]
	private float enemyVerticalMovementSpeed = 0.02f;

	[SerializeField]
	private int moveDirection = 1;
	private Vector2 targetPosition;

	[SerializeField]
	private TextMeshProUGUI tmp;

	[SerializeField]
	private int score = 0;

	private void Start() {
		shootCooldownTimer = shootCooldown;

		ResizeEnemyContainer();
	}

	private void Update() {
		shootCooldownTimer -= Time.deltaTime;

		if (sceneChangeTimerActive) {
			sceneChangeTimer -= Time.deltaTime;
			if (sceneChangeTimer <= 0) SceneManager.LoadScene(menuSceneIndexInBuildSettings);
		}

		EnemyController[] enemies = GetComponentsInChildren<EnemyController>();

		if (enemies.Length > 0 && shootCooldownTimer <= 0) {
			enemies[Random.Range(0, enemies.Length)].Shoot(laserSpeed);
			shootCooldownTimer = shootCooldown;
		} else if (enemies.Length <= 0) {
			if (!sceneChangeTimerActive) {
				sceneChangeTimer = sceneChangeDelay;
				sceneChangeTimerActive = true;
			}
		}

		enemyContainer.transform.position += moveDirection * Vector3.right * enemyHorizontalMovementSpeed * Time.deltaTime;

		tmp.SetText("Score: " + score);
	}

	private void ResizeEnemyContainer() {
		EnemyController[] enemies = enemyContainer.GetComponentsInChildren<EnemyController>();

		float minPointX = Mathf.Infinity;
		float minPointY = Mathf.Infinity;

		float maxPointX = Mathf.NegativeInfinity;
		float maxPointY = Mathf.NegativeInfinity;

		foreach (EnemyController enemy in enemies) {
			SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();

			minPointX = Mathf.Min(minPointX, enemySpriteRenderer.bounds.min.x);
			minPointY = Mathf.Min(minPointY, enemySpriteRenderer.bounds.min.y);

			maxPointX = Mathf.Max(maxPointX, enemySpriteRenderer.bounds.max.x);
			maxPointY = Mathf.Max(maxPointY, enemySpriteRenderer.bounds.max.y);
		}

		Vector2 minPoint = new Vector2(minPointX, minPointY);
		Vector2 maxPoint = new Vector2(maxPointX, maxPointY);

		enemyContainer.GetComponent<BoxCollider2D>().size = maxPoint - minPoint;

		Debug.Log("min: " + minPoint);
		Debug.Log("max: " + maxPoint);
		Debug.Log("delta: " + (maxPoint - minPoint));
		Debug.Log("count: " + enemies.Length);
	}

	public void FlipEnemyMovementDirection() {
		Debug.Log("flip");
		moveDirection *= -1;
		enemyContainer.transform.position += Vector3.down * enemyVerticalMovementSpeed;
	}

	public void OnEnemyDie() {
		score++;
		ResizeEnemyContainer();
	}
}

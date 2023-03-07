using System.Collections.Generic;
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

	[Header("Score Management")]

	[SerializeField]
	private TextMeshProUGUI tmp;

	private int score = 0;

	List<EnemyController> enemies = new List<EnemyController>();

	private void Start() {
		shootCooldownTimer = shootCooldown;

		foreach (EnemyController enemy in GetComponentsInChildren<EnemyController>()) {
			enemies.Add(enemy);
		}

		ResizeEnemyContainer();
	}

	private void Update() {
		shootCooldownTimer -= Time.deltaTime;

		if (sceneChangeTimerActive) {
			sceneChangeTimer -= Time.deltaTime;
			if (sceneChangeTimer <= 0) SceneManager.LoadScene(menuSceneIndexInBuildSettings);
		}

		if (enemies.Count > 0 && shootCooldownTimer <= 0) {
			enemies[Random.Range(0, enemies.Count)].Shoot(laserSpeed);
			shootCooldownTimer = shootCooldown;
		} else if (enemies.Count <= 0) {
			if (!sceneChangeTimerActive) {
				sceneChangeTimer = sceneChangeDelay;
				sceneChangeTimerActive = true;
			}
		}

		enemyContainer.transform.position += moveDirection * Vector3.right * enemyHorizontalMovementSpeed * Time.deltaTime;

		tmp.SetText("Score: " + score);
	}

	private void ResizeEnemyContainer() {
		Bounds bounds = new Bounds();
		foreach (EnemyController enemy in enemies) bounds.Encapsulate(enemy.GetComponent<BoxCollider2D>().bounds);
		// if (enemies.Count > 0) {
		// 	bounds = enemies[0].GetComponent<BoxCollider2D>().bounds;
		// 	for (int i = 1; i < enemies.Count; i++) {
		// 		bounds.Encapsulate(enemies[i].GetComponent<BoxCollider2D>().bounds);
		// 	}
        // }

        enemyContainer.GetComponent<BoxCollider2D>().size = bounds.size;
        enemyContainer.GetComponent<BoxCollider2D>().offset = bounds.center - enemyContainer.transform.position;
		// EnemyController[] enemies = enemyContainer.GetComponentsInChildren<EnemyController>();

		// float minPointX = Mathf.Infinity;
		// float minPointY = Mathf.Infinity;

		// float maxPointX = Mathf.NegativeInfinity;
		// float maxPointY = Mathf.NegativeInfinity;

		// foreach (EnemyController enemy in enemies) {
		// 	SpriteRenderer enemySpriteRenderer = enemy.GetComponent<SpriteRenderer>();

		// 	minPointX = Mathf.Min(minPointX, enemySpriteRenderer.bounds.min.x);
		// 	minPointY = Mathf.Min(minPointY, enemySpriteRenderer.bounds.min.y);

		// 	maxPointX = Mathf.Max(maxPointX, enemySpriteRenderer.bounds.max.x);
		// 	maxPointY = Mathf.Max(maxPointY, enemySpriteRenderer.bounds.max.y);
		// }

		// Vector2 minPoint = new Vector2(minPointX, minPointY);
		// Vector2 maxPoint = new Vector2(maxPointX, maxPointY);

		// enemyContainer.GetComponent<BoxCollider2D>().size = maxPoint - minPoint;

		// Debug.Log("min: " + minPoint);
		// Debug.Log("max: " + maxPoint);
		// Debug.Log("delta: " + (maxPoint - minPoint));
		// Debug.Log("count: " + enemies.Length);
	}

	public void FlipEnemyMovementDirection() {
		moveDirection *= -1;
		enemyContainer.transform.position += Vector3.down * enemyVerticalMovementSpeed;
	}

	public void OnEnemyDie(EnemyController enemy) {
		score++;
		enemies.Remove(enemy);
		ResizeEnemyContainer();
	}
}

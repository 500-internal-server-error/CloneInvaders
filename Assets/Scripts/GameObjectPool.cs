using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool : MonoBehaviour {
	public static GameObjectPool instance { get { return _instance; } }
	private static GameObjectPool _instance;

	private static Dictionary<GameObject, float> destroyTimers = new Dictionary<GameObject, float>();

	private void Start() {
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.LogWarning("Attempted to instantiate multiple instances of singleton GameObjectPool, self-destructing.");
			Object.Destroy(this);
		}
	}

	private void Update() {
		List<GameObject> expiredTimers = new List<GameObject>();
		List<GameObject> unexpiredTimers = new List<GameObject>();

		foreach (KeyValuePair<GameObject, float> kvp in destroyTimers) {
			if (kvp.Key == null) continue;

			if (kvp.Value <= 0) {
				expiredTimers.Add(kvp.Key);
			} else {
				unexpiredTimers.Add(kvp.Key);
			}
		}

		foreach (GameObject expiredTimer in expiredTimers) {
			expiredTimer.SetActive(false);
			destroyTimers.Remove(expiredTimer);
		}

		foreach (GameObject unexpiredTimer in unexpiredTimers) {
			destroyTimers[unexpiredTimer] -= Time.deltaTime;
		}
	}

	public GameObject Instantiate(GameObject original, Vector3 position, Quaternion rotation) {
		Transform container = transform.Find(original.tag);

		if (container == null) {
			GameObject newContainer = new GameObject(original.tag);
			container = newContainer.transform;
			container.SetParent(transform);
		}

		foreach (Transform gameObjectTransform in container) {
			if (!gameObjectTransform.gameObject.activeInHierarchy) {
				gameObjectTransform.gameObject.SetActive(true);

				gameObjectTransform.gameObject.transform.position = position;
				gameObjectTransform.gameObject.transform.rotation = rotation;

				return gameObjectTransform.gameObject;
			}
		}

		GameObject newObject = Object.Instantiate(original, position, rotation);
		newObject.transform.SetParent(container);

		return newObject;
	}

	public void Destroy(GameObject obj) {
		Destroy(obj, 0);
	}

	public void Destroy(GameObject obj, float t) {
		Transform container = transform.Find(obj.tag);

		if (container == null) {
			Debug.LogWarning("Attempted to destroy a non-pooled GameObject!");
			return;
		}

		destroyTimers[obj] = t;
	}
}

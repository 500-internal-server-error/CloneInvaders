using UnityEngine;

public class GameObjectPool : MonoBehaviour {
	public static GameObjectPool instance { get { return _instance; } }
	private static GameObjectPool _instance;

	private void Start() {
		if (_instance == null) {
			_instance = this;
		} else {
			Debug.LogWarning("Attempted to instantiate multiple instances of singleton GameObjectPool, self-destructing.");
			Object.Destroy(this);
		}
	}

	public GameObject Instantiate(SelfDestructableGameObject original, Vector3 position, Quaternion rotation) {
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

		GameObject newObject = Object.Instantiate(original.gameObject, position, rotation);
		newObject.transform.SetParent(container);

		return newObject;
	}

	public void Destroy(SelfDestructableGameObject obj, float t = 0) {
		Transform container = transform.Find(obj.tag);

		if (container == null) {
			Debug.LogWarning("Attempted to destroy a non-pooled GameObject!");
			return;
		}

		obj.SelfDestruct(t);
	}
}

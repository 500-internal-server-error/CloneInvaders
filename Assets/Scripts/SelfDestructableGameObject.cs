using UnityEngine;

public abstract class SelfDestructableGameObject : MonoBehaviour {
	protected float selfDestructTimer = 0;
	protected bool selfDestructTimerActive = false;

	protected virtual void Update() {
		if (selfDestructTimerActive) {
			selfDestructTimer -= Time.deltaTime;
			if (selfDestructTimer <= 0) OnSelfDestruct();
		}
	}

	protected abstract void OnSelfDestruct();

	public void SelfDestruct(float delay = 0) {
		selfDestructTimerActive = true;
		selfDestructTimer = delay;
	}
}

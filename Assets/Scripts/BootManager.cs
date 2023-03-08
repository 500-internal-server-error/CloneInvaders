using UnityEngine;
using UnityEngine.SceneManagement;

public class BootManager : MonoBehaviour {
	[SerializeField]
	private int menuSceneIndexInBuildSettings;

	private void Start() {
		SceneManager.LoadScene(menuSceneIndexInBuildSettings);
	}
}

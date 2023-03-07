using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPlayButtonController : MonoBehaviour {
	[SerializeField]
	private int gameSceneIndexInBuildSettings;

	private void Start() {
		Button button = GetComponent<Button>();
		button.onClick.AddListener(ButtonOnClick);
	}

	private void ButtonOnClick() {
		SceneManager.LoadScene(gameSceneIndexInBuildSettings);
	}
}

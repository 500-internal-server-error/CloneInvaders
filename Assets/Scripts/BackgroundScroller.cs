using UnityEngine;

public class BackgroundScroller : MonoBehaviour {
	[SerializeField, Range(-1f, 1f)]
	private float scrollSpeed = 0.5f;

	private float offset;

	private Material material;

	private void Start() {
		material = GetComponent<Renderer>().material;
		DontDestroyOnLoad(this);
	}

	private void Update() {
		offset -= (scrollSpeed * Time.deltaTime) / 10.0f;
		material.SetTextureOffset("_BaseMap", new Vector2(0, offset));
	}
}

public class LaserController : SelfDestructableGameObject {
	protected override void OnSelfDestruct() {
		gameObject.SetActive(false);
	}
}

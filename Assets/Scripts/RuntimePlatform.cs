using UnityEngine;

public class RuntimePlatform : MonoBehaviour {

	// Vai verificar qual plataforma, se não for mobile,
	//desativa os botões de controle do dragão
	void Start () {
		gameObject.SetActive (Application.isMobilePlatform);
	}
	
}

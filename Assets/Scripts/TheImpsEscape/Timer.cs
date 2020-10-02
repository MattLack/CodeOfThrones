using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Timer : MonoBehaviour {

	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//public fields
	public float startTime;
	public Tyrion tyrion;
	public int score;

	//private fields
	//o tipo Text só é possível encontrar caso o namespace UnityEngine.UI esteja sendo usado no script
	Text timer;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/

	void Start(){
		//get component vai buscar o objeto dentro do gameobject (se não encontrar, retorna nulo)
		timer = GetComponent<Text> ();
	}

	// Update is called once per frame
	void Update () {
		//se o tempo inicial for maior ou igual que zero, então...
		if (startTime >= 0f) {
			//ele vai ser reduzido de acordo com o deltaTime (diferença de tempo entre os updates)
			//ou seja, vai sendo reduzido de acordo com o tempo real, o timer receberá em sua variável
			//text (string) o arredondamento para cima do startTime que foi reduzido a pouco
			startTime -= Time.deltaTime;
			timer.text = Mathf.CeilToInt (startTime).ToString ();

			//score receberá quanto tempo resta multiplicado por 25... ou seja, quanto mais tempo tiver, mais pontos o jogador terá
			score = Mathf.CeilToInt(startTime) * 25;
		} else {
			//caso o tempo inicial for menor que zero, então o tyrion deixará de estar livre (isFree = false)
			//que irá impedir que a interação do jogador como  tyrion... e o método gameOver será chamado
			tyrion.isFree = false;
			tyrion.GameOver ();
		}
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayPause : MonoBehaviour {

	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 

	//para utilizar o Image é necessário usar o namespace UnityEngine.UI
	Image image;
	public Sprite play, pause;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/
	void Start(){
		//procura, 
		image = GetComponent<Image> ();
	}

	/************************************************************
	************************   METHODS   ************************
	************************************************************/
	//método para alternar o estado de play/pause do jogo
	public void PausePlay() {

		//se o tempo do jogo estiver correndo normal (timeScale = 1)
		//então trava o fluxo do jogo (timeScale = 0), altera a sprite
		//do jogo botão play/pause para evidenciar que a próxima vez
		//que pressionar o botão o jogo irá para o estado play
		//e também pausa todo o som do jogo (AudioListener)
		if (Time.timeScale == 1f) {
			Time.timeScale = 0f;
			image.sprite = play;
			AudioListener.pause = true;
		} else {
			//caso o jogo já esteja pausado, então volta o fluxo de
			//tepo do jogo ao normal (timeScale = 1)
			Time.timeScale = 1f;

			//o sprite do botão volta a identificar
			//que quando pressionar ele irá pausar o jogo
			image.sprite = pause;

			//o audio do jogo irá despausar
			AudioListener.pause = false;
		}
	}

}

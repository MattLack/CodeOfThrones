using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour {
	
	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	public Fade fade;
	AudioSource myAudioSource;


	/************************************************************
	******************   GETTERS AND SETTERS   ******************
	************************************************************/
	//usado no canvas, na cena de menu, serve para preencher o audio
	public AudioSource MyAudioSource {
		set { this.myAudioSource = value; }
	}

	/************************************************************
	************************   METHODS   ************************
	************************************************************/
	//este método recebe uma string com o nome da proxima cena e a repassa para a "coroutine"
	public void ChangeSceneAfterAudiosEnd(string scene){
		StartCoroutine (ChangeSceneWhenStopPlay(scene));
	}

	//este metodo recebe uma string com o nome da próxima cena e já muda
	public void ChangeScene(string scene){
		fade.ChangeScene (scene);
		//não tem o FadeOutChangeScene = true;
	}

	//na coroutine, ele altera o nome dá próxima cena, no fade, para o valor recebido
	//espera o audio chegar ao seu fim e, então, ativa a variavel bool responsável para
	// "ganhar opacidade" (no Fade, ele ganha opacidade ao máximo e, então, muda de cena.
	IEnumerator ChangeSceneWhenStopPlay(string scene){
		fade.nextScene = scene;
		while (myAudioSource != null && myAudioSource.isPlaying && !AudioListener.pause) {
			yield return new WaitForSeconds (0.1f);
		}
		fade.podeGanharOpacidade = true;
		fade.gettingOpaque = true;
	}

}

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour {

	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//private variables
	Color tempColor;

	//public variables
	public bool podeGanharOpacidade = false;
	public bool FadeInChangeScene = false;
	[Tooltip("Velocidade com que o acontece o fade in/out")] //este "Atributo" serve para apresentar uma mensagem no Editor
	public float fadeTax = 3f;
	public bool FadeOutChangeScene = false;
	public string nextScene;
	public Image screenToFade;
	public bool gettingOpaque;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/
	//setando valores default e guardando o componente da imagem na variavel
	void Awake(){
		screenToFade.gameObject.SetActive (true);
		gettingOpaque = screenToFade.color.a < 0.5f;
	}

	// Update is called once per frame
	void Update () {
		//FadeIn == ganhar transparência
		//FadeOut == restaurar a opacidade
		//se NÃO poder fazer FadeOut OU se (pode fazer fadeOut E não está ganhando opacidade no momento)
		//então faz fade in, caso contrário, faz fadeout

		if (!podeGanharOpacidade || (podeGanharOpacidade && !gettingOpaque)) {
			GanhaTransparencia ();	
		} else {
			GanhaOpacidade ();
		}
	}

	/************************************************************
	************************   METHODS   ************************
	************************************************************/
	//Fade In == Ganhar transparência
	void GanhaTransparencia() {
		
		//aos poucos dá transparencia à imagem
		screenToFade.color = Color.Lerp (screenToFade.color, Color.clear, Time.deltaTime * fadeTax);
	
		//caso o alfa tenha atingido o valor 0 e a variavel "FadeInChangeScene" for verdadeira
		//então a cena será alterada
		//caso seja falso, inverte o fade in/out
		if (screenToFade.color.a <= 0.05f) {
			if (FadeInChangeScene) {
				SceneManager.LoadScene (nextScene);
			} else {
				gettingOpaque = true;
			}
		}
	}// end fade in

	//Fade Out == Ganhar opacidade
	void GanhaOpacidade() {
		
		//aos poucos dá opacidade à imagem
		screenToFade.color = Color.Lerp (screenToFade.color, Color.black, Time.deltaTime * fadeTax);


		//se o alpha atingir 1, então chegou ao limite máximo
		//e, se, o FadeOutChangeScene for true, então ele muda para a próxima cena
		//caso seja falso, inverte o fade in/out
		if (screenToFade.color.a >= 0.95f) {
			if (FadeOutChangeScene) {
				SceneManager.LoadScene (nextScene);
			} else {
				gettingOpaque = false;
			}
		}
	}//end fade out

	public void ChangeScene(string scene) {
		nextScene = scene;
		FadeOutChangeScene = true;
		podeGanharOpacidade = true;
		gettingOpaque = true;
	}
}
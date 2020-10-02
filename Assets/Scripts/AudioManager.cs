using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/  
	//private variables
	AudioSource mainSound; //"player" de audio

	//public variables
	public Text audioText; //mostra se o audio está ON ou OFF
	public AudioClip theme, dracarys, theNorthRemembers, theImpScape, gameOver; //full soundtrack

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/
	//metodo chamado quando o objeto é instanciado, funciona semelhante a um construtor
	void Awake(){
		mainSound = GetComponent<AudioSource> ();

		//impede que o "GameObject" que armazena este script seja destruido quando mudar de cena
		//observação: este método não está obsoleto!
		DontDestroyOnLoad (gameObject); 

		//aqui é adicionado o método LoadNewScene para ser chamado a cada vez
		//que uma cena for carregada
		//
		///OBSERVAÇÃO: aqui é armazenado o delegate que substitui uma mensagem
		//             obsoleta.
		//
		//ATENÇÃO:  mensagens do Monobehaviour (herança aplicada aos scripts)
		//			basicamente (no caso do unity) são métodos que serão chamados
		//			pelo motor do jogo (nesse caso, o unity) para executar
		//			alguma ação específica, como, por exemplo o Update que é chamado
		//			sempre e serve para manter o fluxo do game.
		//			neste caso, o delegate está substituindo a mensagem OnLevelWasLoaded
		//			que seria executado cada vez que uma cena for carregada
		//			(uma espécie de Start, porém não será chamada quando o objeto for inicializado
		//			mas quando uma cena for carregada)
		SceneManager.sceneLoaded += LoadNewScene;
	}

	void Start () {
		//verifica o estado atual do audio no jogo, se ele está ligado ou desligado
		//1 = ligado, 0 = desligado....... em seguida atualiza o "Text" que mostra o audio
		if (PlayerPrefs.HasKey ("AudioStatus")) {
			AudioListener.pause = PlayerPrefs.GetInt ("AudioStatus") == 1;
		} else {
			AudioListener.pause = false;
		}
		UpdateAudioText ();
	}

	//esse metodo é chamado quando uma nova cena é carregada
	//de acordo com o que foi colocado lá no Awake, este
	//é o método que será delegado quando o objeto for criado
	void LoadNewScene (Scene scene, LoadSceneMode mode){

		//switch para verificar qual a cena atual para associar
		//um soundtrack diferente para cada cena
		switch (SceneManager.GetActiveScene ().name) {
		case "Start":
			mainSound.clip = theme;
			break;
		case "Dracarys":
			mainSound.clip = dracarys;
			break;
		case "TheNorthRemembers":
			mainSound.clip = theNorthRemembers;
			break;
		case "TheImpsEscape":
			mainSound.clip = theImpScape;
			break;
		case "GameOver":
			mainSound.clip = gameOver;
			break;

		}

		//caso o audio esteja tocando, não vai chamar o play para não
		//reiniciar o audio, e apenas deixa continuar tocando
		if(!mainSound.isPlaying)
			mainSound.Play ();
	}

	/************************************************************
	************************   METHODS   ************************
	************************************************************/
	//altera o estado do audio "ON" e "OFF"
	public void ChangeAudioStatus(){
		//inverte o boolean do AudioListener e em seguida, salva seu novo estado
		//como ligado (1) ou desligado (0) e, em seguida, atualiza o texto do audio
		AudioListener.pause = !AudioListener.pause;
		PlayerPrefs.SetInt ("AudioStatus", AudioListener.pause ? 1 : 0);
		PlayerPrefs.Save ();
		UpdateAudioText ();
	}

	//atualiza o texto do audio
	public void UpdateAudioText(){
		//primeiro verifica se o texto existe (nem todas as cenas tem esse texto)
		//depois atualiza seus valores para "on" ou "off" de acordo se ele está
		//ligado ou não
		if (audioText != null) {
			audioText.text = "Audio: " + (AudioListener.pause ? "Off" : "On");
		}
	}
}

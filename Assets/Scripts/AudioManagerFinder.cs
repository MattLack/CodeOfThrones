using UnityEngine;
using UnityEngine.UI;

public class AudioManagerFinder : MonoBehaviour {

	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//private variables
	AudioManager audioManager;
	Text audioText;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/
	public void Start () {
		//busca o Text do audio entre os filhos (hierarchy)
		audioText = GetComponentInChildren<Text>();

		//busca um objeto do tipo AudiOManager na cena inteira
		//como ele "nasceu" na cena anterior não tem como "encontrá-lo"
		//de forma mais simples
		audioManager = GameObject.FindObjectOfType<AudioManager>();

		//preenche o audio manager com o text e, em seguida, o atualiza
		audioManager.audioText = audioText;
		audioManager.UpdateAudioText();
	}
	
	/************************************************************
	************************   METHODS   ************************
	************************************************************/
	//apenas um método intermediário, como o AudioManager nao estava
	//na cena em sua construção, este é o "meio" utilizado para
	//acessa-lo "in game".
	public void ChangeAudioStatus () {
		audioManager.ChangeAudioStatus();
	}
}

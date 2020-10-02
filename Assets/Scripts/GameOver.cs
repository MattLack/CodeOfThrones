using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour {

	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	public Text yourScore, highScore, record;

	/************************************************************
*****************   MONOBEHAVIOUR MESSAGES   ****************	************************************************************/
	void Start () {
		//torna o text do Record falso por default
		record.gameObject.SetActive(false);

		//vai buscar no player prefs o valor do Score
		int playerScore = PlayerPrefs.GetInt ("Score");
		yourScore.text = playerScore.ToString();

		//busca no playerprefs o record atual
		int currentRecord = PlayerPrefs.GetInt (PlayerPrefs.GetString ("LastGame") + "HighScore");

		//se a pontuação do jogador for maior que o recorde atual
		//então o recorde atual passa a ser o mesmo valor da pontuação do jogador
		//o novo recorde é gravado no playerprefs e a mensagem de novo record é ativada
		if (playerScore > currentRecord) {
			currentRecord = playerScore;
			PlayerPrefs.SetInt (PlayerPrefs.GetString ("LastGame") + "HighScore", currentRecord);
			PlayerPrefs.Save ();
			record.gameObject.SetActive (true);
		}

		//apresentar o valor do recorede (que pode ser a sua pontuação)
		highScore.text = currentRecord.ToString();
	}

}

using UnityEngine;
using System.Collections;

public class Wall : MonoBehaviour {
	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//public fields
	public Transform playerTransform;

	//este pivot é a representação de qual altura o jogador precisa passar para ser considerado a frente/abaixo do objeto
	public Transform pivot; 

	//private field
	//objeto responsável pela renderização de um sprite
	SpriteRenderer sprite;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/

	//o getcomponent vai buscar no próprio game object um objeto do tipo especificado
	//caso não encontre, retorna null
	void Start(){
		sprite = GetComponent<SpriteRenderer> ();
	}


	// Update is called once per frame
	void Update () {
		//operador ternário, se a altura do pivo for maior que a altura do player, então o player estará "abaixo" do objeto
		//e o objeto estará na layer background (renderizada atrás do player), causando a impressão visual de que o player estar na frente do objeto
		//caso a "pergunta" seja falsa, então o objeto estará na layer "Foreground" que sera renderizada na frente do personagem do jogador
		//e isso dará a ideia de que o personagem do jogador estará por trás do objeto
		string slayer = (pivot.position.y > playerTransform.position.y) ? "Background" : "Foreground";

		//com a layer definida, ela é passada para o renderizador de spirtes desenhar os objetos da forma mais adequada
		sprite.sortingLayerName = slayer;
	}
}

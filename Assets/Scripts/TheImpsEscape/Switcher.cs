using UnityEngine;
using System.Collections;

public class Switcher : MonoBehaviour {
	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//public fields
	public SpriteRenderer wall; //este objeto é responsável por renderizar o sprite na tela
	public GameObject door;

	//private fields
	Animator anim; //este objeto é o responsável por gerenciar as transições das animações
	bool active;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/

	//o getcomponent vai buscar o objeto do tipo especificado dentro do próprio gameobject
	void Start () {
		active = false;
		anim = GetComponent<Animator> ();
	}

	//aqui é onde é alterada a situação do switcher, em um nível de protótipo
	void Switch(){
		active = !active; //quanto o método é chamado ele inverte o fato de estar ativo ou não (inicia com o switch desativado -- ver no metodo Start)
		anim.SetBool ("Active",active); //então, o resultado da ativação (ou desativação) do switch é repassado para o animator que irá gerenciar qual animação usar
		door.SetActive (!active); //a porta será ativada ou desativada de acordo com a situação do inversa do switcher (se ele estvier desativo, a porta está ativa!)

		//como o muro todo não pode desaparecer, coloquei bem exagerado aqui para facilitar o entendimento...
		//se o switcher estiver ativo, a ordem de renderização na mesma layer será 80 (ou seja, atrás de outros sprites e a passagem ficará na frente, ou seja...
		//a passagem ficará "livre", caso o switcher esteja desativado, então a porta estará ativada com a ordem da layer em 800, ou seja, na frente e visível 
		//para o jogador...
		wall.sortingOrder = active ? 80 : 800; 

	}
}

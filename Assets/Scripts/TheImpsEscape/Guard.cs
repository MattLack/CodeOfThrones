using UnityEngine;
using System.Collections;

public class Guard : MonoBehaviour {
	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//public fields
	public float watchTime,colorTime;
	//objeto responsável por renderizar um sprite
	public SpriteRenderer watchViewSprite;

	//private fields
	Animator anim;
	Color color;
	bool isLookingDown;
	bool seeingPlayer;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/
	// Use this for initialization
	void Start () {
		seeingPlayer = false;
		isLookingDown = false;
		//getcomponent vai buscar o objeto específico dentro do próprio gameobject
		anim = GetComponent<Animator> ();

		//o método "changewatchside" (parametro1) será chamado em "watchTime" (parametro2)
		//segundos e após isso será chamado novamente a cada "watchtime" (parametro3) segundos 
		InvokeRepeating ("ChangeWatchSide", watchTime, watchTime);

		//armazena o filtro de cor original (o default é branco, mas nesse caso foi ajustado no inspector para ser azul)
		color = watchViewSprite.color;
	}

	void Update(){
		//se não estiver vendo o personagem do jogador e a cor da area de visão for diferente da cor original....
		if (!seeingPlayer && watchViewSprite.color != color) {
			//então a cor irá mudar aos poucos da "cor atual" até a "cor original" de acordo com a variável
			//a variavel colorTime (ajustada no inspector)
			watchViewSprite.color = Color.Lerp (watchViewSprite.color, color, colorTime);
		}
	}

	//se um colisor trigger for detectado e for o player, então a variável seeingPlayer será verdadeira
	//(detectou que o player está a vista)
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			seeingPlayer = true;
		}
	}

	//se tiver um colisor passando na área de vista, e for o player
	//eñtão a cor da area de visão vai mudando aos poucos para vermelho de acordo com a variavel colorTime
	void OnTriggerStay2D (Collider2D coll){
		if (coll.gameObject.CompareTag ("Player")) {
			watchViewSprite.color = Color.Lerp (watchViewSprite.color, Color.red, colorTime);

			//se a cor estiver em 80% do vermelho, envia uma mensagem
			//para o gameObject do player para buscar o método Surrender e ativá-lo
			//depois a animação do guarda é parada
			if (watchViewSprite.color.r > 0.8f) {
				coll.gameObject.SendMessage ("Surrender");
				anim.enabled = false;
			}
		}
	}

	//se um colisor sair da área de visão E for um player E a cor so prite for menor que 80% do vermelho,
	//então a variavel seeingPlayer recebe false, ou seja, o jogador não está mais sendo visto pelo guarda...
	void OnTriggerExit2D(Collider2D coll){
		if (coll.gameObject.CompareTag ("Player") && watchViewSprite.color.r <= 0.8f) {
			seeingPlayer = false;
		}
	}

	/************************************************************
	************************   METHODS   ************************
	************************************************************/
	//altera o lado em que o guarda está vigiando, varia entre olhar pra baixo e olhar para a esquerda
	//tem um guarda q olha para a direita, é pq a escola (esticar ou encolher) no eixo y está invertida
	//(virou o guarda pelo avesso, hehe, invertendo a escala do eixo x, ele foi completamente invertido
	//horizontalmente)
	void ChangeWatchSide(){
		isLookingDown = !isLookingDown;
		anim.SetBool("isLookingDown",isLookingDown);
	}

}

using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class Tyrion : MonoBehaviour {
	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//public fields
	public float speed;
	public float handReachOffset;
	public LayerMask interactableLayer; //pelo inspector é selecionada a layer dos objetos que poderão ser afetadas pelo personagem
	public Fade fade; //controlador da imagem que escurece ou clareia quando muda de cena
	public Timer timer; //controlador do tempo do jogo
	public GameObject switcher; //interruptor que irá permitirá que a porta seja aberta para poder "terminar" o game

	[HideInInspector]//atributo para permitir que uma variável seja pública e que não apareça no inspector
	public bool isFree;//variável para saber se o tyrion foi, ou não, capturado após sua fuga...

	//private fields
	Rigidbody2D rb;
	Animator anim;
	float originalXScale;
	int currentDirection;
	Vector3 referencePoint;
	Vector3 direction;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	*************************************************************/
	//inicialização das variáveis, o switcher inicia como nulo por default
	void Start () {
		switcher = null;

		/*	DIREÇÃO
		 * 1. Olhando para o norte (de costas para a camera)
		 * 2. Olhando para o leste
		 * -2. Olhando para o oeste
		 * 3. Olhando para o sul (de frente para a camera)
		 */
		currentDirection = 3;

		//get component vai buscar o objeto dentro do próprio gameobject (caso nao encontre, retorna null)
		rb = GetComponent<Rigidbody2D> ();
		isFree = true;

		//get component vai buscar o objeto dentro dos gameobjects filhos (na hierarchy do unity). Caso nao encontre, retorna null
		anim = GetComponentInChildren<Animator> ();

		//armazena a escala horizontal inicial
		originalXScale = transform.localScale.x;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//se o tyrion estiver free...
		if (isFree) {
			Vector2 movement = rb.position; //armazena a posição atual do personagem

			//pega os eixos horizontais (seta esquerda e direita, analógicos de joystick para esquerda e direita,
			//analógicos posto na tela para a build para android para esquerda e direita) que retonram valores
			//entre -1 e 1 (esquerda direita) ajusta a uma velocidade e armazena em uma variável
			float x = CrossPlatformInputManager.GetAxis ("Horizontal") * speed * Time.fixedDeltaTime;

			//o mesmo da situação anterior, porém para o eixo vertical
			//(setas para cima e para baixo, analógicos para cima e para baixo)
			float y = CrossPlatformInputManager.GetAxis ("Vertical") * speed * Time.fixedDeltaTime;

			//ajusta, na variavel que armazena a posição atual, o valor dos eixos verificados acima
			//e atualiza a posição atual do personagem
			movement.x += x;
			movement.y += y;
			rb.MovePosition (movement);

			//controle das animações através do eixo que tem maior movimento
			//se o módulo (ignora valor negativo) do eixo horizontal (input do jogador)
			// for maior que o eixo vertical (input do jogador)...
			if (Mathf.Abs (x) > Mathf.Abs (y)) {
				//então o personagem está indo MAIS para o eixo horizontal do que o vertical
				//logo, se o eixo horizontal é maior que 0, o personagem receberá a direção 2 (leste)
				//caso contrário, a direção do personagem será -2 (oeste)
				currentDirection = x > 0 ? 2 : -2;
				//e a animação horizontal será chamada...
				anim.SetInteger ("isMovingTo", 2);

				//caso não entre no primeiro if, então será verificado se
				//o módulo do eixo x do input do jogador é menor que o input y
				//ou seja, está andando mais na vertical do que na horizontal
			} else if (Mathf.Abs (x) < Mathf.Abs (y)) {
				//eentão verifica-se se o input vertical é maior que 0, sendo,
				//então o persongaem está indo para a direção 1 (norte)
				//caso contrário, estará indo para 3 (sul)
				currentDirection = y > 0 ? 1 : 3;
				//a animação será ajustada de acordo com a direção (1 ou 3)
				anim.SetInteger ("isMovingTo", currentDirection);
			} else if (x == 0 && y == 0) {
				//caso estejam todos iguais a 0, a animação será ajustada para a posição 0 (parado)
				anim.SetInteger ("isMovingTo", 0);
			}

			//controle da escala do eixo x
			Vector3 scale = transform.localScale;
			//caso a direção atual seja -2, a escala horizontal do persongem será igual a escala orizontal negativada (inverterá o lado que está sendo observado)
			//caso contrário, será a escala horizontal original (não ficará ivnertido)
			scale.x = currentDirection == -2 ? -originalXScale : originalXScale;
			transform.localScale = scale;

			//se o personagem não estiver jogando em uma plataforma mobile e o input fire1 (para ver o input, ir em:
			//edit > project settings > input > axes (e procurar pelo Fire1 --  mas basicamente é o clique esquerdo do mouse) 
			if (!Application.isMobilePlatform && Input.GetButtonDown ("Fire1")) {
				//metodo da interação será chamado...
				Interact ();
			}

		}
	}//end fixed update

	void OnCollisionStay2D (Collision2D coll){
		//se estiver colidindo com o switcher...
		if (coll.gameObject.CompareTag ("Switcher")) {
			//crio um ponto de referência para a atuação do tyrion que será a própria posição
			//subtraída, apenas no eixo Y, do valor da variável do alcance da mão...
			//serve para o raio não sair exatamente do centro do tyrion, mas de uma posição mais "ajustada"
			referencePoint = transform.position - Vector3.up * handReachOffset;

			//então é feito uma escolha baseada na direção atual do tyrion...
			//armazena o vector3 (up,right,left,down) de acordo com essa direção
			switch(currentDirection){
			case 1:
				direction = Vector3.up;
				break;
			case 2:
				direction = Vector3.right;
				break;
			case -2:
				direction = Vector3.left;
				break;
			case 3:
				direction = Vector3.down;
				break;
			}

			//agora é lançado um raio invisível que sai o referencePoint
			//indo na direção (que depende da direção do tyrion) e que procura
			//apenas layers que possam ser afetadas pelo tyrion
			//caso encontre algo e este algo seja um switcher, então...
			Collider2D hit = Physics2D.Raycast (referencePoint, direction,handReachOffset,interactableLayer).collider;
			if (hit != null && hit.gameObject.CompareTag ("Switcher")) {
				//então... a variável switcher será preenchida com o gameObject
				switcher = hit.gameObject;
			} else {
				//caso contrário a variável continuará nula
				switcher = null;
			}
		}
	}

	//caso seja detectado um colisor passando pela área (trigger) e ele for o "Finish"
	//então o status atual é salvo com o personagem "vivo" e a cena irá mudar para a tela de vitória
	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.CompareTag ("Finish")) {
			SaveStatus (true);
			fade.ChangeScene ("TIE_Win");
		}
	}

	//método chamado quando o tyrion se render (morrer/perder o jogo/ for derrotado/for catpurado)
	void Surrender(){
		isFree = false;//ajusta o isFree para falso, para impedir outras interações do jogador
		anim.SetBool ("wasCought", true);//ativa o parametro de animação "foi pego"

		//e invoca o gameOver depois de meio segundo (pequeno delay para dar tempo ao
		//jogador para processar a informação)
		Invoke ("GameOver", 0.5f);
	}

	//salva o status do jogo com o jogador capturado (morto/preso/derrotado/etc) e muda para a cena game over
	public void GameOver() {
		SaveStatus (false);
		fade.ChangeScene ("GameOver");
	}

	//ato de interação do tyrion com o mundo do jogo...
	public void Interact(){
		//se a variável switcher estiver preenchida com alguma coisa....
		if (switcher != null) {
			//então envia uma mensagem para essa "coisa" chamando o método "Switch"
			//que irá ativar/desativar a alavanca, abrindo/fechando a porta de saída
			switcher.SendMessage ("Switch");
		}
	}

	//salva o status atual do jogo para enviar as informações para uma próxima cena
	void SaveStatus(bool isAlive){
		//salva qual o jogo está sendo jogado agora e o "score" atual...
		PlayerPrefs.SetString("LastGame",SceneManager.GetActiveScene().name);
		PlayerPrefs.SetInt ("Score", isAlive ? timer.score : 0);

		//salva as informações em arquivos
		PlayerPrefs.Save ();
	}

	//quando o Tyrion estiver selecionado, durante a edição do jogo,
	//será desenhado na tela da edição um linha...
	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red; //vermelho será cor da linha e do raio
		Gizmos.DrawLine (referencePoint, referencePoint + direction); //a linha terá origem no reference point e irá até na direção direction
	}

}

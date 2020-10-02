using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

//using System.Text;
//using System.IO;

public class FollowPath : MonoBehaviour {

	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//private fields
	Rigidbody2D rb;
	bool isStopped; //se o objeto parou por algum motivo

	//public fields
	public List<Vector3> path; //pontos no caminho para quais os objetos se direcionam
	public float speed = 0.1f; //velocidade com que o objeto vai de um ponto a outro
	public bool loop; //verifica se o personagem irá ficar percorrendo o caminho (ida e volta) em loop
	public int step; //identifica em qual posição atual da lista o personagem está se dirigindo
	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/
	void Start () {
		isStopped = false;
		transform.position = path [0];
		step = 0;
		rb = GetComponent<Rigidbody2D> (); //procura o rigidbody2D no próprio objeto, se não encontrar retorna null
	}

	void FixedUpdate () {
		//se não estiver parado, pode seguir com o caminho
		if (!isStopped) {
			//ajusta o valor de nova position para ir, aos poucos, alcançando o path[step]
			rb.MovePosition (Vector2.MoveTowards (rb.position, path [step], speed * Time.fixedDeltaTime));

			//se a distância for menor que 0.1, então passa para o próximo passo (step)
			//caso não tenha outro passo ele verifica se está em loop, estando, reinicia
			//a caminhada com o path revertido
			if (Vector2.Distance (transform.position, path [step]) < 0.1f) {
				if (step + 1 < path.Count) {
					step++;
				} else if (loop) {
					path.Reverse ();
					step = 0;
				}
				LookToTarget ();
			}
		}
	}//end fixed update

	/************************************************************
	************************   METHODS   ************************
	************************************************************/
	//olhar para a direção do target no step atual,
	//porem é na direção horizontal, esquerda ou direita
	public void LookToTarget(){
		Vector3 scale = transform.localScale;
		scale.x = (path [step].x < transform.position.x) ? -Mathf.Abs (scale.x) : Mathf.Abs (scale.x);
		transform.localScale = scale;
	}

	//sobrecarga do método, aqui ele olha na direção do target que é passado por parâmetro
	public void LookToTarget(Vector3 target){
		Vector3 scale = transform.localScale;
		scale.x = (target.x < transform.position.x) ? -Mathf.Abs (scale.x) : Mathf.Abs (scale.x);
		transform.localScale = scale;
	}

	//quando chamado, este método irá tornar a variável isStopped
	//em true, parando completamente a ação do script
	public void Stop(){
		isStopped = true;
	}

	//quando chamado, este método irá tornar a variável isStopped
	//em false, fazendo com que ela volte a funcionalidade normal
	public void Play(){
		isStopped = false;
	}

}


	/************************************************************
	***********************   NEW CLASS   ***********************
	************************************************************/

//classe para a alterar o unity no modo edição
public class FollowPathGizmoDrawer {
	#if UNITY_EDITOR

	//esse atributo irá permitir que o gizmo seja desenhado
	//apenas quando o objeto estiver ativo no inspector
	[DrawGizmo (GizmoType.Active)]
	static void DrawGizmoForFollowPath (FollowPath followPath, GizmoType type) {
		//alteração de cor, e ajuste do tamanho que irei querer para o "cubo"
		Gizmos.color = Color.yellow;
		float size = .3f;

		//aqui vai varrer todos os pontos do array e gerar linhas entre eles,
		//assim como, nos pontos específicos, gerar uns cubos para representar os pontos
		for (int i = 0; i < followPath.path.Count; i++) {
			if (i < followPath.path.Count - 1) {
				Gizmos.DrawLine (followPath.path[i],followPath.path[i+1]);
				Gizmos.DrawCube (followPath.path[i], Vector3.one * size);
			}
			//aqui é para desenhar o último cubo, pois a sequencia anterior vai até o penúltimo
			Gizmos.DrawCube (followPath.path[followPath.path.Count-1], Vector3.one * size);
		}

	
	}//end draw gizmos for followpath
	#endif
}//end class

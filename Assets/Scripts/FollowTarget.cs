using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//private field
	Vector3 velocity;

	//public fields
	public Vector3 offset;
	public Transform target;
	public float damping;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/
	void Start(){
		velocity = Vector3.zero;
	}


	// Update is called once per frame
	void Update () {
		//esse ścript fará com que o objeto que o carregue siga o "target" de forma suavizada
		//futurePosition recebe a posição do target, então o offset, no eixo x, é atualizado com a informação
		//que depende da condição "se o alvo estiver virado para a esquerda, então o eixo x do
		//offset será negativo, caso contrário, será positivo
		Vector3 futurePosition = target.position;
		offset.x = target.localScale.x < 0 ? -Mathf.Abs(offset.x) : Mathf.Abs(offset.x);

		//a posição futura terá esse offset adicionado
		//esse offset é apenas um deslocamento do objeto que está seguindo, para que o alvo não fique sempre centralizado
		//um ajuste lateral, na altura e/ou na profundidade.
		futurePosition += offset;

		//então a posição corrente é atualizada através do SmoothDamp que irá retornar um Vector3 com
		//o valor "saindo do parâmetro 1", em direção ao "parâmetro2",
		//na velocidade do "parametro3" (a própria função mexe nesse valor, apenas inicializei ele)
		//e irá demorar o tempo do "parâmetro4" para alcançar seu valor (funciona como uma suavização)
		transform.position = Vector3.SmoothDamp (transform.position, futurePosition, ref velocity, damping);
	}
}

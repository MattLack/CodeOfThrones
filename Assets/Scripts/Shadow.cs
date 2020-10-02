using UnityEngine;

public class Shadow : MonoBehaviour {
	/************************************************************
	**********************   PROPERTIES   ***********************
	*************************************************************/ 
	//public fields
	public SpriteRenderer mainArt;
	[Range(0f,1f)]
	public float maxAlpha = .5f;
	public float distanceOffset = 1f;
	public float horizontalOffset = 0f;
	public Transform originalBody;

	//private fields
	SpriteRenderer sprite;
	Transform earthEdge;

	/************************************************************
	*****************   MONOBEHAVIOUR MESSAGES   ****************
	************************************************************/
	void Start(){
		sprite = GetComponent<SpriteRenderer> ();	
	}

	void Update () {
		//vou ajustando o alpha em relação a arte principal
		Color c = sprite.color;
		c.a = Mathf.Clamp(mainArt.color.a, 0f, maxAlpha);
		sprite.color = c;

		//armazenando a distancia do corpo original para o limite da terra
		float originalBodyDistance = originalBody.position.y - earthEdge.position.y;

		//criando uma copia da posição do corpo original
		Vector3 newPosition = originalBody.position;

		//ajustando a posição da sombra de forma horizontal
		newPosition.x += horizontalOffset;

		//usando a distancia do corpo original de forma negativa para usar como um espelho
		newPosition.y = earthEdge.position.y - originalBodyDistance - distanceOffset;

		//atribuindo a nova distancia à sombra
		transform.position = newPosition;
	}
}

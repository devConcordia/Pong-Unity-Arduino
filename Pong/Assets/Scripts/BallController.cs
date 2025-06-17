using UnityEngine;

public class BallController : MonoBehaviour {
	
	/// velocidade da bola
	public float speed = 5f;
	
	/// 
	private Rigidbody2D body;
	
	/// variavel para ajudar a definir a direção da colisão
	private Vector2 lastVelocity;
	
	/// método chamado quando o BallController iniciar
    void Start() {
		
		body = GetComponent<Rigidbody2D>();
        
    }
	
	/// método chamado a cada frame
    void Update() {
		
        // guarda a última velocidade
        lastVelocity = body.linearVelocity;
		
    }
	
	/** reset
	 *	
	 *	Método utilizado para resetar a velocidade e posição do objeto.
	 *
	 */
	public void reset() {
		
		/// para de mover o objeto
		body.linearVelocity = new Vector2( 0f, 0f );
		
		/// move para a posição inicial
		transform.position = new Vector3( 0f, 0f, 0f );
		
	}
	
	/** startMove
	 *	
	 *	Método utilizado para iniciar uma movimentação em uma direção aleatoria.
	 *
	 */
	public void startMove() {
		
		/// para o eixo x o valor pode ser entre -1 e 1
		/// para o eixo y o valor pode ser entre -0.25 e 0.25, pois não queromos que ele vá para cima ou baixo diretamente
		body.linearVelocity = new Vector2(Random.Range(-1f, 1f), Random.Range(-.25f, .25f)).normalized * speed;
		
	}
	
	/// Quando ocorrer uma colisão
	private void OnCollisionEnter2D( Collision2D collision ) {
		
        /// obtem a normal da colisão
        Vector2 normal = collision.contacts[0].normal;

        /// calcula a direção refletida
        Vector2 direction = Vector2.Reflect(lastVelocity.normalized, normal);
		
		/// define uma nova velocidade na direção refletida
		body.linearVelocity = direction * speed;
		
		/// verifica se a colisão é um goal, caso seja atualiza o placar
		if( collision.gameObject.CompareTag("GoalLeft") ) {
			
			GameManager.Instance.addRightPoint();
			
		} else if( collision.gameObject.CompareTag("GoalRight") ) {
			
			GameManager.Instance.addLeftPoint();
			
		}
		
	}
	
}

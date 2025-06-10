using UnityEngine;

public class BallController : MonoBehaviour
{
	
	public float speed = 5f;
	private Rigidbody2D body;
	private Vector2 lastVelocity;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		
		body = GetComponent<Rigidbody2D>();
        
    }
	
	
    void Update()
    {
        // Guardamos a última velocidade para refletir corretamente
        lastVelocity = body.linearVelocity;
    }
	
	public void reset() {
		
		body.linearVelocity = new Vector2(0f,0f);
		transform.position = new Vector3(0f,0f,0f);
		
	}
	
	public void start() {
		
		body.linearVelocity = new Vector2(Random.Range(-1f, 1f), 1).normalized * speed;
		
	}
	
	
	private void OnCollisionEnter2D( Collision2D collision ) {
		
	//	Vector2 direction = Vector2.Reflect(body.linearVelocity.normalized, collision.contacts[0].normal);
		
        // Pega a normal do ponto de colisão
        Vector2 normal = collision.contacts[0].normal;

        // Reflete a velocidade usando a normal
        Vector2 direction = Vector2.Reflect(lastVelocity.normalized, normal);
		
		body.linearVelocity = direction * speed;
		
		if( collision.gameObject.CompareTag("GoalLeft") ) {
			
			GameManager.Instance.addRightPoint();
			
		} else if( collision.gameObject.CompareTag("GoalRight") ) {
			
			GameManager.Instance.addLeftPoint();
			
		}
		
		
		
	/*	Vector2 dir = new Vector2(0f,0f);
		
		if( collision.gameObject.CompareTag("PlayerLeft") ) {
			
			dir = Vector2.Reflect( body.linearVelocity.normalized, new Vector2( 1f, 0f ) );
			
		} else if( collision.gameObject.CompareTag("PlayerRight") ) {
			
			dir = Vector2.Reflect( body.linearVelocity.normalized, new Vector2( -1f, 0f ) );
			
		} else if( collision.gameObject.CompareTag("Wall") ) {
			
			float y = (collision.gameObject.transform.position.y > 0f)? -1f : 1f;
			
			dir = Vector2.Reflect(body.linearVelocity.normalized, new Vector2( 0f, y ));
		//	dir = Vector2.Reflect(body.linearVelocity.normalized, collision.contacts[0].normal);
			
			
		}
		
		
		body.linearVelocity = dir * speed;
		
		
	/**/
		
	}
	
}

using UnityEngine;

public class BallController : MonoBehaviour
{
	
	public float speed = 5f;
	private Rigidbody2D body;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		
		body = GetComponent<Rigidbody2D>();
        
    }
	
	public void reset() {
		
		body.linearVelocity = new Vector2(0f,0f);
		transform.position = new Vector3(0f,0f,0f);
		
	}
	
	public void throwRight() {
		
		body.linearVelocityY = speed;
		body.linearVelocityX = speed;
		
	}
	
	public void throwLeft() {
		
		body.linearVelocityX = -speed;
		body.linearVelocityY = speed;
		
	}
	
	private void OnCollisionEnter2D( Collision2D collision ) {
		
		if( collision.gameObject.CompareTag("PlayerLeft") ) {
			
			Vector2 dir = Vector2.Reflect( body.linearVelocity.normalized, new Vector2(-1.0f, 0f ) );
			
			body.linearVelocity = dir * speed;
			
		} else if( collision.gameObject.CompareTag("PlayerRight") ) {
			
			Vector2 dir = Vector2.Reflect( body.linearVelocity.normalized, new Vector2( 1.0f, 0f ) );
			
			Debug.Log( dir );
			
			body.linearVelocity = dir * speed;
			
		} else if( collision.gameObject.CompareTag("Wall") ) {
			
			float y = (collision.gameObject.transform.position.y > 0f)? 1f : -1f;
			
			Debug.Log( body.linearVelocity.normalized );
			
			Vector2 dir = Vector2.Reflect( body.linearVelocity.normalized, new Vector2( 0f, y ) );
			
			Debug.Log( dir );
			
			body.linearVelocity = dir * speed;
			
		}
		
	}
	
}

using UnityEngine;

public class PlayerRight : MonoBehaviour {
	
	[SerializeField] public GameObject ball;
	[SerializeField] public float speed = 2;
	
	private Rigidbody2D body;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
		body = GetComponent<Rigidbody2D>();
		
    }

    // 
    void FixedUpdate() {
        
		/// obtem a distancia entre o objeto e a bola no eixo x
		float dy = ball.transform.position.y - transform.position.y;
		
		/// move objeto em direção a posição y da bola
		body.linearVelocityY = dy * speed;
		
    }
	
}

using UnityEngine;

public class PlayerRight : MonoBehaviour
{
	
	[SerializeField] public GameObject ball;
	[SerializeField] public float speed = 2;
	
	private Rigidbody2D body;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
		body = GetComponent<Rigidbody2D>();
		
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
		float dy = ball.transform.position.y - transform.position.y;
		
		body.linearVelocityY = dy * speed;
		
    }
	
	
	
}

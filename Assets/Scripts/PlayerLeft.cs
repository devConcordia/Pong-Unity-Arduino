using UnityEngine;

public class PlayerLeft : MonoBehaviour
{
	
	[SerializeField] public float speed = 2;
	
	private Rigidbody2D body;
	
	private bool zBtn = false;
	private bool xBtn = false;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
		body = GetComponent<Rigidbody2D>();
		
    }

    // Update is called once per frame
    void Update()
    {
        
		if( Input.GetKeyDown(KeyCode.Z) ) zBtn = true;
		if( Input.GetKeyDown(KeyCode.X) ) xBtn = true;
		
		if( Input.GetKeyUp(KeyCode.Z) ) zBtn = false;
		if( Input.GetKeyUp(KeyCode.X) ) xBtn = false;
		
		
		if( zBtn ) {
			
			moveUp();
			
		} else if( xBtn ) {
			
			moveDown();
			
		} else {
			
			body.linearVelocityY = 0f;
			
		}
		
    }
	
	public void moveUp() {
		
		body.linearVelocityY = speed;
		
	}
	
	public void moveDown() {
		
		body.linearVelocityY = -speed;
		
	}
	
	
}

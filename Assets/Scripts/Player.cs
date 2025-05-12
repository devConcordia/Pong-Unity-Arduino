using UnityEngine;

public class Player : MonoBehaviour
{

	//private bool jumping = false;
	
	public bool isGround = false;
	public float speed = 8f;
	public SimuladorFisica fisica;
	
	
	
	//
	private Vector3 cameraOffset = new Vector3( 4.4f, 0f, -10f );
	[SerializeField] public Camera mainCamera;
	
	[SerializeField] public GameObject boomerang;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

		fisica = new SimuladorFisica( transform );
		
    }
	
    // Update is called once per frame
    void FixedUpdate()
    {
		
		/// gravidade
		if( !isGround ) {
			
			fisica.addForca( new Vector3(0f,-10f,0f) );
		
		}
		

		if( fisica.velocidade.x != 0 ) {
			
			fisica.velocidade.x *= .8f;
			
		}
		
		
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
		
		///
		if( x != 0 )
			fisica.addForca( new Vector3(x * speed,0f,0f) );
		
		if( y > 0 )
			fisica.addForca( new Vector3(0f, y * 30f, 0f) );
		
		/// atualiza camera acompanhando o player
		mainCamera.transform.position = cameraOffset + new Vector3( transform.position.x, 0f, 0f );
		
        if( Input.GetMouseButtonDown(0) ) Shoot();
        
		
		///
		fisica.atualizar();
		
    }
	
	
    void Shoot() {
		
		Vector3 p = transform.position + new Vector3(1f, 0f, 0f);
		
        GameObject projectile = Instantiate(boomerang, p, Quaternion.identity);
		
	//	projectile.getFisica().addForca( new Vector3(10f,0f,0f) );
		
    }

	private void OnCollisionExit2D(Collision2D collision) {
		
        if( collision.gameObject.CompareTag("Ground") ) {
			
			isGround = false;
			
		}
	
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		
        if( collision.gameObject.CompareTag("Ground") ) {
			
			isGround = true;
			
			/// adiciona a força normal, para cancelar a gravidade
			fisica.aceleracao.y = 0f;
			fisica.velocidade.y = 0f;
			
		}
	
	}

	
}

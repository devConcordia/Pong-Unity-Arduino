using UnityEngine;

public class PlayerLeft : MonoBehaviour {
	
	/// velocidade de movimentação 
	[SerializeField] public float speed = 2;
	
	private Rigidbody2D body;
	
	/// variaveis para os botões (z e x) do teclado
	/// quando pressinados será true
	private bool zBtn = false;
	private bool xBtn = false;
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
		body = GetComponent<Rigidbody2D>();
		
    }

    // Update is called once per frame
    void Update() {
        
		/// verifica se os botões foram pressionados
		if( Input.GetKeyDown(KeyCode.Z) ) zBtn = true;
		if( Input.GetKeyDown(KeyCode.X) ) xBtn = true;
		
		/// verifica se os botões foram soltos
		if( Input.GetKeyUp(KeyCode.Z) ) zBtn = false;
		if( Input.GetKeyUp(KeyCode.X) ) xBtn = false;
		
		if( zBtn ) {
			
			/// se o botão z estiver pressionado move para cima
			moveUp();
			
		} else if( xBtn ) {
			
			/// se o botão x estiver pressionado move para baixo
			moveDown();
			
		} else {
			
			/// se nenhum botão estiver pressionado, para de mover
			body.linearVelocityY = 0f;
			
		}
		
    }
	
	/// move objeto para cima
	public void moveUp() {
		
		body.linearVelocityY = speed;
		
	}
	
	/// move objeto para baixo
	public void moveDown() {
		
		body.linearVelocityY = -speed;
		
	}
	
}

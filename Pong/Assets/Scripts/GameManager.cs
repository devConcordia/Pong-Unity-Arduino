using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	
	[SerializeField] public GameObject ball;
	private BallController ballCtrl;
	
	[SerializeField] public GameObject hudCanvas;
	[SerializeField] public GameObject Scoreboard;
	private TMP_Text textScoreboard;
	private int scoreLeft = 0;
	private int scoreRight = 0;
	
	[SerializeField] public GameObject managerCanvas;
	[SerializeField] public GameObject GameMessage;
	[SerializeField] public GameObject GameHelper;
	private TMP_Text textGameMessage;
//	private TMP_Text textGameHelper;
	
	
	
	public static GameManager Instance = null;
	
	public bool playing = false;
	
    private void Awake() {
		
        if (Instance != null && Instance != this) {
            Destroy(gameObject); // Evita duplicação
            return;
        }

        Instance = this;
        
    }
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
		textScoreboard = Scoreboard.GetComponent<TMP_Text>();
		textGameMessage = GameMessage.GetComponent<TMP_Text>();
	//	textGameHelper = GameHelper.GetComponent<TMP_Text>();
		
		ballCtrl = ball.GetComponent<BallController>();

	//	playing = false;
    //   Time.timeScale = .00001f;
		
		prepareGame();
		
    }
	
	void Update() {
		
		if( !playing ) {
			if( Input.GetKeyDown(KeyCode.Space) ) {
				
				prepareGame();
				
			}
		}
		
	}
	
	public void startGame() {
		
		hudCanvas.SetActive(true);
		managerCanvas.SetActive(false);
		
		ballCtrl.reset();
		ballCtrl.start();
		
	}
	
	public void endGame( string message ) {
		
		textGameMessage.text = message;
		
		managerCanvas.SetActive(true);
		GameHelper.SetActive(true);
		
		playing = false;
        Time.timeScale = .00001f;
		
	}
	
	public void prepareGame() {
		
		hudCanvas.SetActive(false);
		managerCanvas.SetActive(true);
		
		ballCtrl.reset();
		
		scoreLeft = 0;
		scoreRight = 0;
		updateScoreboard();
		
		playing = true;
        Time.timeScale = 1f;
		
		StartCoroutine( startGameCountdown() );
		
	}
	
    IEnumerator startGameCountdown() {
        
		//textGameMessage.text = "Prepara ...";
		
		GameHelper.SetActive(false);
		
		int currentTime = 3;

        while( currentTime > 0 ) {
           
			textGameMessage.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            
			currentTime--;
        
		}

        textGameMessage.text = "GO!";
		yield return new WaitForSeconds(1f);
        
		
		startGame();
		
    }
	
	
	public void updateScoreboard() {
		
		textScoreboard.text = scoreLeft +" x "+ scoreRight;
		
		if( scoreLeft >= 2 ) {
			
			endGame( "Parabéns! Você ganhou!" );
			
		} else if( scoreRight >= 2 ) {
			
			endGame( "Não foi dessa vez ..." );
			
		} else {
			
			if( playing ) startGame();
			
		}
		
	}
	
	public void addLeftPoint() {
		
		scoreLeft++;
		updateScoreboard();
		
	}
	
	public void addRightPoint() {
		
		scoreRight++;
		updateScoreboard();
		
	}
	
}

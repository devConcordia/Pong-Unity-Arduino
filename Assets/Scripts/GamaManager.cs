using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GamaManager : MonoBehaviour
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
	private TMP_Text textGameMessage;
	
	
	
	public static GamaManager Instance = null;
	
	private bool playing = false;
	
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
		
		ballCtrl = ball.GetComponent<BallController>();

		playing = false;
        Time.timeScale = .00001f;
		
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
		
		setScoreboard( 0, 0 );
		
		ballCtrl.reset();
		ballCtrl.throwRight();
		
	}
	
	public void endGame( string message ) {
		
		textGameMessage.text = message;
		
		managerCanvas.SetActive(true);
		
		playing = false;
        Time.timeScale = .00001f;
		
	}
	
	public void prepareGame() {
		
		hudCanvas.SetActive(false);
		managerCanvas.SetActive(true);
		
		playing = true;
        Time.timeScale = 1f;
		
		StartCoroutine( startGameCountdown() );
		
	}
	
    IEnumerator startGameCountdown() {
        
		Debug.Log("startGameCountdown");
		
		textGameMessage.text = "Prepara ...";
		
		yield return new WaitForSeconds(3f);
        startGame();
		
    }
	
	
	public void setScoreboard( int left, int right ) {
		
		textScoreboard.text = left +" x "+ right;
		
	}
	
	public void addLeftPoint() {
		
		scoreLeft++;
		
		setScoreboard( scoreLeft, scoreRight );
		
	}
	
	public void addRightPoint() {
		
		scoreRight++;
		
		setScoreboard( scoreLeft, scoreRight );
		
	}
	
	
	public static void GoodGame() {
		
		Instance.endGame( "Parabéns!" );
		
	}
	
	public static void GameOver() {
		
		Instance.endGame( "Você perdeu!" );
		
	}
	
}

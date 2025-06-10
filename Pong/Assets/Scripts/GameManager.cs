using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
	
	/// para utilizar esse recurso é preciso utilizar o .NET Framework
	/// Edit > Project Settings > Player > Configuration - API Compatibility Level = ".NET Framework"
	private SerialPort serialPort; // = new SerialPort("COM4", 9600);
	private Coroutine routineSerialPort;
	
	[SerializeField] public GameObject goPlayerLeft;
	private PlayerLeft playerLeft;
	
	[SerializeField] public GameObject goBall;
	private BallController ballCtrl;
	
	[SerializeField] public GameObject goHudCanvas;
	[SerializeField] public GameObject goSerialPort;
	[SerializeField] public GameObject goScoreboard;
	private TMP_Text textScoreboard;
	private TMP_Text textSerialPort;
	private int scoreLeft = 0;
	private int scoreRight = 0;
	
	[SerializeField] public GameObject goManagerCanvas;
	[SerializeField] public GameObject goGameMessage;
	[SerializeField] public GameObject goGameHelper;
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
        
		textScoreboard = goScoreboard.GetComponent<TMP_Text>();
		textSerialPort = goSerialPort.GetComponent<TMP_Text>();
		textGameMessage = goGameMessage.GetComponent<TMP_Text>();
	//	textGameHelper = goGameHelper.GetComponent<TMP_Text>();
		
		playerLeft = goBall.GetComponent<PlayerLeft>();
		ballCtrl = goBall.GetComponent<BallController>();

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
		
		
		if( Input.GetKeyDown(KeyCode.Alpha3) )
			startSerialPort("COM3");
		
		if( Input.GetKeyDown(KeyCode.Alpha4) )
			startSerialPort("COM4");
		
		if( Input.GetKeyDown(KeyCode.Alpha5) )
			startSerialPort("COM5");
		
		if( Input.GetKeyDown(KeyCode.Alpha6) )
			startSerialPort("COM6");
		
		if( Input.GetKeyDown(KeyCode.Alpha7) )
			startSerialPort("COM7");
		
		if( Input.GetKeyDown(KeyCode.Alpha8) )
			startSerialPort("COM8");
		
		if( Input.GetKeyDown(KeyCode.Alpha9) )
			startSerialPort("COM9");
		
	}
	
	public void startGame() {
		
		goHudCanvas.SetActive(true);
		goManagerCanvas.SetActive(false);
		
		ballCtrl.reset();
		ballCtrl.start();
		
	}
	
	public void endGame( string message ) {
		
		textGameMessage.text = message;
		
		goManagerCanvas.SetActive(true);
		goGameHelper.SetActive(true);
		
		playing = false;
        Time.timeScale = .00001f;
		
	}
	
	public void prepareGame() {
		
		goHudCanvas.SetActive(false);
		goManagerCanvas.SetActive(true);
		
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
		
		goGameHelper.SetActive(false);
		
		int currentTime = 3;

        while( currentTime > 0 ) {
           
			textGameMessage.text = currentTime.ToString();
            yield return new WaitForSeconds(1f);
            
			currentTime--;
        
		}

        textGameMessage.text = "GO!";
		yield return new WaitForSeconds(1f);
        
		///
		startGame();
		
    }
	
	
	public void updateScoreboard() {
		
		textScoreboard.text = scoreLeft +" x "+ scoreRight;
		
		int[] leds = { 0, 0, 0, 0 };
		
		for( int i = 0; i < scoreLeft; i++ )
			leds[i] = 1;
		
		for( int i = 0; i < scoreRight; i++ )
			leds[3-i] = 1;
		
		///
		setLEDValues( leds );
		
		///
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
	
	
	
	public void onArduinoReceive( int btnL, int btnR ) {
		
		if( btnL == 1 ) playerLeft.moveUp();
		
		if( btnR == 1 ) playerLeft.moveDown();
		
	}
	
	private void startSerialPort( string com, int rate = 57600 ) {
		
		textSerialPort.text = com;
		
		
		if( serialPort != null ) 
			stopSerialPort();
		
		serialPort = new SerialPort( com, rate );
		
		routineSerialPort = StartCoroutine(ReadSerialPort()); // Start reading the serial port using a coroutine
		
	}
	
	private void stopSerialPort() {
		
		if( serialPort.IsOpen )
			serialPort.Close(); // Close the serial port when the application quits
		
		StopCoroutine(routineSerialPort);
		
	}
	
	IEnumerator ReadSerialPort()
	{
		while( true ) {
			string data = null;
			
			if( serialPort.IsOpen ) {
				try {
					
					// Read a line of data from the serial port
					data = serialPort.ReadLine();
					
					string[] btns = data.Split(';');
					
					if( btns.Length == 2 ) {
						if( int.TryParse(btns[0], out int btnL) && int.TryParse(btns[1], out int btnR) ) {
							
							onArduinoReceive( btnL, btnR );
							
						}
					}
					
				} catch( System.Exception ) {
					
					// Handle any exceptions that occur during reading
				
				}
			}
			
			yield return null; // Wait for the next frame
		
		}
	}

	void OnApplicationQuit()
	{
		if( serialPort != null && serialPort.IsOpen ) {
			serialPort.Close(); // Close the serial port when the application quits
		}
	}
	
	void setLEDValues( int[] leds ) {
		
		if( serialPort != null && serialPort.IsOpen && leds.Length == 4 ) {
            
			string message = string.Join(";", leds);
			
			serialPort.Write( message + "\n" );
            
			Debug.Log( message );
			
        }
    }
	
}

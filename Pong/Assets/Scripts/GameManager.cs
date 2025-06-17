using System.IO.Ports;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
	
	/// para utilizar esse recurso é preciso utilizar o .NET Framework
	/// Edit > Project Settings > Player > Configuration - API Compatibility Level = ".NET Framework"
	private SerialPort serialPort;
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
	
	/// criamos uma referencia estatica para ser acessada 
	/// pela bola quando ela colidir com um goal, e assim 
	/// atualizar o placar
	public static GameManager Instance = null;
	
	/// identifica se está em jogo ou aguardando iniciar
	public bool playing = false;
	
	/// invocado quando o objeto é criado
    private void Awake() {
		
		// Evita duplicação
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
    }
	
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
		textScoreboard = goScoreboard.GetComponent<TMP_Text>();
		textSerialPort = goSerialPort.GetComponent<TMP_Text>();
		textGameMessage = goGameMessage.GetComponent<TMP_Text>();
		
		playerLeft = goPlayerLeft.GetComponent<PlayerLeft>();
		ballCtrl = goBall.GetComponent<BallController>();
		
		prepareGame();
		
    }
	
	void Update() {
		
		if( !playing ) {
			if( Input.GetKeyDown(KeyCode.Space) ) {
				
				prepareGame();
				
			}
		}
		
		/// a seguir identificamos quando uma tecla (acima do teclado)
		/// de um numero (3 ao 9) é pressionada, e iniciamos uma 
		/// a conexão com a porta COM desse numero
		
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
	
	/** OnApplicationQuit
	 *	
	 *	Método invocado quando a aplicação encerrar
	 *	
	 */
	void OnApplicationQuit() {
		
		/// verifica se há conexão com alguma porta serial
		if( serialPort != null && serialPort.IsOpen ) {
			
			/// encerra comunicação com a porta serial
			serialPort.Close();
			
		}
		
	}
	
	
	/** prepareGame
	 *	
	 *	Zera placar e prepara o proximo jogo, então inica uma contagem regressiva.
	 *	Chamado quando playing for false e o jogador pressionar Space.
	 *	playing é definido como true e o tempo volta a correr normalmente.
	 *	
	 */
	public void prepareGame() {
		
		/// altera os canvas
		goHudCanvas.SetActive(false);
		goManagerCanvas.SetActive(true);
		
		/// remove a movimentação e reposiciona a bola
		ballCtrl.reset();
		
		/// zera placar
		scoreLeft = 0;
		scoreRight = 0;
		updateScoreboard();
		
		/// altera status do jogo
		playing = true;
        Time.timeScale = 1f;
		
		/// inicia a contagem regressiva
		StartCoroutine( startGameCountdown() );
		
	}
	
	/** startGameCountdown
	 *	
	 *	Contagem regressiva antes de chamar o startGame.
	 *	Deverá ser chamado com o metodo StartCoroutine();
	 *
	 */
    IEnumerator startGameCountdown() {
        
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
	
	/** startGame
	 *	
	 *	Inicia um novo jogo, chamado ao final da contagem regressiva do prepareGame().
	 *	
	 */
	public void startGame() {
		
		/// altera os canvas
		goHudCanvas.SetActive(true);
		goManagerCanvas.SetActive(false);
		
		/// remove a movimentação e reposiciona a bola
		ballCtrl.reset();
		
		/// inicia movimentação aleatoria da bola 
		ballCtrl.startMove();
		
	}
	
	/** endGame
	 *	
	 *	Encerra o jogo e exibe a mensagem (derrota ou vitoria).
	 *	playing é alterado para false e o tempo é reduzido (para a execução).
	 *	
	 */
	public void endGame( string message ) {
		
		/// exibe mensagem (derrota ou vitoria)
		textGameMessage.text = message;
		
		/// altera os canvas
		goManagerCanvas.SetActive(true);
		goGameHelper.SetActive(true);
		
		/// altera status do jogo
		playing = false;
        Time.timeScale = .00001f;
		
	}
	
	/** updateScoreboard
	 *	
	 *	Atualiza placar no HUD do jogo e calcula os leds 
	 *	que precisam estar acessos e chama o metodo setLEDValues
	 *	para escrever na porta serial
	 *	
	 */
	public void updateScoreboard() {
		
		/// atualiza texto do HUD 
		textScoreboard.text = scoreLeft +" x "+ scoreRight;
		
		/// inicia variavel dos estados dos LEDs
		int[] leds = { 0, 0, 0, 0 };
		
		/// define pontuação do jogador esquerdo
		for( int i = 0; i < scoreLeft; i++ )
			leds[i] = 1;
		
		/// define pontuação do jogador direito
		for( int i = 0; i < scoreRight; i++ )
			leds[3-i] = 1;
		
		/// escreve na porta serial os valores dos LEDs
		setLEDValues( leds );
		
		/// verifica se há jogador vencedor e encerra jogo
		if( scoreLeft >= 2 ) {
			
			endGame( "Parabéns! Você ganhou!" );
			
		} else if( scoreRight >= 2 ) {
			
			endGame( "Não foi dessa vez ..." );
			
		} else {
			
			/// se não houve vencedor e o jogo ainda está sendo executado
			/// inicia nova partida imediatamente (sem contagem regressiva)
			if( playing ) startGame();
			
		}
		
	}
	
	/** addLeftPoint
	 *	
	 *	Adiciona pontos ao jogador da esquerda e chama updateScoreboard
	 *	
	 */
	public void addLeftPoint() {
		
		scoreLeft++;
		updateScoreboard();
		
	}
	
	/** addRightPoint
	 *	
	 *	Adiciona pontos ao jogador da direita e chama updateScoreboard
	 *	
	 */
	public void addRightPoint() {
		
		scoreRight++;
		updateScoreboard();
		
	}
	
	
	/** onArduinoReceive
	 *	
	 *	Esse método move o jogador esquerdo quando haver 
	 *	conteudo lido na porta serial que acontece em ReadSerialPort.
	 *	
	 */
	public void onArduinoReceive( int btnL, int btnR ) {
		
		if( btnL == 1 ) playerLeft.moveUp();
		
		if( btnR == 1 ) playerLeft.moveDown();
		
	}
	
	
	/** startSerialPort
	 *	
	 *	Inicia uma nova conexão e leitura de uma porta COM
	 *	
	 */
	private void startSerialPort( string com, int rate = 57600 ) {
		
		/// atualiza HUD
		textSerialPort.text = com;
		
		/// fecha a ultima porta aberta
		if( serialPort != null ) 
			stopSerialPort();
		
		/// inicia porta serial
		serialPort = new SerialPort( com, rate );
		
		/// define o tempo maximo de espera de alguma mensagem
		serialPort.ReadTimeout = 5;
		
		/// inicoa comunicação
		serialPort.Open();
		
		/// inicia leitura da porta serial utilizando o coroutine
		routineSerialPort = StartCoroutine(ReadSerialPort()); 
		
	}
	
	/** stopSerialPort
	 *	
	 *	Encerra conexão e leitura da porta COM
	 *	
	 */
	private void stopSerialPort() {
		
		/// fecha se porta atual estiver aberta
		if( serialPort.IsOpen )
			serialPort.Close();
		
		/// encerra coroutine da leitura da porta serial
		StopCoroutine(routineSerialPort);
		
	}
	
	/** ReadSerialPort
	 *	
	 *	Coroutine para leitura da porta serial.
	 *	Devera ser chamada com StartCoroutine e devera ser 
	 *	encerrada com StopCoroutine, quando a porta for fechada.
	 *	
	 */
	IEnumerator ReadSerialPort() {
		
		while( true ) {
			
			string data = null;
			
			if( serialPort.IsOpen && serialPort.BytesToRead > 0 ) {
				try {
					
					/// Realiza uma leitura do conteudo da porta serial,
					/// o conteudo será lido até a quabra de linha
					data = serialPort.ReadLine();
					
					/// obtem os dados que veio do arduino e separa pelo ';'
					string[] btns = data.Split(';');
					
					/// se a quantidade separada for igual ao esperado (0;0)
					if( btns.Length == 2 ) {
						
						/// tenta converter os dados em int
						if( int.TryParse(btns[0], out int btnL) && int.TryParse(btns[1], out int btnR) ) {
							
							/// realiza as operações de movimentação
							onArduinoReceive( btnL, btnR );
							
						}
					}
					
				} catch( System.TimeoutException ) {
					
					/// Ignora as falhas de timeout
					
				} catch( System.Exception e ) {
					
					/// escreve no log outros erros de conexão com a porta serial
					Debug.LogError( e );
					
				}
			}
			
			/// espera o proximo frame
			yield return null;
		
		}
		
	}
	
	/** setLEDValues
	 *	
	 *	Metodo escreve os valores dos LEDs na porta serial
	 *	
	 */
	void setLEDValues( int[] leds ) {
		
		/// verifica se a conexão com a porta serial foi aberta
		/// e se a quantidade de informações de LEDs é a correta
		if( serialPort != null && serialPort.IsOpen && leds.Length == 4 ) {
            
			/// envia para o arduino as informações da porta serial 
			serialPort.Write( string.Join(";", leds) +"\n" );
            
        }
		
    }
	
}

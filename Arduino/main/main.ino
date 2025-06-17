
#define LED_1 6
#define LED_2 7
#define LED_3 8
#define LED_4 9

#define BTN_L 10
#define BTN_R 11

/// inicia um array para os pinos dos LEDs
int led_pins[4] = { LED_1, LED_2, LED_3, LED_4 };

/// declara o valor dos LEDs
int leds[4];

/** setup
 *	
 */
void setup() {
	
	/// inicia a porta serial
	Serial.begin( 57600 );
	
	/// incia os pinos dos botões como entrada PULLUP
	pinMode( BTN_L, INPUT_PULLUP );
	pinMode( BTN_R, INPUT_PULLUP );
	
	/// inicia os pinos dos LEDs como saida
	pinMode( LED_1, OUTPUT );
	pinMode( LED_2, OUTPUT );
	pinMode( LED_3, OUTPUT );
	pinMode( LED_4, OUTPUT );

}

/** loop
 *	
 */
void loop() {
	
	/// obtemos valore invertidos dos botões, por conta do INPUT_PULLUP
	int btnL = !digitalRead( BTN_L ),
		btnR = !digitalRead( BTN_R );
	
	/// escreve na porta serial os valores dos botões
	writeSerialPort( btnL, btnR );
	
	/// lê a porta serial e carrega os valores de cada LED
	readSerialPort();
	
	/// como o loop do arduino é muito rapido causava
	/// legs na excução do Jogo na Unity então usamos
	/// o delay para forçar a redução da transmissão
	delay(5);

}

/** writeSerialPort
 *	
 *	Esse metodo é responsavel por escrever na porta serial 
 *	o estado dos botões: 0 para não pressionado e 1 para pressionado.
 *	
 *	O valor escrito na porta serial é uma string com os valores
 *	dos botões separados por ponto e virgula (;).
 *	O primeiro valor deverá ser do botão a esquerda e o segundo 
 *	o estado do botão direito.
 *	
 */
void writeSerialPort( int btnL, int btnR ) {
	
	/// escreve o estado do botão esquerdo
	Serial.print( btnL );
	
	/// escreve o separador 
	Serial.print( ";" );
	
	/// escreve o estado do botão direito com quebra de linha
	Serial.println( btnR );

}

/** readSerialPort
 *	
 *	Esse método é responsavel por ler a porta serial, interpretar e chamar o metodo que altera o estado dos LEDs.
 *	
 */
void readSerialPort() {
	
	/// verifica se há leitura para ser ser realizada
	if( Serial.available() ) {
		
		/// inicia uma String com o conteudo da porta serial
		String data = Serial.readStringUntil('\n');
		
		/// remove espaços e quebras de linha no começo e fim da string
		data.trim();
		
		/// inicia o indice referente a cada LED
		int index = 0;
		
		/// inicia uma String auxiliar para separar os valores da leitura
		String subdata;
		
		/// enquanto data possuir caracteres e 
		/// o indice for menor que 4 (quantidade de LEDs)
		while( data.length() > 0 && index < 4 ) {
			
			/// obtem a posição do proximo ponto-e-virgula (;)
			int i = data.indexOf(';');
			
			/// verifica se não encontrou uma posição na string
			if( i == -1 ) {
				
				/// assume que data possui o conteudo restante
				subdata = data;
				
				/// remove conteudo de data, para sair do loop
				data = "";
			
			} else {
				
				/// "corta" o data na posição inicial até o ponto-e-virgula
				subdata = data.substring( 0, i );
				
				/// remove de data o valor capturado de subdata
				data = data.substring( i + 1 );
				
			}
			
			/// converte o valor de subdata em int e prepara para escrever no pino
			leds[index] = subdata.toInt();
			
			/// proximo valor do LED
			index++;
			
		}
		
		/// altera os valores dos LEDs
		setLeds( leds );
		
	}
	
}

/** setLeds
 *	
 *	Esse método é responsavel por acender (1) ou apagar (0) os LEDs,
 *	de acordo com o valore informados.
 *	
 */
void setLeds( int values[] ) {
	
	/// para cadaa LED, altera seu estado de acordo com os valores informados
	for( int i = 0; i < 4; i++ )
		digitalWrite( led_pins[i], values[i] == 1 ? HIGH : LOW );
	
}

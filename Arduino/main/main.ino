
#define LED_1 6
#define LED_2 7
#define LED_3 8
#define LED_4 9

#define BTN_L 10
#define BTN_R 11

///
int led_pins[4] = { LED_1, LED_2, LED_3, LED_4 };
int leds[4];
///
void setup() {
	
	Serial.begin( 57600 );
	
	pinMode( BTN_L, INPUT_PULLUP );
	pinMode( BTN_R, INPUT_PULLUP );
	
	pinMode(LED_1, OUTPUT);
	pinMode(LED_2, OUTPUT);
	pinMode(LED_3, OUTPUT);
	pinMode(LED_4, OUTPUT);

}

//
void loop() {

	/// Invertido por causa do INPUT_PULLUP
	int btnL = !digitalRead( BTN_L ),
		btnR = !digitalRead( BTN_R );
	
	///
	writeSerialPort( btnL, btnR );
	readSerialPort();
	
	delay(5);

}

void writeSerialPort( int btnL, int btnR ) {
	
	/// 
	Serial.print( btnL );
	Serial.print( ";" );
	Serial.println( btnR );

}

/// 
int readSerialPort() {
	
	if( Serial.available() ) {
		
		// lê até encontrar \n
		String data = Serial.readStringUntil('\n');
		// remove espaços e quebras de linha extras
		data.trim();

		int index = 0;

		while( data.length() > 0 && index < 4 ) {
			
			int i = data.indexOf(';');
			String subdata;

			if( i == -1 ) {
				
				subdata = data;
				data = "";
			
			} else {
				
				subdata = data.substring(0, i);
				data = data.substring(i + 1);
				
			}

			leds[index] = subdata.toInt();
			index++;
		}
		
		setLeds( leds );
		
	}
	
}

void setLeds( int values[] ) {

	for( int i = 0; i < 4; i++ )
		digitalWrite( led_pins[i], values[i] == 1 ? HIGH : LOW );
	
}

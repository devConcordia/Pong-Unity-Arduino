
#define LED_1 6
#define LED_2 7
#define LED_3 8
#define LED_4 9

#define BTN_L 10
#define BTN_R 11

void setup() {
	
	Serial.begin( 57600 );
	
	pinMode( BTN_L, INPUT_PULLUP );
	pinMode( BTN_R, INPUT_PULLUP );
	
	pinMode(LED_1, OUTPUT);
	pinMode(LED_2, OUTPUT);
	pinMode(LED_3, OUTPUT);
	pinMode(LED_4, OUTPUT);

}

void loop() {

	//	/ Invertido por causa do INPUT_PULLUP
	int btnL = !digitalRead( BTN_L ),
		btnR = !digitalRead( BTN_R );
	
	///
	writeSerialPort( btnL, btnR );
	
	///
	int leds = readSerialPort();
	
	if( leds != -1 ) 
		setLeds( leds );
	
}

void writeSerialPort( int btnL, int btnR ) {
	
	/// 
	Serial.write( btnL << 1 | btnR );
//	Serial.write( (byte)(btnL << 1 | btnR) );

//	Serial.print( btnL << 1 | btnR );
//	Serial.print( "\n" );
	
//	Serial.print( btnL );
//	Serial.print( "," );
//	
//	///
//	Serial.print( btnR );
//	Serial.print( "\n" );
	
}

/// 
int readSerialPort() {
	
	// 
	if( Serial.available() > 0 )
		return Serial.read();
	
	return -1;
	
}

void setLeds( int leds ) {
	
	digitalWrite( LED_1, leds & 1 );
	digitalWrite( LED_2, (leds >> 1) & 1 );
	digitalWrite( LED_3, (leds >> 2) & 1 );
	digitalWrite( LED_4, (leds >> 3) & 1 );
	
}

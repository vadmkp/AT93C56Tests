#include <Arduino.h>
#include <SoftwareSerial.h>

// Konfiguracja pinu LED zgodnie z ArduinoConfiguration
const uint8_t LED_PIN = 13;
const unsigned long COMMAND_DELAY_MS = 100;

// Piny SoftwareSerial dla modu³u HC-05
const uint8_t BT_RX = 10; // Arduino receives from HC-05 TX
const uint8_t BT_TX = 11; // Arduino sends to HC-05 RX

SoftwareSerial btSerial(BT_RX, BT_TX); // RX, TX

String inputBuffer = "";

void setup() {
  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, LOW);

  Serial.begin(115200); // debug przez USB
  btSerial.begin(9600); // HC-05 domyœlnie 9600

  Serial.println("Arduino ready");
  btSerial.println("Arduino ready");
}

void loop() {
  // Odbierz dane z USB Serial
  while (Serial.available()) {
    char c = (char)Serial.read();
    if (c == '\n') {
      handleCommand(inputBuffer);
      inputBuffer = "";
    } else if (c != '\r') {
      inputBuffer += c;
    }
  }

  // Odbierz dane z Bluetooth
  while (btSerial.available()) {
    char c = (char)btSerial.read();
    if (c == '\n') {
      handleCommand(inputBuffer);
      inputBuffer = "";
    } else if (c != '\r') {
      inputBuffer += c;
    }
  }
}

void sendResponse(const String &resp) {
  Serial.println(resp);
  btSerial.println(resp);
}

void handleCommand(const String &cmd) {
  String s = cmd;
  s.trim();
  if (s.length() == 0) return;

  // Debug
  Serial.print("CMD: "); Serial.println(s);

  if (s.equalsIgnoreCase("PING")) {
    sendResponse("PONG");
    return;
  }

  if (s.startsWith("SET:")) {
    // Format: SET:<pin>:<0|1>
    int first = s.indexOf(':');
    int second = s.indexOf(':', first + 1);
    if (first == -1 || second == -1) {
      sendResponse("ERR");
      return;
    }
    String pinStr = s.substring(first + 1, second);
    String valStr = s.substring(second + 1);
    int pin = pinStr.toInt();
    int val = valStr.toInt();

    if (pin == LED_PIN) {
      digitalWrite(pin, val == 0 ? LOW : HIGH);
      delay(COMMAND_DELAY_MS);
      sendResponse("OK");
      return;
    } else {
      sendResponse("ERR");
      return;
    }
  }

  if (s.startsWith("GET:")) {
    // Format: GET:<pin>
    int colon = s.indexOf(':');
    if (colon == -1) { sendResponse("ERR"); return; }
    String pinStr = s.substring(colon + 1);
    int pin = pinStr.toInt();
    if (pin == LED_PIN) {
      int state = digitalRead(pin);
      sendResponse("STATE:" + String(pin) + ":" + String(state == LOW ? 0 : 1));
      return;
    } else {
      sendResponse("ERR");
      return;
    }
  }

  if (s.startsWith("TOGGLE:")) {
    int colon = s.indexOf(':');
    if (colon == -1) { sendResponse("ERR"); return; }
    String pinStr = s.substring(colon + 1);
    int pin = pinStr.toInt();
    if (pin == LED_PIN) {
      int current = digitalRead(pin);
      digitalWrite(pin, current == LOW ? HIGH : LOW);
      delay(COMMAND_DELAY_MS);
      int state = digitalRead(pin);
      sendResponse("STATE:" + String(pin) + ":" + String(state == LOW ? 0 : 1));
      return;
    } else {
      sendResponse("ERR");
      return;
    }
  }

  sendResponse("ERR");
}

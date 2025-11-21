Opis układu Arduino dla projektu Atmel

Cel
- Prosty protokół szeregowy do sterowania pinami Arduino (np. LED) przez aplikację WinUI przez moduł Bluetooth (HC-05) lub USB.

Lista elementów
- Arduino Uno / Nano (5V)
- Moduł Bluetooth HC-05 (opcjonalnie)
- LED (np. czerwona)
- Rezystor 220 Ω do LED
- Przewody połączeniowe
- (opcjonalnie) płytka stykowa

Schemat połączeń
- LED: dłuższa nóżka (anoda) -> rezystor 220 Ω -> pin `13` Arduino
         krótsza nóżka (katoda) -> GND

- HC-05 (jeśli używasz):
  - VCC -> 5V (na niektórych modułach można zasilać 3.3V; typowo HC-05 działa z 5V)
  - GND -> GND
  - TXD (HC-05) -> pin `10` Arduino (RX dla SoftwareSerial)
  - RXD (HC-05) -> pin `11` Arduino (TX dla SoftwareSerial) — przez dzielnik napięcia z 2x rezystorów (np. 2k2 i 1k) lub inny konwerter, aby nie podawać 5V na wejście HC-05

Uwaga: piny 0 i 1 to sprzętowy Serial (USB). Zalecane użycie `SoftwareSerial` na pinii 10/11, żeby móc monitorować debug przez USB.

Parametry i zgodność z projektem
- W projekcie .NET konfiguracja `ArduinoConfiguration` ma domyślnie `LedPin = 13` i `CommandDelayMs = 100`. Kod Arduino używa pinu 13 jako domyślnego do diody i zastosuje opóźnienie (debounce / delay) w milisekundach zgodnie z `CommandDelayMs`.
- Moduł Bluetooth w projekcie jest często nazwany `HC-05` (pole `AlternativeDeviceName`) — układ przewiduje komunikację RFCOMM przez interfejs szeregowy.

Protokół szeregowy (tekstowy)
- Komendy przychodzą jako linia tekstu zakończona `\n` (baud 9600). Przykłady:
  - `SET:<pin>:<0|1>` — ustawia stan pinu (0 = LOW, 1 = HIGH). Odpowiedź: `OK` lub `ERR`.
  - `GET:<pin>` — pobiera stan pinu. Odpowiedź: `STATE:<pin>:<0|1>`.
  - `TOGGLE:<pin>` — przełącza stan pinu i odsyła `STATE:<pin>:<0|1>`.
  - `PING` — odpowiada `PONG`.

- Komendy można wysyłać z aplikacji PC/mobile przez port szeregowy (USB) lub przez połączony moduł Bluetooth (HC-05).

Bezpieczeństwo i uwagi praktyczne
- Przy podłączaniu RX/TX HC-05 użyj dzielnika napięcia na linii Arduino->HC-05 RX.
- Nie podłączaj HC-05 do pinów 0/1, jeśli równocześnie używasz USB Serial do debugu.
- Jeśli urządzenie ma działać bez modułu Bluetooth, kod obsługuje również `Serial` (USB) do testów.

Dalsze rozszerzenia
- Autentykacja prostym hasłem w protokole (np. `AUTH:<pass>`).
- Obsługa kilku urządzeń/pinów oraz raportowanie stanu w formacie JSON.

Plik z kodem: `Arduino.ino` — zawiera implementację powyższego protokołu i obsługę LED na pinie 13.

# MQTT-konsument för sensordata

## Beskrivning
Applikationen tar emot temperaturdata från en ESP32 via MQTT, omvandlar JSON-meddelanden till C#-objekt och skriver ut mätvärdena i konsolen.

## Lösningens delar
- **TelemetryMessage:** Representerar sensordata med enhets-ID, mätvärde, enhet och tidsstämpel.
- **MqttConsumerService:** Ansluter till MQTT-brokern, prenumererar på ett topic och tar emot inkommande meddelanden.
- **TelemetryDisplayService:** Visar mottagna mätvärden och tidsstämplar i konsolen.
- **Programstart:** Skapar tjänsterna och startar MQTT-konsumenten.

## Teknik och beroenden
- **.NET 10:** Ramverk som applikationen körs på.
- **MQTTnet 5.2.0:** Bibliotek för MQTT-kommunikation.
- **Microsoft.Extensions.Configuration.Json 10.0.13:** Används för att läsa konfiguration från `appsettings.json`.

Konfigurationsfilen `appsettings.json` kopieras automatiskt till programmets utdatakatalog vid byggning.

## Konfiguration
MQTT-inställningarna anges i `appsettings.json`:

- MQTT-brokerns adress och port.
- Användarnamn och lösenord, om sådana krävs.
- MQTT-topic som applikationen ska prenumerera på.

Applikationen använder `localhost` och port `1883` som standardvärden om motsvarande inställningar saknas. Filen `appsettings.json` måste dock finnas för att applikationen ska starta.

## Starta applikationen
1. Installera .NET 10 SDK.
2. Kontrollera att MQTT-brokern är igång och att `appsettings.json` innehåller rätt anslutningsuppgifter.
3. Öppna en terminal i projektets katalog.
4. Kör `dotnet run` för att starta applikationen.
5. När anslutningen lyckats visas ett meddelande i konsolen, följt av inkommande sensordata.

## Dataflöde
ESP32 → MQTT-broker → MQTT-konsument → JSON-avkodning → Konsolutskrift.

#define EEPROM_SUPPORT 1
#define DEBUG 0

#include <EEPROM.h>

const int RED = 8;
const int GREEN = 9;
const int BLUE = 10;

int last_r = 255;
int last_g = 255;
int last_b = 255;

void UpdateColors(int r, int g, int b)
{
    analogWrite(RED, 255 - r);
    analogWrite(GREEN, 255 - g);
    analogWrite(BLUE, 255 - b);
}

int Sanitize(int input)
{
    if (input == 0)
    {
        return -1;
    }
    if (input == -1)
    {
        return 0;
    }
    return input;
}

void setup()
{
    Serial.begin(9600);
    pinMode(RED, OUTPUT);
    pinMode(GREEN, OUTPUT);
    pinMode(BLUE, OUTPUT);

#if EEPROM_SUPPORT
    last_r = EEPROM.read(0);
    last_g = EEPROM.read(1);
    last_b = EEPROM.read(2);
#endif

#if DEBUG
    Serial.println(last_r);
    Serial.println(last_g);
    Serial.println(last_b);
#endif

    analogWrite(RED, 255 - last_r);
    analogWrite(GREEN, 255 - last_g);
    analogWrite(BLUE, 255 - last_b);
}

void loop()
{
    if (Serial.available() == 0)
    {
        return;
    }

    int r = Sanitize(Serial.parseInt());
    int g = Sanitize(Serial.parseInt());
    int b = Sanitize(Serial.parseInt());

    if (r == -1 || g == -1 || b == -1)
    {
        return;
    }

    if (r != last_r || g != last_g || g != last_b)
    {
        UpdateColors(r, g, b);

        last_r = r;
        last_g = g;
        last_b = b;

#if DEBUG
        Serial.println(last_r);
        Serial.println(last_g);
        Serial.println(last_b);
#endif

#if EEPROM_SUPPORT
        EEPROM.write(0, r - 256);
        EEPROM.write(1, g - 256);
        EEPROM.write(2, b - 256);
#endif
    }
}

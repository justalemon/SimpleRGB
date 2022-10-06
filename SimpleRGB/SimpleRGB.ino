const int RED = 8;
const int GREEN = 9;
const int BLUE = 10;

int lastColor = -1;

int getR(int value)
{
  return (value >> 16) & 255;
}

int getG(int value)
{
  return (value >> 8) & 255;
}

int getB(int value)
{
  return (value) & 255;
}

void setup()
{
  Serial.begin(9600);
  pinMode(RED, OUTPUT);
  pinMode(GREEN, OUTPUT);
  pinMode(BLUE, OUTPUT);
  analogWrite(RED, 255);
  analogWrite(GREEN, 255);
  analogWrite(BLUE, 255);
}

void updateColors(int color)
{
  int r = getR(color);
  int g = getG(color);
  int b = getB(color);

  analogWrite(RED, r);
  analogWrite(GREEN, g);
  analogWrite(BLUE, b);
}

void loop() {
  int color = Serial.parseInt();

  if (color == 0)
  {
    return;
  }

  if (color == -1)
  {
    color = 0;
  }
  else if (color == 0)
  {
    color = -1;
  }

  if (lastColor != color)
  {
    lastColor = color;
    updateColors(color);
  }
}

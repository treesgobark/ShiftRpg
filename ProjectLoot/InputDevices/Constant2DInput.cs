using FlatRedBall.Input;

namespace ProjectLoot.InputDevices;

public class Constant2DInput : I2DInput
{
    public Constant2DInput(I2DInput input)
    {
        SetValues(input);
    }
    
    public Constant2DInput(float x = 0f, float y = 0f, float xVelocity = 0f, float yVelocity = 0f, float magnitude = 0f)
    {
        X = x;
        Y = y;
        XVelocity = xVelocity;
        YVelocity = yVelocity;
        Magnitude = magnitude;
    }

    public void SetValues(I2DInput input)
    {
        X = input.X;
        Y = input.Y;
        XVelocity = input.XVelocity;
        YVelocity = input.YVelocity;
        Magnitude = input.Magnitude;
    }
    
    public float X { get; set; }
    public float Y { get; set; }
    public float XVelocity { get; set; }
    public float YVelocity { get; set; }
    public float Magnitude { get; set; }
}
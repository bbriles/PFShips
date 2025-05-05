using Godot;
using System;

public partial class Laser : Area2D
{
    public int ID; 
    public bool IsActive { get; private set; } = false;
    [Export] int TimeToLive = 5000;

    private Vector2 _velocity;

    public void Activate(Vector2 position, Vector2 velocity)
    {
        //GD.Print("Activating laser " + ID + " at position " + position + " with velocity " + velocity);
        Position = position;
        Rotation = velocity.Angle();
        _velocity = velocity;

        IsActive = true;
        Visible = true;
        SetProcess(true);
        TimeToLive = 5000; // Reset time to live
    }

    public void Deactivate()
    {
        _velocity = Vector2.Zero;

        IsActive = false;
        Visible = false;
        SetProcess(false);
    }

    public override void _Ready()
    {
        Deactivate();
    }

    public override void _Process(double delta)
    {
        if (IsActive)
        {
            Position += _velocity * (float)delta;
            TimeToLive -= (int)(delta * 1000); // Convert delta to milliseconds
        }
        if (TimeToLive <= 0)
        {
            Deactivate();
        }
    }
}

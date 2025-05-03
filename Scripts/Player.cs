using Godot;
using System;

public partial class Player : Node2D
{
    [Export] public float ThrustPower = 50f; // Forward/backward thrust power
    [Export] public float RotationSpeed = 5f; // Rotation speed
    [Export] public float MaxVelocity = 100f; // Maximum velocity

    private Vector2 _velocity = Vector2.Zero;

    public override void _Process(double delta)
    {
        HandleInput(delta);
        Position += _velocity;
    }

    private void HandleInput(double delta)
    {
        // Rotation
        if (Input.IsActionPressed("ui_left"))
        {
            Rotation -= RotationSpeed * (float)delta;
        }
        if (Input.IsActionPressed("ui_right"))
        {
            Rotation += RotationSpeed * (float)delta;
        }

        // Thrust
        Vector2 direction = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
        if (Input.IsActionPressed("ui_up"))
        {
            _velocity += direction * ThrustPower * (float)delta;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            _velocity -= direction * ThrustPower * (float)delta;
        }

        // Limit the maximum velocity
        if (_velocity.Length() > MaxVelocity)
        {
            _velocity = _velocity.Normalized() * MaxVelocity;
        }

        // Apply damping to simulate friction
        _velocity = _velocity.Lerp(Vector2.Zero, 0.1f * (float)delta);
    }
}

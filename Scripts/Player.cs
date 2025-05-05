using Godot;
using System;

public partial class Player : SpaceObject
{
    [Export] public float ThrustPower = 10f; // Forward/backward thrust power
    [Export] public float RotationSpeed = 5f; // Rotation speed
    [Export] public float MaxVelocity = 50f; // Maximum velocity

    [Export] AnimatedSprite2D EngineSprite; // Reference to the engine sprite
    [Export] LaserEmitter LaserEmitter; // Reference to the laser emitter

    private Vector2 _velocity = Vector2.Zero;

    public Vector2 Velocity => _velocity;

    public override void _Process(double delta)
    {
        HandleInput(delta);
        Position += _velocity * (float)delta;
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
        var thrustOn = false;
        Vector2 direction = new Vector2(Mathf.Cos(Rotation), Mathf.Sin(Rotation));
        if (Input.IsActionPressed("ui_up"))
        {
            _velocity += direction * ThrustPower * (float)delta;
            thrustOn = true;
        }
        if (Input.IsActionPressed("ui_down"))
        {
            _velocity -= direction * ThrustPower * (float)delta;
            thrustOn = true;
        }
        if(thrustOn)
        {
            EngineSprite.Visible = true;
            EngineSprite.Play("Thrust");
        }
        else
        {
            EngineSprite.Visible = false;
        }   

        // Firing Lasers
        if (Input.IsActionJustPressed("ui_select"))
        {
            LaserEmitter.FireLaser();
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

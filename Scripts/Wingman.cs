using Godot;
using System;

public partial class Wingman : SpaceObject
{
    [Export] public float ThrustPower = 15f; // Forward/backward thrust power
    [Export] public float RotationSpeed = 5f; // Rotation speed
    [Export] public float MaxVelocity = 60f; // Maximum velocity

    [Export] public float TargetAttraction = 50f; // Attraction force towards the target
    [Export] public float TargetRepulsion = 100f; // Repulsion force from the target 
    [Export] public float TargetAttractionDistance = 0.1f; // Controls how attraction falls off with distance
    [Export] public float TargetRepulsionDistance = 2.5f; // Stronger falloff to push away when too close
     

    [Export] SpaceObject Target; // Reference to the target object

    private Vector2 _velocity = Vector2.Zero;
    private Vector2 _steeringForce = Vector2.Zero;

    public override void _Process(double delta)
    {
        ProcessAI(delta);
        Position += _velocity;
    }

    private void ProcessAI(double delta)
    {
        // Reset steering force
        _steeringForce = Vector2.Zero;

        // AI logic for the wingman
        AttractToTarget();

        // Steering force can't exceed max thrust
        _steeringForce = _steeringForce.LimitLength(ThrustPower * (float)delta);

        _velocity += _steeringForce;
        Rotation = _steeringForce.Angle();

        if (_velocity.Length() > MaxVelocity)
        {
            _velocity = _velocity.Normalized() * MaxVelocity;
        }

    }

    private void AttractToTarget()
    {
        // Apply the Lendard-Jones potential to attract the wingman to the target
        Vector2 direction = (Target.Position - Position).Normalized();
        float distance = Position.DistanceTo(Target.Position);
        distance = Mathf.Max(distance, 0.01f);

        var force = TargetAttraction / Mathf.Pow(distance, TargetAttractionDistance) - TargetRepulsion / Mathf.Pow(distance, TargetRepulsionDistance);


        _steeringForce += (direction * force);
    }
}

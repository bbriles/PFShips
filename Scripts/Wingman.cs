using Godot;
using System;

public partial class Wingman : SpaceObject
{
    [Export] public float ThrustPower = 450f; // Forward/backward thrust power
    [Export] public float RotationSpeed = 5f; // Rotation speed
    [Export] public float MaxVelocity = 500f; // Maximum velocity

    [Export] public float TargetAttraction = 300f; // Attraction force towards the target
    [Export] public float TargetRepulsion = 2000f; // Repulsion force from the target 
    [Export] public float TargetAttractionDistance = 0.1f; // Controls how attraction falls off with distance
    [Export] public float TargetRepulsionDistance = 0.48f; // Stronger falloff to push away when too close

    [Export] public float EnemyAttraction = 1000f; // Attraction force towards the enemies
    [Export] public float EnemyRepulsion = 3000f; // Repulsion force from enemies 
    [Export] public float EnemyAttractionDistance = 0.2f; // Controls how attraction falls off with distance
    [Export] public float EnemyRepulsionDistance = 0.45f; // Stronger falloff to push away when too close


    [Export] SpaceObject Target; // Reference to the target object

    private Vector2 _velocity = Vector2.Zero;
    private Vector2 _acceleration = Vector2.Zero;

    public Vector2 Velocity => _velocity;
    public Vector2 Acceleration => _acceleration;

    public override void _Process(double delta)
    {
        ProcessAI(delta);
        Position += _velocity * (float)delta;
        GD.Print("delta: " + delta);

        // Apply damping to simulate friction
        _velocity = _velocity.Lerp(Vector2.Zero, 0.3f * (float)delta);
    }

    private void ProcessAI(double delta)
    {
        // Reset steering force
        _acceleration = Vector2.Zero;

        // AI logic for the wingman
        AttractToTarget();
        ChaseEnemies();

        // acceleration can't exceed max thrust
        _acceleration = _acceleration.LimitLength(ThrustPower);

        // Apply acceleration
        _velocity += (_acceleration * (float)delta);
        _velocity = _velocity.LimitLength(MaxVelocity);

        if (_velocity.Length() > 0.01f) // Avoid jittering when velocity is near zero
        {
            Rotation = _acceleration.Angle();
        }
    }

    private void AttractToTarget()
    {
        // Apply the Lenard-Jones potential to attract the wingman to the target
        Vector2 direction = (Target.Position - Position).Normalized();
        float distance = Position.DistanceTo(Target.Position);
        distance = Mathf.Max(distance, 0.01f);

        var force = (-TargetAttraction) / Mathf.Pow(distance, TargetAttractionDistance) + TargetRepulsion / Mathf.Pow(distance, TargetRepulsionDistance);
        //GD.Print($"Force: {force}");

        _acceleration += (-direction * force);
    }

    private void ChaseEnemies()
    {
        // iterate through list of enemies
        foreach (var enemy in Game.Enemies)
        {
            float distance = Position.DistanceTo(enemy.Position);
            // Check if the enemy is within a certain distance
            if (distance < 800f)
            {
                Vector2 direction = (enemy.Position - Position).Normalized();
                
                distance = Mathf.Max(distance, 0.01f);

                var force = (-EnemyAttraction) / Mathf.Pow(distance, EnemyAttractionDistance) + EnemyRepulsion / Mathf.Pow(distance, EnemyRepulsionDistance);
                //GD.Print($"Force: {force}");

                _acceleration += (-direction * force);
            }
        }
    }

    public override void _Draw()
    {
     
    }
}
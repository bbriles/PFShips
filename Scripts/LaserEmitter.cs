using Godot;
using System;
using System.Collections.Generic;

public partial class LaserEmitter : Node2D
{
    [Export] public float FireRate = 0.1f; // Time between shots in seconds
    [Export] public float LaserSpeed = 500f; // Speed of the laser
    private float _timeSinceLastShot = 0f;

    public override void _Ready()
    {
      
    }
    public override void _Process(double delta)
    {
        _timeSinceLastShot += (float)delta;
    }
    public void FireLaser()
    {
        if (_timeSinceLastShot >= FireRate)
        {
            var laser = Game.GetLaserFromPool(Game.LaserColor.Green);
            var parent = (Area2D)GetParent();
            laser.Activate(GlobalPosition, Vector2.Right.Rotated(parent.Rotation) * LaserSpeed);
            _timeSinceLastShot = 0f; // Reset the shot timer
        }
    }
}

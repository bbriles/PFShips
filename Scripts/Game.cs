using Godot;
using System;
using System.Collections.Generic;

public partial class Game : Node
{
    private static Player _player;
    public static Player Player
    {
        get => _player;
        set => _player = value;
    }

    private static List<SpaceObject> _enemies = new List<SpaceObject>();
    public static List<SpaceObject> Enemies
    {
        get => _enemies;
    }

    private static List<SpaceObject> _allies = new List<SpaceObject>();
    public static List<SpaceObject> Allies
    {
        get => _allies;
    }

    private static List<Laser> _laserGreenPool = new List<Laser>();
    private static List<Laser> _laserRedPool = new List<Laser>();
    private static int LaserPoolSize = 50;
    public enum LaserColor
    {
        Green,
        Red
    }

    public override void _Ready()
    {
        InitializeLaserPools();
    }

    private void InitializeLaserPools()
    {
        var laserScene = ResourceLoader.Load<PackedScene>("res://Entities/LaserGreen.tscn");
        for (int i = 0; i < LaserPoolSize; i++)
        {
            var laser = laserScene.Instantiate<Laser>();
            laser.ID = i; // Assign a unique ID to each laser
            _laserGreenPool.Add(laser);
            AddChild(laser);
        }

        laserScene = ResourceLoader.Load<PackedScene>("res://Entities/LaserRed.tscn");
        for (int i = 0; i < LaserPoolSize; i++)
        {
            var laser = laserScene.Instantiate<Laser>();
            laser.ID = i; // Assign a unique ID to each laser
            _laserRedPool.Add(laser);
            AddChild(laser);
        }
    }

    public static Laser GetLaserFromPool(LaserColor color)
    {
        List<Laser> laserPool = null;
        switch (color)
        {
            case LaserColor.Green:
                laserPool = _laserGreenPool;
                break;
            case LaserColor.Red:
                laserPool = _laserRedPool;
                break;
        }
        foreach (var laser in laserPool)
        {
            if (!laser.IsActive)
            {
                GD.Print("Returning laser " + laser.ID + "  from pool");
                return laser;
            }
        }
        GD.Print("No available lasers in pool");
        return null;
    }
}

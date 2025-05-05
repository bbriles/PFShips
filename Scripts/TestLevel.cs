using Godot;
using System;

public partial class TestLevel : Node2D
{
    public override void _Ready()
    {
        Game.Player = GetNode<Player>("Player");
        
        var alliesNode = GetNode<Node>("Allies");
        foreach(var ally in alliesNode.GetChildren())
        {
            Game.Allies.Add((SpaceObject)ally);
        }

        var enemiesNode = GetNode<Node>("Enemies");
        foreach (var enemy in enemiesNode.GetChildren())
        {
            Game.Enemies.Add((SpaceObject)enemy);
        }
    }
}

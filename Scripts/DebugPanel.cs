using Godot;
using System;

public partial class DebugPanel : Control
{
    [Export] public Player Player;
    [Export] public Wingman Wingman;

    private Label _playerVelocityLabel;
    private Label _wingmanVelocityLabel;
    private Label _wingmanForceLabel;

    public override void _Ready()
    {
        _playerVelocityLabel = GetNode<Label>("Panel/VBoxContainer/PlayerVelocityLabel");
        _wingmanVelocityLabel = GetNode<Label>("Panel/VBoxContainer/WingmanVelocityLabel");
        _wingmanForceLabel = GetNode<Label>("Panel/VBoxContainer/WingmanForceLabel");   
    }

    public override void _Process(double delta)
    {
        UpdateDebugInfo();
    }

    private void UpdateDebugInfo()
    {
        _playerVelocityLabel.Text = $"Player Velocity: {Player.Velocity.Length():F2}";
        _wingmanVelocityLabel.Text = $"Wingman Velocity: {Wingman.Velocity.Length():F2}";
        _wingmanForceLabel.Text = $"Wingman Force: {Wingman.Acceleration.Length():F2}";
    }
}

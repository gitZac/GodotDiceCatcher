using Godot;
using System;

public partial class Fox : Area2D
{
    [Export] private Sprite2D _foxSprite;
    [Export] private float _speed = 500.0f;
    [Export] private AudioStreamPlayer2D _soundEat;
    [Signal] public delegate void spriteCollisionEventHandler();

    private float _calc_move = 0.0f;

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    public override void _PhysicsProcess(double delta)
    {

    }

    public override void _Process(double delta)
    {
        checkInputs(delta);
    }


    public void checkInputs(double delta)
    {
        float moveDirection = Input.GetAxis("ui_left", "ui_right");

        if (!Mathf.IsZeroApprox(moveDirection))
        {
            _foxSprite.FlipH = moveDirection > 0.0f;
        }

        Position += new Vector2(moveDirection * (float)delta * _speed, 0);
    }

    private void OnAreaEntered(Area2D area)
    {
        _soundEat.Play();
        if (area is Dice)
        {
            area.QueueFree(); //remove area
            EmitSignal(SignalName.spriteCollision);
        }
    }

}

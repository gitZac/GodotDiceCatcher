using Godot;

public partial class Dice : Area2D
{
    [Export] private Sprite2D _diceSprite;
    [Signal] public delegate void boundsOutEventHandler();

    private const float SPEED = 150.0f;
    private const float BASE_ROTATION_SPEED = 4.0f;

    private float _rotationSpeed = BASE_ROTATION_SPEED;

    public override void _Ready()
    {
        if (GD.Randf() < 0.5f)
        {
            _rotationSpeed *= -1;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += new Vector2(0, SPEED * (float)delta);
        _diceSprite.Rotate(_rotationSpeed * (float)delta);
        CheckBoundsOut();
    }

    private void CheckBoundsOut()
    {
        Rect2 viewPortRect = GetViewportRect();

        if (Position.Y > viewPortRect.End.Y)
        {
            EmitSignal(SignalName.boundsOut);
            SetPhysicsProcess(false);
            QueueFree();
        }
    }
}

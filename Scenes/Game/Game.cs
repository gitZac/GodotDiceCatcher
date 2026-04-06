using Godot;

public partial class Game : Node2D

{
    [Export] private Fox _fox;
    [Export] private Label _scoreLabel;
    [Export] private PackedScene _diceScene;
    [Export] private Timer _spawnTimer;
    [Export] private AudioStreamPlayer2D _soundGameOver;
    [Export] private AudioStreamPlayer2D _soundMusic;
    [Export] private Label _gameOverText;

    private int _points;
    private const float MARGIN = 80.0f;
    private const string PAUSEABLE_GROUP = "isPauseable";

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("restart"))
        {
            GetTree().ReloadCurrentScene();
        }
    }

    public override void _Ready()
    {
        GD.Print($"Ready {Name} {GetInstanceId()}");
        _fox.spriteCollision += calcPlayerScore;
        _spawnTimer.Timeout += SpawnDice;
    }

    public void SpawnDice()
    {
        Dice diceInstance = _diceScene.Instantiate<Dice>();
        Rect2 diceRect = GetViewportRect();
        float spawnX = getRandomXPosition(diceRect);
        diceInstance.Position = new Vector2(spawnX, -MARGIN);
        diceInstance.boundsOut += initGameOver;
        AddChild(diceInstance);
    }

    private void PauseAll()
    {
        GD.Print("Pauseing");
        _spawnTimer.Stop();
        var nodesToPause = GetTree().GetNodesInGroup(PAUSEABLE_GROUP);

        foreach (Node node in nodesToPause)
        {
            node.SetPhysicsProcess(false);
            node.SetProcess(false);
        }
    }

    public void initGameOver()
    {
        GD.Print("GAME OVER");
        _soundMusic.Stop();
        _soundGameOver.Play();
        _gameOverText.Show();
        PauseAll();
    }

    public void calcPlayerScore()
    {
        _points++;
        _scoreLabel.Text = _points.ToString("D2");
    }

    private float getRandomXPosition(Rect2 rect)
    {
        return (float)GD.RandRange(rect.Position.X + MARGIN, rect.End.X - MARGIN);
    }

}

using Godot;
using System;

public class Main : Node
{
	[Export]
	public PackedScene Mob;

	private int _score;

	// We use 'System.Random' as an alternative to GDScript's random methods.
	private Random _random = new Random();

	public override void _Ready()
	{
	}

	// We'll use this later because C# doesn't support GDScript's randi().
	private float RandRange(float min, float max)
	{
		return (float)_random.NextDouble() * (max - min) + min;
	}

	public void game_over()
	{
		GD.Print("GameOver");

		GetNode<Timer>("MobTimer").Stop();
		GetNode<Timer>("ScoreTimer").Stop();
		GetTree().CallGroup("mobs", "queue_free");

		var hub = GetNode<HUD>("HUD");
		hub.ShowGameOver();
	}

	public void NewGame()
	{
		GD.Print("NewGame");

		_score = 0;

		var player = GetNode<Player>("Player");
		var startPosition = GetNode<Position2D>("StartPosition");
		player.Start(startPosition.Position);

		GetNode<Timer>("StartTimer").Start();

		var hub = GetNode<HUD>("HUD");
		hub.UpdateScore(_score);
		hub.ShowMessage("Get Ready !");
	}

	public void _on_StartTimer_timeout()
	{
		GD.Print("OnStartTimerTimeout");

		GetNode<Timer>("MobTimer").Start();
		GetNode<Timer>("ScoreTimer").Start();
	}

	public void _on_ScoreTimer_timeout()
	{
		GD.Print("OnScoreTimerTimeout");

		_score++;

		GetNode<HUD>("HUD").UpdateScore(_score);
	}

	public void _on_MobTimer_timeout()
	{
		GD.Print("OnMobTimerTimeout");

		// Choose a random location on Path2D.
		var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
		mobSpawnLocation.Offset = _random.Next();

		// Create a Mob instance and add it to the scene.
		var mobInstance = (Mob)Mob.Instance();
		AddChild(mobInstance);

		// Set the mob's direction perpendicular to the path direction.
		float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

		// Set the mob's position to a random location.
		mobInstance.Position = mobSpawnLocation.Position;

		// Add some randomness to the direction.
		direction += RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
		mobInstance.Rotation = direction;

		// Choose the velocity.
		mobInstance.LinearVelocity = new Vector2(RandRange(mobInstance.MinSpeed, mobInstance.MaxSpeed), 0).Rotated(direction);
	}
}

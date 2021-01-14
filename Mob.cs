using Godot;
using System;

public class Mob : RigidBody2D
{
	[Export]
	public int MinSpeed = 350; // Minimum speed range.

	[Export]
	public int MaxSpeed = 650; // Maximum speed range.

	static private Random _random = new Random();


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		var mobTypes = animSprite.Frames.GetAnimationNames(); // ["walk", "swim", "fly"]
		animSprite.Animation = mobTypes[_random.Next(0, mobTypes.Length)];

		//GetNode<Label>("Velocity").Hide();
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
		//GetNode<Label>("Velocity").Text = LinearVelocity.ToString();
    }

    public void _on_VisibilityNotifier2D_screen_exited()
	{
		//GD.Print("OnVisibilityNotifier2DScreenExited");

		QueueFree();
	}
}

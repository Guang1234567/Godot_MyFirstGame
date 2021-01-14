using Godot;
using System;

public class Player : Area2D
{
	[Signal]
	public delegate void Hit();

	[Export]
	public int Speed = 1000; // How fast the player will move (pixels/sec).

	private Vector2 _screenSize; // Size of the game window.

	private Vector2 _target;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_screenSize = GetViewport().Size;

		GD.Print("GetViewport().Size = ", _screenSize);

		Hide();
	}

	// Change the target whenever a touch event happens.
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventScreenTouch eventMouseButton && eventMouseButton.Pressed)
		{
			_target = (@event as InputEventScreenTouch).Position;
		}
	}

	public override void _Process(float delta)
	{
		var velocity = new Vector2(); // The player's movement vector.

		if(Position.DistanceTo(_target) > 10)
        {
			velocity = _target - Position;
        }

		/*
        if (Input.IsActionPressed("ui_right"))
        {
            velocity.x += 1;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            velocity.x -= 1;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            velocity.y += 1;
        }

        if (Input.IsActionPressed("ui_up"))
        {
            velocity.y -= 1;
        }
		*/

        var animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite.Play();
			
			Position += velocity * delta;
			Position = new Vector2(
				x: Mathf.Clamp(Position.x, 0, _screenSize.x),
				y: Mathf.Clamp(Position.y, 0, _screenSize.y)
			);
		}
		else
		{
			animatedSprite.Stop();
		}

		if (velocity.x != 0)
		{
			animatedSprite.Animation = "walk";
			animatedSprite.FlipV = false;
			// See the note below about boolean assignment
			animatedSprite.FlipH = velocity.x < 0;
		}
		else if (velocity.y != 0)
		{
			animatedSprite.Animation = "up";
			animatedSprite.FlipV = velocity.y > 0;
		}
	}

	public void _on_Player_body_entered(PhysicsBody2D body)
	{
		GD.Print("OnPlayerBodyEntered");

		Hide(); // Player disappears after being hit.
		EmitSignal("Hit");
		GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
	}

	public void Start(Vector2 pos)
	{
		_target = pos;
		Position = pos;
		Show();
		GetNode<CollisionShape2D>("CollisionShape2D").Disabled = false;
	}
}

using Godot;
using System;
using System.Diagnostics;

public partial class player : Area2D
{
  [Export]
  private int Speed = 350;
  private Vector2 velocity = Vector2.Zero;
  private Vector2 screenSize = new Vector2(480, 720);

  [Signal]
  public delegate void PickUpEventHandler();

  [Signal]
  public delegate void HurtEventHandler();

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() { }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
	velocity = Input.GetVector("left", "right", "up", "down");
	Position += velocity * Speed * (float)delta;
	Position = Position with
	{
	  X = Mathf.Clamp(Position.X, 0, screenSize.X),
	  Y = Mathf.Clamp(Position.Y, 0, screenSize.Y)
	};

	var animated2DSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	animated2DSprite.Animation = velocity.Length() > 0 ? "run" : "idle";
	animated2DSprite.FlipH = velocity.X != 0 && velocity.X < 0;
  }

  private void start()
  {
	SetProcess(true);
	Position = screenSize / 2;
	GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "idle";
  }

  private void die()
  {
	GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "hurt";
	SetProcess(false);
  }

  private void _on_area_entered(Area2D area)
  {
	if (area.IsInGroup("coins"))
	{
	  EmitSignal(SignalName.PickUp);
	}
	else if (area.IsInGroup("obstacles"))
	{
	  EmitSignal(SignalName.Hurt);
	  die();
	}
  }
}

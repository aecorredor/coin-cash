using Godot;
using System;
using System.Diagnostics;

public partial class player : Area2D
{
  [Export]
  int Speed = 350;
  Vector2 Velocity = Vector2.Zero;
  public Vector2 ScreenSize = new Vector2(480, 720);

  [Signal]
  public delegate void PickUpEventHandler();

  [Signal]
  public delegate void HurtEventHandler();

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() { }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    Velocity = Input.GetVector("left", "right", "up", "down");
    Position += Velocity * Speed * (float)delta;
    Position = Position with
    {
      X = Mathf.Clamp(Position.X, 0, ScreenSize.X),
      Y = Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
    };

    var animated2DSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    animated2DSprite.Animation = Velocity.Length() > 0 ? "run" : "idle";
    animated2DSprite.FlipH = Velocity.X != 0 && Velocity.X < 0;
  }

  public void Start()
  {
    SetProcess(true);
    Position = ScreenSize / 2;
    GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "idle";
  }

  public void Die()
  {
    GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "hurt";
    SetProcess(false);
  }

  private void _on_area_entered(Area2D area)
  {
    if (area.IsInGroup("coins") && area.HasMethod("PickUp"))
    {
      area.Call("PickUp");
      EmitSignal(SignalName.PickUp, "coin");
    }
    else if (area.IsInGroup("obstacles"))
    {
      EmitSignal(SignalName.Hurt);
      Die();
    }
    else if (area.IsInGroup("powerups") && area.HasMethod("PickUp"))
    {
      area.Call("PickUp");
      EmitSignal(SignalName.PickUp, "powerup");
    }
  }
}

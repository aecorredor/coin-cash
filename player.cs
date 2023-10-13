using Godot;
using System;
using System.Diagnostics;

public partial class player : Area2D
{
  [Export]
  private int Speed = 350;
  private Vector2 velocity = Vector2.Zero;
  private Vector2 screenSize = new Vector2(480, 720);

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
  }
}

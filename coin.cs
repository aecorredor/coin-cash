using Godot;
using System;

public partial class coin : Node2D
{
  private Vector2 screenSize = Vector2.Zero;

  private void PickUp()
  {
    QueueFree();
  }

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() { }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) { }
}

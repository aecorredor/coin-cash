using Godot;
using System;
using System.Collections;

public partial class powerup : Area2D
{
  public Vector2 ScreenSize = Vector2.Zero;

  private async void PickUp()
  {
    GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    var tw = CreateTween().SetParallel().SetTrans(Tween.TransitionType.Quad);
    tw.TweenProperty(this, "scale", Scale * 3, 0.3);
    tw.TweenProperty(this, "modulate:a", 0.0, 0.3);
    await ToSignal(tw, Tween.SignalName.Finished);
    QueueFree();
  }

  // Called when the node enters the scene tree for the first time.
  public override void _Ready() { }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) { }

  private void _on_lifetime_timeout()
  {
    QueueFree();
  }
}

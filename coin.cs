using Godot;
using System;

public partial class coin : Area2D
{
  private Timer timer;

  public Vector2 ScreenSize = Vector2.Zero;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    timer = GetNode<Timer>("Timer");
    timer.Start(GD.RandRange(3, 8));
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) { }

  private void _on_timer_timeout()
  {
    var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    animatedSprite.Frame = 0;
    animatedSprite.Play();
  }

  private async void PickUp()
  {
    GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred("disabled", true);
    var tw = CreateTween().SetParallel().SetTrans(Tween.TransitionType.Quad);
    tw.TweenProperty(this, "scale", Scale * 3, 0.3);
    tw.TweenProperty(this, "modulate:a", 0.0, 0.3);
    await ToSignal(tw, Tween.SignalName.Finished);
    QueueFree();
  }
}

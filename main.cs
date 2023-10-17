using Godot;
using System;
using System.Linq;

public partial class main : Node2D
{
  player Player;

  [Export]
  PackedScene CoinScene;

  [Export]
  int playTime = 30;

  int Level = 1;
  int Score = 0;
  int TimeLeft = 0;
  Vector2 ScreenSize = Vector2.Zero;
  bool isPlaying = false;

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    ScreenSize = GetViewport().GetVisibleRect().Size;
    Player = GetNode<player>("Player");
    Player.ScreenSize = ScreenSize;
    Player.Hide();
  }

  public void NewGame()
  {
    isPlaying = true;
    Level = 1;
    Score = 0;
    TimeLeft = playTime;
    Player.Start();
    Player.Show();
    GetNode<Timer>("GameTimer").Start();
    SpawnCoins();
    GetNode<HUD>("HUD").UpdateScore(Score);
    GetNode<HUD>("HUD").UpdateTimer(TimeLeft);
  }

  void SpawnCoins()
  {
    for (var i = 0; i < Level + 4; i++)
    {
      var coin = CoinScene.Instantiate() as coin;
      AddChild(coin);
      coin.ScreenSize = ScreenSize;
      coin.Position = new Vector2(
        (float)GD.RandRange(0, ScreenSize.X),
        (float)GD.RandRange(0, ScreenSize.Y)
      );
    }
    GetNode<AudioStreamPlayer>("LevelSound").Play();
  }

  public void GameOver()
  {
    isPlaying = false;
    GetNode<Timer>("GameTimer").Stop();
    GetTree().CallGroup("coins", "queue_free");
    GetNode<HUD>("HUD").ShowGameOver();
    Player.Die();
    GetNode<AudioStreamPlayer>("EndSound").Play();
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta)
  {
    if (isPlaying && GetTree().GetNodesInGroup("coins").Count() == 0)
    {
      Level += 1;
      TimeLeft += 5;
      SpawnCoins();
    }
  }

  private void _on_game_timer_timeout()
  {
    TimeLeft -= 1;
    GetNode<HUD>("HUD").UpdateTimer(TimeLeft);
    if (TimeLeft <= 0)
    {
      GameOver();
    }
  }

  private void _on_player_hurt()
  {
    GameOver();
  }

  private void _on_player_pick_up()
  {
    Score += 1;
    GetNode<HUD>("HUD").UpdateScore(Score);
    GetNode<AudioStreamPlayer>("CoinSound").Play();
  }

  private void _on_hud_start_game()
  {
    NewGame();
  }
}

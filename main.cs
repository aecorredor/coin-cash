using Godot;
using System;

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
    NewGame();
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
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) { }
}

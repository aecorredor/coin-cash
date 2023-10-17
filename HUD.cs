using Godot;
using System;

public partial class HUD : CanvasLayer
{
  [Signal]
  public delegate void StartGameEventHandler();

  private Timer timer;
  private Button startButton;
  private Label message;

  public void UpdateScore(int value)
  {
    GetNode<Label>("MarginContainer/Score").Text = value.ToString();
  }

  public void UpdateTimer(int value)
  {
    GetNode<Label>("MarginContainer/Time").Text = value.ToString();
  }

  public void ShowMessage(string text)
  {
    message.Text = text;
    message.Show();
    timer.Start();
  }

  public async void ShowGameOver()
  {
    ShowMessage("Game Over");
    await ToSignal(timer, "timeout");
    startButton.Show();
    message.Text = "Coin Dash!";
    message.Show();
  }

  // Called when the node enters the scene tree for the first time.
  public override void _Ready()
  {
    timer = GetNode<Timer>("Timer");
    startButton = GetNode<Button>("StartButton");
    message = GetNode<Label>("Message");
  }

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(double delta) { }

  private void _on_timer_timeout()
  {
    message.Hide();
  }

  private void _on_start_button_pressed()
  {
    startButton.Hide();
    message.Hide();
    EmitSignal(SignalName.StartGame);
  }
}

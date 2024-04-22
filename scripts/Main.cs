using Godot;

public partial class Main : Node
{
    [Export] public Label ratsCollectedLabel;
    [Export] Label timeLeftLabel;
    [Export] Timer energyRefillTimer;
    [Export] Timer gameEndTimer;
    [Export] TextureProgressBar energyBar;
    [Export] Texture2D energyBarReady;
    [Export] Texture2D energyBarNotReady;
    public int pointTotal = 0;
    public int energy = 100;
    int timeLeft;

    public override void _Ready()
    {
        Color bgColor = new(1f, 0.878f, 0.964f);
        RenderingServer.SetDefaultClearColor(bgColor);
        energyRefillTimer.Timeout += OnEnergyRefillTimerTimeout;
        gameEndTimer.Timeout += OnGameEndTimerTimeout;
    }

    public override void _Process(double delta)
    {
        RefillEnergy();

        if (energy > 100) energy = 100;

        timeLeft = (int)gameEndTimer.TimeLeft;
        timeLeftLabel.Text = timeLeft.ToString();
    }

    public void IncreaseRatPoints(int newPoints)
    {
        pointTotal += newPoints;
        ratsCollectedLabel.Text = "rats collected: " + pointTotal;
    }

    private void OnEnergyRefillTimerTimeout()
    {
        energy += 1;
    }

    private void RefillEnergy()
    {
        energyBar.Value = energy;
        energyBar.TextureProgress = energyBar.Value >= 20 ? energyBarReady : energyBarNotReady;
    }

    private void OnGameEndTimerTimeout()
    {
        GetTree().ChangeSceneToFile("res://scenes/end.tscn");
    }
}

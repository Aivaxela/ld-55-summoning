using Godot;

public partial class Main : Node
{
    [Export] public Label ratsCollectedLabel;
    [Export] Timer energyRefillTimer;
    [Export] TextureProgressBar energyBar;
    int pointTotal = 0;
    public int energy = 100;

    public override void _Ready()
    {
        Color bgColor = new(1f, 0.878f, 0.964f);
        RenderingServer.SetDefaultClearColor(bgColor);
        energyRefillTimer.Timeout += OnEnergyRefillTimerTimeout;
    }

    public override void _Process(double delta)
    {
        RefillEnergy();
    }


    public void IncreaseRatPoints(int points)
    {
        pointTotal += points;
        ratsCollectedLabel.Text = "rats collected: " + pointTotal;
    }

    private void OnEnergyRefillTimerTimeout()
    {
        energy += 1;
    }

    private void RefillEnergy()
    {
        energyBar.Value = energy;
    }
}

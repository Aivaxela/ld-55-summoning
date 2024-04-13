using Godot;

public partial class Main : Node
{
    [Export] public Label ratsCollectedLabel;
    int pointTotal = 0;

    public override void _Ready()
    {
        Color bgColor = new(1f, 0.878f, 0.964f);
        RenderingServer.SetDefaultClearColor(bgColor);
    }

    public void IncreaseRatPoints(int points)
    {
        pointTotal += points;
        ratsCollectedLabel.Text = "rats collected: " + pointTotal;
    }
}

using Godot;

public partial class Main : Node
{
    public override void _Ready()
    {
        Color bgColor = new(1f, 0.878f, 0.964f);
        RenderingServer.SetDefaultClearColor(bgColor);
    }
}

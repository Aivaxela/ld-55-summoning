using Godot;

public partial class Menu : Node2D
{
    [Export] Button goButton;

    public override void _Ready()
    {
        goButton.Pressed += OnButtonPressed;
    }

    private void OnButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/main.tscn");
    }
}

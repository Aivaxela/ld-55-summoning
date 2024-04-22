using Godot;

public partial class CounterItem : AnimatedSprite2D
{
    [Export] Area2D area;
    [Export] AnimationPlayer animPlayer;
    Main main;

    public override void _Ready()
    {
        main = GetNode<Main>("/root/main/");
        area.AreaEntered += AreaEntered;
        Frame = GD.RandRange(0, 4);
    }

    private void AreaEntered(object _)
    {
        animPlayer.Play("falling");
        main.energy += 2;
    }
}

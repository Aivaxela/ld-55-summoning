using Godot;

public partial class EnergyPickup : CharacterBody2D
{
    [Export] public float speed;
    [Export] Area2D area;
    [Export] AudioStreamPlayer pickupSFX;
    Main main;
    Cat cat;
    public Vector2 velocity = Vector2.Zero;

    public override void _Ready()
    {
        cat = GetNode<Cat>("/root/main/cat");
        main = GetNode<Main>("/root/main/");

        area.AreaEntered += OnAreaEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        CalculateVelocity();
        Velocity = velocity;
        MoveAndSlide();
        CleanUp();
    }

    private void CalculateVelocity()
    {
        velocity.X = speed;
    }

    private void OnAreaEntered(object _)
    {
        RemoveChild(pickupSFX);
        GetParent().AddChild(pickupSFX);
        pickupSFX.Play();
        main.energy += 40;
        DestroyAndReset();
    }
    private void CleanUp()
    {
        if (GlobalPosition.X <= -1200) DestroyAndReset();
    }

    private void DestroyAndReset()
    {
        QueueFree();
    }
}

using Godot;

public partial class Rat : CharacterBody2D
{
    [Export] public float speed;
    [Export] Area2D area;
    Main main;
    Cat cat;
    Spawner ratSpawner;
    public Vector2 velocity = Vector2.Zero;

    public override void _Ready()
    {
        ratSpawner = GetNode<Spawner>("/root/main/spawners/spawner-rat");
        cat = GetNode<Cat>("/root/main/cat");
        main = GetNode<Main>("/root/main/");

        ratSpawner.canSpawn = false;
        area.AreaEntered += OnAreaEntered;
    }

    public override void _Process(double delta)
    {
        AdjustSpeed();
        CalculateVelocity();
        Velocity = velocity;
        MoveAndSlide();
        CleanUp();
    }

    private void AdjustSpeed()
    {
        if (cat.isSpotted == true)
        {
            speed = -500;
        }
        else
        {
            speed = 0;
        }
    }

    private void CalculateVelocity()
    {
        velocity.X = speed;
    }

    private void OnAreaEntered(object _)
    {
        main.IncreaseRatPoints(1);
        DestroyAndReset();
    }

    private void CleanUp()
    {
        if (GlobalPosition.X <= -1200) DestroyAndReset();
    }

    private void DestroyAndReset()
    {
        ratSpawner.canSpawn = true;
        QueueFree();
    }
}

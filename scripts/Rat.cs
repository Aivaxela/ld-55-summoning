using Godot;

public partial class Rat : CharacterBody2D
{
    public enum State { RUNNING, GRABBED }
    public State currentRatState;

    public float speed;
    [Export] float gravity;
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

        currentRatState = State.RUNNING;

        ratSpawner.canSpawn = false;
        area.AreaEntered += OnAreaEntered;
    }

    public override void _Process(double delta)
    {
        CleanUp();

        switch (currentRatState)
        {
            case State.RUNNING:
                _Process_Running(delta);
                break;
            case State.GRABBED:
                _Process_Grabbed(delta);
                break;
        }
    }

    public void _Process_Running(double delta)
    {
        AdjustSpeed();
        CalculateVelocity();
        Velocity = velocity;
        MoveAndSlide();
    }

    public void _Process_Grabbed(double delta)
    {
        velocity = Vector2.Zero;
    }


    private void AdjustSpeed()
    {
        if (cat.isSpotted == true)
        {
            speed = -300;
        }
        else
        {
            speed = -40;
        }
    }

    private void CalculateVelocity()
    {
        velocity.Y += gravity;
        if (IsOnFloor()) velocity.Y = 0;
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

    public void DestroyAndReset()
    {
        ratSpawner.canSpawn = true;
        QueueFree();
    }
}

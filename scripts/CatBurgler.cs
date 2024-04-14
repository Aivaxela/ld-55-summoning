using System;
using Godot;

public partial class CatBurgler : CharacterBody2D
{
    [Export] public float speed;
    [Export] Area2D area;
    [Export] Sprite2D claw;
    float clawYPos;
    Main main;
    Cat cat;
    Rat rat;
    Spawner catBurgSpawner;
    public Vector2 velocity = Vector2.Zero;

    public override void _Ready()
    {
        catBurgSpawner = GetNode<Spawner>("/root/main/spawners/spawner-cat-burgler");
        cat = GetNode<Cat>("/root/main/cat");
        main = GetNode<Main>("/root/main/");

        catBurgSpawner.canSpawn = false;
        area.AreaEntered += OnAreaEntered;
    }

    public override void _Process(double delta)
    {
        CalculateVelocity();
        Velocity = velocity;
        MoveAndSlide();
        CleanUp();
        ActivateClaw();
    }

    private void CalculateVelocity()
    {
        rat = (Rat)GetTree().GetFirstNodeInGroup("rat");
        if (rat == null || rat.GlobalPosition.X > 400)
        {
            velocity = new Vector2(200, 0);
            return;
        }
        velocity.X = RatLocation() > GlobalPosition.X ? speed : -speed;
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
        catBurgSpawner.canSpawn = true;
        QueueFree();
    }

    private float RatLocation()
    {
        if (rat == null || rat.GlobalPosition.X > 400) return 0;
        return rat.GlobalPosition.X;
    }

    private void ActivateClaw()
    {
        if (rat != null && rat.GlobalPosition.X > 400)
        {
            clawYPos = Mathf.Lerp(claw.GlobalPosition.Y, rat.GlobalPosition.Y, 1f);
            claw.GlobalPosition = new Vector2(GlobalPosition.X, clawYPos);
        }
        else
        {
            GD.Print("this");
            clawYPos = Mathf.Lerp(claw.GlobalPosition.Y, GlobalPosition.Y, 1f);
            claw.Position = new Vector2(-2, 52);
        }
    }
}

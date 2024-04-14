using System;
using Godot;

public partial class CatBurgler : CharacterBody2D
{
    [Export] public float speed;
    [Export] Area2D area;
    [Export] Sprite2D claw;
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
        FlipSprite();
        PullRat();
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

    private void OnAreaEntered(Area2D area)
    {
        Rat rat = (Rat)area.GetParent();
        rat.currentRatState = Rat.State.GRABBED;
    }

    private void CleanUp()
    {
        if (GlobalPosition.X <= -1200 || GlobalPosition.X >= 800) DestroyAndReset();
    }

    private void DestroyAndReset()
    {
        rat.DestroyAndReset();
        catBurgSpawner.canSpawn = true;
        QueueFree();
    }

    public void HitByCatAttack()
    {
        rat.currentRatState = Rat.State.RUNNING;
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
        if (rat == null || rat.currentRatState == Rat.State.GRABBED)
        {
            RetractClaw();
        }

        else if (rat != null && rat.GlobalPosition.X < 400)
        {
            SendClaw();
        }
    }

    private void RetractClaw()
    {
        Vector2 clawOrigPos = new Vector2(-2, 52);
        claw.Position = new Vector2(claw.Position.X, Mathf.Lerp(claw.Position.Y, clawOrigPos.Y, 0.01f));
    }

    private void SendClaw()
    {
        Vector2 direction = (rat.GlobalPosition - claw.GlobalPosition).Normalized();
        Vector2 movement = direction * 1.1f;
        claw.GlobalPosition += movement;
    }

    private void PullRat()
    {
        if (rat == null) return;
        if (rat.currentRatState != Rat.State.GRABBED) return;
        rat.GlobalPosition = claw.GlobalPosition;
    }

    private void FlipSprite()
    {
        Sprite2D sprite = GetNode<Sprite2D>("body");
        if (velocity.X > 0) sprite.FlipH = true;
        else sprite.FlipH = false;
    }
}

using Godot;
using System;

public partial class Obstacle : CharacterBody2D
{
    [Export] float speed;
    [Export] Area2D couchJumpArea;
    public Vector2 velocity = Vector2.Zero;
    Cat cat;

    public override void _Ready()
    {
        cat = GetNode<Cat>("/root/main/cat");
        if (couchJumpArea != null) couchJumpArea.AreaEntered += CouchJumpAreaEntered;
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
            speed = -300;
        }
    }

    private void CalculateVelocity()
    {
        velocity.X = speed;
    }

    private void CleanUp()
    {
        if (GlobalPosition.X <= -1200)
        {
            QueueFree();
        }
    }

    private void CouchJumpAreaEntered(object _)
    {
        cat.forcedJump = true;
    }
}

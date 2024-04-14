using Godot;
using System;

public partial class EnergyPickup : CharacterBody2D
{
    [Export] public float speed;
    [Export] Area2D area;
    Main main;
    Cat cat;
    Spawner floatingEnergySpawner;
    public Vector2 velocity = Vector2.Zero;

    public override void _Ready()
    {
        cat = GetNode<Cat>("/root/main/cat");
        main = GetNode<Main>("/root/main/");
        floatingEnergySpawner = GetNode<Spawner>("/root/main/spawners/spawner-floating-energy");

        floatingEnergySpawner.canSpawn = false;
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
            speed = -200;
        }
        else
        {
            speed = -100;
        }
    }

    private void CalculateVelocity()
    {
        velocity.X = speed;
    }

    private void OnAreaEntered(object _)
    {
        main.energy += 40;
        DestroyAndReset();
    }
    private void CleanUp()
    {
        if (GlobalPosition.X <= -1200) DestroyAndReset();
    }

    private void DestroyAndReset()
    {
        floatingEnergySpawner.canSpawn = true;
        QueueFree();
    }
}

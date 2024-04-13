using Godot;
using System;

public partial class Spawner : Marker2D
{
    [Export] int spawnRateMin;
    [Export] int spawnRateMax;
    [Export] public bool canSpawn = true;
    [Export] PackedScene[] spawnScene;
    [Export] Timer timeUntilSpawnTimer;

    public override void _Ready()
    {
        timeUntilSpawnTimer.OneShot = true;
        timeUntilSpawnTimer.WaitTime = GD.RandRange(spawnRateMin, spawnRateMax);
        timeUntilSpawnTimer.Start();
    }

    public override void _Process(double delta)
    {
        if (timeUntilSpawnTimer.TimeLeft == 0 && canSpawn)
        {
            CallDeferred("Spawn");
            timeUntilSpawnTimer.WaitTime = GD.RandRange(spawnRateMin, spawnRateMax);
            timeUntilSpawnTimer.Start();
        }
    }

    private void Spawn()
    {
        CharacterBody2D newSpawn = (CharacterBody2D)spawnScene[0].Instantiate();
        GetParent().AddChild(newSpawn);
        newSpawn.GlobalPosition = GlobalPosition;
    }
}

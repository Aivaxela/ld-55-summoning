using Godot;
using System;

public partial class ParallaxBackground : Godot.ParallaxBackground
{
    [Export] float xScrollSpeed;
    [Export] Cat cat;

    float xOffset;

    public override void _Ready()
    {
        xOffset = ScrollOffset.X;
    }

    public override void _Process(double delta)
    {
        AdjustSpeed();
        ScrollOffset = new Vector2(xOffset, ScrollOffset.Y);
        xOffset -= xScrollSpeed;
    }

    private void AdjustSpeed()
    {
        if (cat.isSpotted == true)
        {
            xScrollSpeed = 2.3f;
        }
        else
        {
            xScrollSpeed = 1.6f;
        }
    }
}

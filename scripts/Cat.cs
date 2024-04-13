using System.Net.NetworkInformation;
using Godot;

public partial class Cat : CharacterBody2D
{
    [Export] public Sprite2D sprite;
    [Export] Texture2D catBlack;
    [Export] Texture2D catSpotted;
    [Export] float gravity;
    [Export] public float jumpStrength;
    public bool isSpotted = false;
    public Vector2 velocity = Vector2.Zero;
    bool leftCeiling = false;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        CalculateVelocity();
        ListenForInputs();
        Velocity = velocity;
        MoveAndSlide();
        velocity.X = 0;
        GlobalPosition = new Vector2(-380f, GlobalPosition.Y);
    }

    private void CalculateVelocity()
    {
        velocity.Y += gravity;
        if (IsOnFloor())
        {
            leftCeiling = false;
            velocity.Y = 0;
        }
        if (IsOnCeiling() && !leftCeiling)
        {
            leftCeiling = true;
            velocity.Y = 0;
        }
    }

    private void ListenForInputs()
    {
        if (Input.IsActionJustPressed("jump") && IsOnFloor())
        {
            velocity.Y = 0 - jumpStrength;
        }

        if (Input.IsActionJustPressed("transform-1"))
        {
            if (sprite.Texture == catBlack) return;
            sprite.Texture = catBlack;
            isSpotted = false;
        }
        if (Input.IsActionJustPressed("transform-2"))
        {
            if (sprite.Texture == catSpotted) return;
            sprite.Texture = catSpotted;
            isSpotted = true;
        }
    }

    public void CouchJumpAreaEntered()
    {
        GlobalPosition = new Vector2(GlobalPosition.X, 64);
    }
}
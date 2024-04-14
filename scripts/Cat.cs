using System.Data.Common;
using System.Net.NetworkInformation;
using Godot;

public partial class Cat : CharacterBody2D
{
    [Export] public Sprite2D sprite;
    [Export] Texture2D catBlack;
    [Export] Texture2D catSpotted;
    [Export] Texture2D catPoofy1;
    [Export] Texture2D catPoofy2;
    [Export] AnimationPlayer animPlayer;
    [Export] float gravity;
    [Export] public float jumpStrength;
    Main main;
    public bool isSpotted = false;
    public Vector2 velocity = Vector2.Zero;
    bool leftCeiling = false;
    public bool forcedJump = false;
    bool hasExtraJump = false;

    public override void _Ready()
    {
        main = GetNode<Main>("/root/main/");
    }

    public override void _Process(double delta)
    {
        CalculateVelocity();
        ListenForInputs();
        Velocity = velocity;
        MoveAndSlide();
        velocity.X = 0;
        GlobalPosition = new Vector2(-380f, GlobalPosition.Y);
        ResetPoofyCat();
        UpdateAnimations();
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
        if (Input.IsActionJustPressed("jump"))
        {
            if (IsOnFloor())
            {
                velocity.Y = 0 - jumpStrength;
                forcedJump = false;
            }
            else if (hasExtraJump && sprite.Texture == catPoofy1)
            {
                velocity.Y = 0 - jumpStrength;
                forcedJump = false;
                hasExtraJump = false;
                sprite.Texture = catPoofy2;
            }

        }
        if (forcedJump)
        {
            velocity.Y = 0 - jumpStrength;
            forcedJump = false;
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
            if (main.energy < 20) return;
            sprite.Texture = catSpotted;
            isSpotted = true;
            main.energy -= 20;
        }
        if (Input.IsActionJustPressed("transform-3"))
        {
            if (sprite.Texture == catPoofy1) return;
            if (main.energy < 20) return;
            sprite.Texture = catPoofy1;
            main.energy -= 20;
            isSpotted = false;
        }
    }

    public void CouchJumpAreaEntered()
    {
        forcedJump = true;
    }

    private void ResetPoofyCat()
    {
        if (IsOnFloor())
        {
            hasExtraJump = true;
            if (sprite.Texture == catPoofy2) sprite.Texture = catPoofy1;
        }
    }

    private void UpdateAnimations()
    {
        if (!IsOnFloor()) animPlayer.Play(velocity.Y > 0 ? "falling-down" : "jumping-up");
        else animPlayer.Play("running");
    }
}
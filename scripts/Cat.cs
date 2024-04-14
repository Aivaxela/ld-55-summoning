using System.Data.Common;
using System.Net.NetworkInformation;
using Godot;

public partial class Cat : CharacterBody2D
{
    [Export] public AnimatedSprite2D animSprite;
    [Export] Sprite2D sprite;
    [Export] AnimationPlayer animPlayer;
    [Export] PackedScene catAttackAttackScene;
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
            else if (hasExtraJump && animSprite.Animation == "poofy")
            {
                velocity.Y = 0 - jumpStrength;
                forcedJump = false;
                hasExtraJump = false;
                sprite.Visible = true;
                animSprite.Visible = false;
            }

        }
        if (forcedJump)
        {
            velocity.Y = 0 - jumpStrength;
            forcedJump = false;
        }

        CatAttackAttack();
        SummonCat();
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
            if (sprite.Visible)
            {
                animSprite.Animation = "poofy";
                animSprite.Visible = true;
                sprite.Visible = false;
            }
        }
    }

    private void UpdateAnimations()
    {
        if (!IsOnFloor()) animPlayer.Play(velocity.Y > 0 ? "falling-down" : "jumping-up");
        else animPlayer.Play("running");
    }

    private void CatAttackAttack()
    {
        if (Input.IsActionJustPressed("cat-attack-attack") && animSprite.Animation == "attack")
        {
            CatAttackAttack catAttackAttack = (CatAttackAttack)catAttackAttackScene.Instantiate();
            GetParent().AddChild(catAttackAttack);
            catAttackAttack.GlobalPosition = GlobalPosition;
            catAttackAttack.SetDirAndVel(GetGlobalMousePosition());
            animSprite.Animation = "black";
            animSprite.Visible = true;
            sprite.Visible = false;

        }
    }

    private void SummonCat()
    {
        if (Input.IsActionJustPressed("transform-1"))
        {
            if (animSprite.Animation == "black") return;
            animSprite.Animation = "black";
            isSpotted = false;
            animSprite.Visible = true;
            sprite.Visible = false;
        }
        if (Input.IsActionJustPressed("transform-2"))
        {
            if (animSprite.Animation == "spotted") return;
            if (main.energy < 20) return;
            animSprite.Animation = "spotted";
            isSpotted = true;
            main.energy -= 20;
            animSprite.Visible = true;
            sprite.Visible = false;
        }
        if (Input.IsActionJustPressed("transform-3"))
        {
            if (animSprite.Animation == "poofy") return;
            if (main.energy < 20) return;
            animSprite.Animation = "poofy";
            main.energy -= 20;
            isSpotted = false;
            animSprite.Visible = true;
            sprite.Visible = false;
        }
        if (Input.IsActionJustPressed("transform-4"))
        {
            if (animSprite.Animation == "attack") return;
            if (main.energy < 20) return;
            animSprite.Animation = "attack";
            main.energy -= 20;
            isSpotted = false;
            animSprite.Visible = true;
            sprite.Visible = false;
        }
    }
}
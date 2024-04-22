using Godot;

public partial class Cat : CharacterBody2D
{
    [Export] public AnimatedSprite2D animSprite;
    [Export] Sprite2D sprite;
    [Export] AnimationPlayer animPlayer;
    [Export] PackedScene catAttackAttackScene;
    [Export] float gravity;
    [Export] public float jumpStrength;
    [Export] Area2D obstacleArea;
    [Export] GpuParticles2D transformParticles;
    [Export] AudioStreamPlayer catAttackAttackSFX;
    [Export] AudioStreamPlayer catTrasnformSFX;
    [Export] AudioStreamPlayer catLandingSFX;
    [Export] AudioStreamPlayer catJumpSFX;
    Main main;
    public bool isFastAfBoi = false;
    public Vector2 velocity = Vector2.Zero;
    bool leftCeiling = false;
    public bool forcedJump = false;
    bool hasExtraJump = false;
    uint colLayerInitVal;
    bool isOnFloorForFirstTime = true;

    public override void _Ready()
    {
        main = GetNode<Main>("/root/main/");
        colLayerInitVal = obstacleArea.CollisionLayer;
    }

    public override void _PhysicsProcess(double delta)
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
            if (isOnFloorForFirstTime)
            {
                catLandingSFX.Play();
                isOnFloorForFirstTime = false;
            }
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
                catJumpSFX.Play();
                isOnFloorForFirstTime = true;
            }
            else if (hasExtraJump && animSprite.Animation == "poofy")
            {
                velocity.Y = 0 - jumpStrength;
                forcedJump = false;
                hasExtraJump = false;
                sprite.Visible = true;
                animSprite.Visible = false;
                catJumpSFX.Play();
                isOnFloorForFirstTime = true;
            }
        }
        if (forcedJump)
        {
            velocity.Y = 0 - jumpStrength;
            forcedJump = false;
            catJumpSFX.Play();
            isOnFloorForFirstTime = true;
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
            catAttackAttackSFX.Play();
        }
    }

    private void SummonCat()
    {
        if (Input.IsActionJustPressed("transform-1"))
        {
            if (animSprite.Animation == "black") return;
            animSprite.Animation = "black";
            isFastAfBoi = false;
            animSprite.Visible = true;
            sprite.Visible = false;
            obstacleArea.CollisionLayer = colLayerInitVal;
            jumpStrength = 1200;
            transformParticles.Restart();
            catTrasnformSFX.Play();
        }
        if (Input.IsActionJustPressed("transform-2"))
        {
            if (animSprite.Animation == "spotted") return;
            if (main.energy < 20) return;
            animSprite.Animation = "spotted";
            isFastAfBoi = true;
            main.energy -= 20;
            animSprite.Visible = true;
            sprite.Visible = false;
            obstacleArea.CollisionLayer = colLayerInitVal;
            jumpStrength = 1200;
            transformParticles.Restart();
            catTrasnformSFX.Play();
        }
        if (Input.IsActionJustPressed("transform-3"))
        {
            if (animSprite.Animation == "poofy") return;
            if (main.energy < 20) return;
            animSprite.Animation = "poofy";
            main.energy -= 20;
            isFastAfBoi = false;
            animSprite.Visible = true;
            sprite.Visible = false;
            obstacleArea.CollisionLayer = colLayerInitVal;
            jumpStrength = 1200;
            transformParticles.Restart();
            catTrasnformSFX.Play();
        }
        if (Input.IsActionJustPressed("transform-4"))
        {
            if (animSprite.Animation == "attack") return;
            if (main.energy < 20) return;
            animSprite.Animation = "attack";
            main.energy -= 20;
            isFastAfBoi = false;
            animSprite.Visible = true;
            sprite.Visible = false;
            obstacleArea.CollisionLayer = colLayerInitVal;
            jumpStrength = 1200;
            transformParticles.Restart();
            catTrasnformSFX.Play();
        }
        if (Input.IsActionJustPressed("transform-5"))
        {
            if (animSprite.Animation == "mini") return;
            if (main.energy < 20) return;
            animSprite.Animation = "mini";
            main.energy -= 20;
            isFastAfBoi = true;
            animSprite.Visible = true;
            sprite.Visible = false;
            obstacleArea.CollisionLayer = colLayerInitVal - 2;
            jumpStrength = 600;
            transformParticles.Restart();
            catTrasnformSFX.Play();
        }
    }
}
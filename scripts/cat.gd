extends CharacterBody2D

# Exported variables
@export var animSprite: AnimatedSprite2D
@export var sprite: Sprite2D
@export var animPlayer: AnimationPlayer
@export var catAttackAttackScene: PackedScene
@export var gravity: float
@export var jumpStrength: float
@export var obstacleArea: Area2D
@export var transformParticles: GPUParticles2D
@export var catAttackAttackSFX: AudioStreamPlayer
@export var catTransformSFX: AudioStreamPlayer
@export var catLandingSFX: AudioStreamPlayer
@export var catJumpSFX: AudioStreamPlayer

# Member variables
var main
var isFastAfBoi: bool = false
var leftCeiling: bool = false
var forcedJump: bool = false
var hasExtraJump: bool = false
var colLayerInitVal: int
var isOnFloorForFirstTime: bool = true

func _ready():
	main = get_node("/root/main/")
	#colLayerInitVal = obstacleArea.collision_layer

func _physics_process(_delta):
	calculate_velocity()
	listen_for_inputs()
	velocity.x = 0
	move_and_slide()
	global_position.x = -380  # Set the X position directly
	reset_poofy_cat()
	update_animations()

func calculate_velocity():
	velocity.y += gravity
	if is_on_floor():
		leftCeiling = false
		velocity.y = 0
		if isOnFloorForFirstTime:
			catLandingSFX.play()
			isOnFloorForFirstTime = false
	if is_on_ceiling() and not leftCeiling:
		leftCeiling = true
		velocity.y = 0

func listen_for_inputs():
	if Input.is_action_just_pressed("jump"):
		if is_on_floor():
			velocity.y = -jumpStrength
			forcedJump = false
			catJumpSFX.play()
			isOnFloorForFirstTime = true
		elif hasExtraJump and animSprite.animation == "poofy":
			velocity.y = -jumpStrength
			forcedJump = false
			hasExtraJump = false
			sprite.visible = true
			animSprite.visible = false
			catJumpSFX.play()
			isOnFloorForFirstTime = true

	if forcedJump:
		velocity.y = -jumpStrength
		forcedJump = false
		catJumpSFX.play()
		isOnFloorForFirstTime = true

	catAttackAttack()
	summon_cat()

func couch_jump_area_entered():
	forcedJump = true

func reset_poofy_cat():
	if is_on_floor():
		hasExtraJump = true
		if sprite.visible:
			animSprite.animation = "poofy"
			animSprite.visible = true
			sprite.visible = false

func update_animations():
	if not is_on_floor():
		animPlayer.play("jumping-up" if velocity.y > 0 else "falling-down")
	else:
		animPlayer.play("running")

func catAttackAttack():
	if Input.is_action_just_pressed("cat-attack-attack") and animSprite.animation == "attack":
		var catAttackAttack = catAttackAttackScene.instantiate()
		get_parent().add_child(catAttackAttack)
		catAttackAttack.global_position = global_position
		catAttackAttack.set_dir_and_vel(get_global_mouse_position())
		animSprite.animation = "black"
		animSprite.visible = true
		sprite.visible = false
		catAttackAttackSFX.play()

func summon_cat():
	if Input.is_action_just_pressed("transform-1"):
		if animSprite.animation == "black":
			return
		animSprite.animation = "black"
		isFastAfBoi = false
		animSprite.visible = true
		sprite.visible = false
		obstacleArea.collision_layer = colLayerInitVal
		jumpStrength = 1200
		transformParticles.restart()
		catTransformSFX.play()

	if Input.is_action_just_pressed("transform-2"):
		if animSprite.animation == "spotted":
			return
		animSprite.animation = "spotted"
		isFastAfBoi = true
		main.energy -= 20
		animSprite.visible = true
		sprite.visible = false
		obstacleArea.collision_layer = colLayerInitVal
		jumpStrength = 1200
		transformParticles.restart()
		catTransformSFX.play()
		
	if Input.is_action_just_pressed("transform-3"):
		if animSprite.animation == "poofy":
			return
		animSprite.animation = "poofy"
		isFastAfBoi = false
		main.energy -= 20
		animSprite.visible = true
		sprite.visible = false
		obstacleArea.collision_layer = colLayerInitVal
		jumpStrength = 1200
		transformParticles.restart()
		catTransformSFX.play()
		
	if Input.is_action_just_pressed("transform-4"):
		if animSprite.animation == "attack":
			return
		animSprite.animation = "attack"
		isFastAfBoi = false
		main.energy -= 20
		animSprite.visible = true
		sprite.visible = false
		obstacleArea.collision_layer = colLayerInitVal
		jumpStrength = 1200
		transformParticles.restart()
		catTransformSFX.play()
		
	if Input.is_action_just_pressed("transform-5"):
		if animSprite.animation == "mini":
			return
		animSprite.animation = "mini"
		isFastAfBoi = true
		main.energy -= 20
		animSprite.visible = true
		sprite.visible = false
		obstacleArea.collision_layer = colLayerInitVal - 2
		jumpStrength = 600
		transformParticles.restart()
		catTransformSFX.play()

extends Node

# Exported variables
@export var ratsCollectedLabel: Label
@export var energyRefillTimer: Timer
@export var energyBar: TextureProgressBar
@export var energyBarReady: Texture
@export var energyBarNotReady: Texture

# Member variables
var pointTotal: int = 0
var energy: int = 100

func _ready():
	var bgColor = Color(1.0, 0.878, 0.964)
	RenderingServer.set_default_clear_color(bgColor)
	energyRefillTimer.timeout.connect(_on_energy_refill_timer_timeout)

func _process(_delta):
	refill_energy()
	
	if energy > 100:
		energy = 100

func increase_rat_points(newPoints: int):
	pointTotal += newPoints
	ratsCollectedLabel.text = "rats collected: " + str(pointTotal)

func _on_energy_refill_timer_timeout():
	energy += 1

func refill_energy():
	energyBar.value = energy
	energyBar.texture_progress = energyBarReady if energyBar.value >= 20 else energyBarNotReady

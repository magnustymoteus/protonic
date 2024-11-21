extends Control

const ParticleBeam := preload("res://utils/ParticleBeam.cs")
var envelope : ParticleBeam
var x : float = 0.0
var plot

func _init() -> void:
	envelope = ParticleBeam.new()
	envelope.Alpha = -1
	envelope.Beta = 4
	envelope.Emittance = 7e-5

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	plot = $Graph2D.add_plot_item("Envelope", Color.RED)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	var y = envelope.GetEnvelope(x)
	print(y)
	plot.add_point(Vector2(x,y))
	x += 50

extends Control

const ParticleBeam := preload("res://utils/ParticleBeam.cs")
var envelope : ParticleBeam
var x : float = 0.0
var envelopePlot1
var envelopePlot2
var FODOLattice: Array[String] = ["focusing_quadrupole", "drift", "defocusing_quadrupole", "drift"]
var index: int = 0

var x_bounds: Vector2 = Vector2(0,0)
var y_bounds: Vector2 = Vector2(0,0)

func update_bounds(elem: float, bounds: Vector2) -> Vector2:
	if elem < bounds.x:
		bounds.x = elem
	elif elem > bounds.y:
		bounds.y = elem
	return bounds
	
func set_graph_bounds():
	$Graph2D.x_min = x_bounds.x
	$Graph2D.x_max = x_bounds.y
	$Graph2D.y_min = y_bounds.x
	$Graph2D.y_max = y_bounds.y

func _init() -> void:
	envelope = ParticleBeam.new()
	envelope.Alpha = 0
	envelope.Beta = 2
	envelope.Emittance = 100

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	envelopePlot1 = $Graph2D.add_plot_item("Envelope ", Color.RED)
	envelopePlot2 = $Graph2D.add_plot_item("Envelope min", Color.RED)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if x <= 100:
		var y = envelope.GetEnvelope(x, FODOLattice[index])
		index += 1
		index %= (len(FODOLattice))
		envelopePlot1.add_point(Vector2(x, y))
		envelopePlot2.add_point(Vector2(x, -y))
		x += 10
		
		x_bounds = update_bounds(x, x_bounds)
		y_bounds = update_bounds(y, y_bounds)
		y_bounds = update_bounds(-y, y_bounds)
		set_graph_bounds()
		
		

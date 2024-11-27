extends Control

const ParticleBeam := preload("res://utils/ParticleBeam.cs")
var beam : ParticleBeam

var ellipsePlot

var x_bounds: Vector2 = Vector2(0,0)
var y_bounds: Vector2 = Vector2(0,0)

var s = 0

func update_bounds(elem: float, bounds: Vector2) -> Vector2:
	if elem < bounds.x:
		bounds.x = elem
	elif elem > bounds.y:
		bounds.y = elem
	return bounds

func set_graph_bounds():
	$Graph2D.x_min = x_bounds.x-1
	$Graph2D.x_max = x_bounds.y+1
	$Graph2D.y_min = y_bounds.x-1
	$Graph2D.y_max = y_bounds.y+1

func _init() -> void:
	beam = ParticleBeam.new()
	beam.Alpha = -1.0
	beam.Beta = 5
	beam.Emittance = 1

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	ellipsePlot = $Graph2D.add_plot_item("Ellipse", Color.GREEN)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if s <= 500:
		var positionMomentum = beam.GetPhaseSpace(s, 500)
		ellipsePlot.add_point(positionMomentum)

		x_bounds = update_bounds(positionMomentum.x, x_bounds)
		y_bounds = update_bounds(positionMomentum.y, y_bounds)
		set_graph_bounds()

		s += 1

extends Control

const ParticleBeam := preload("res://utils/ParticleBeam.cs")
var beam : ParticleBeam
var positionMomentum: Vector2 = Vector2(1,1)
var FODOLattice: Array[String] = ["focusing_quadrupole", "drift", "defocusing_quadrupole", "drift"]
var index: int = 0

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
	$Graph2D.x_min = x_bounds.x
	$Graph2D.x_max = x_bounds.y
	$Graph2D.y_min = y_bounds.x
	$Graph2D.y_max = y_bounds.y

func _init() -> void:
	beam = ParticleBeam.new()
	beam.Alpha = 0
	beam.Beta = 10
	beam.Emittance = 1

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	ellipsePlot = $Graph2D.add_plot_item("Ellipse", Color.GREEN)

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if s <= 500:
		positionMomentum = beam.GetPhaseSpace(s, FODOLattice[index], 500)
		ellipsePlot.add_point(positionMomentum)
		print(positionMomentum)
		index += 1
		index %= (len(FODOLattice))

		x_bounds = update_bounds(positionMomentum.x, x_bounds)
		y_bounds = update_bounds(positionMomentum.y, y_bounds)
		set_graph_bounds()

		s += 1

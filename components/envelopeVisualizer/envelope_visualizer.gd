extends Control

const ParticleBeam := preload("res://utils/ParticleBeam.cs")
const BeamlineTube := preload("res://components/particleAccelerator/elements/beamline_tube/BeamlineTube.cs")

var envelope : ParticleBeam

var x : float = 0.0
var xLimit: float = 0.0

var envelopePlot1: PlotItem
var envelopePlot2: PlotItem

var tubeArray: Array = []
var index: int = 0

var x_bounds: Vector2 = Vector2(0,0)
var y_bounds: Vector2 = Vector2(0,0)

var numOfParticlePlots: int = 50
var particlePlots: Array[PlotItem] = []

var stream: bool = false

func switch_emitter():
	stream = !stream

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
	envelope.Alpha = 1
	envelope.Beta = 0.55
	envelope.Emittance = 3.75
	
func reset_plot() -> void:
	$Graph2D.remove_all()
	x = 0.0
	index = 0
	x_bounds = Vector2(0,0)
	y_bounds = Vector2(0,0)
	particlePlots.clear()
	
func set_tubeArray(tubeArrayArg: Array) -> void:
	tubeArray = tubeArrayArg
	reset_plot()
	initialize_plot()
	xLimit = len(tubeArray)*5
	
func initialize_plot() -> void:
	envelopePlot1 = $Graph2D.add_plot_item("Envelope max", Color.RED)
	envelopePlot2 = $Graph2D.add_plot_item("Envelope min", Color.RED)
	for elem in numOfParticlePlots:
		particlePlots.append($Graph2D.add_plot_item(" ", Color.GREEN, 0.5))

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	initialize_plot()
	
func set_params(alpha: float, beta: float, emittance: float) -> void:
	envelope.Alpha = alpha
	envelope.Beta = beta
	envelope.Emittance = emittance
	
func replot() -> void:
	reset_plot()
	initialize_plot()

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta: float) -> void:
	if index < len(tubeArray):
		var envelopeY = envelope.GetEnvelope(tubeArray[index])
		if stream:
			for particlePlot in particlePlots:
				if particlePlot._points.size() > 0:
					print(particlePlot._points[-1])
					print(envelopeY)
					print()
					if abs(particlePlot._points[-1].y) == abs(envelopeY):
						continue
				var particleY: float = envelope.GetEnvelope(tubeArray[index]) * envelope.GetRandomFactor()
				particlePlot.add_point(Vector2(x, particleY))
		index += 1
		envelopePlot1.add_point(Vector2(x, envelopeY))
		envelopePlot2.add_point(Vector2(x, -envelopeY))
		
		x_bounds = update_bounds(x, x_bounds)
		y_bounds = update_bounds(envelopeY, y_bounds)
		y_bounds = update_bounds(-envelopeY, y_bounds)
		set_graph_bounds()
		
		x += 1
		

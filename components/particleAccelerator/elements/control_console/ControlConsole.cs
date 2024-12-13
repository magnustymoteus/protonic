using Godot;
using System.Linq;
using protonic;

public partial class ControlConsole : Element
{
	private Control _canvasChild, _instance;
	
	public ParticleEmitter ConnectedEmitter;

	public ParticleEmitter GetEmitter()
	{
		return (ParticleEmitter)GetFilteredConnectedElements<ParticleEmitter>().FirstOrDefault();
	}

	public override void _Ready()
	{
		var task = GD.Load<PackedScene>("res://components/particleAccelerator/elements/control_console/control_console.tscn");
		
		_instance = task.Instantiate<Control>();
		
		_canvasChild = GetNode<Control>("/root/Node2D/CanvasLayer/mainUI");
		_canvasChild.SetAnchorsPreset(Control.LayoutPreset.Center);
		_instance.GetNode<ControlConsoleWindow>("./").ControlConsole = this;
	}
	public ControlConsole(string path, Vector2I position, Vector2I size, float rotation) : base(path, position, size,
		rotation) { }
	public override void _Input(InputEvent @event)
	{
		if(Input.IsActionJustPressed("element_click") && new Rect2(beginPosition*32, (endPosition-beginPosition)*32).HasPoint(GetGlobalMousePosition()))
		{
			if (!_canvasChild.Visible)
			{
				foreach (var child in _canvasChild.GetChildren())
				{
					_canvasChild.RemoveChild(child);
				}	
				_canvasChild.SetVisible(true);
				_canvasChild.AddChild(_instance);
				_instance.GetNode<ControlConsoleWindow>("./").UpdateVisualization();
			}
		}
	}
}

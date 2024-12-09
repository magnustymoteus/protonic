using Godot;
using System.Linq;
using Godot.Collections;
using protonic;
using protonic.utils;

public partial class ControlConsole : Element
{
	private Control _canvasChild, _envelopeVisualizer, _instance;
	public Array<BeamlineTube> GetBeamline()
	{
		Array<BeamlineTube> result = new Array<BeamlineTube>();
		if (occupiedElementConnections.Count == 1)
		{
			// here we assume control console is connected to a single beamline tube element
			BeamlineTube currentTube = (BeamlineTube)occupiedElementConnections.Values.ToArray()[0];
			result.Add(currentTube);
			while(currentTube.occupiedElementConnections.Count > 1) {
				currentTube = (BeamlineTube)currentTube.occupiedElementConnections.Values.ToArray()[1];
				result.Add(currentTube);
				if (currentTube.name.Split("/").Where(x => x != "").ToArray()[0] != "beamline_tube") break;
			}
		}
		return result;
	}

	public override void _Ready()
	{
		var task = GD.Load<PackedScene>("res://components/particleAccelerator/elements/control_console/control_console.tscn");
		_instance = task.Instantiate<Control>();
		_canvasChild = GetNode<Control>("/root/Node2D/CanvasLayer/mainUI");
		_canvasChild.SetAnchorsPreset(Control.LayoutPreset.Center);
		_envelopeVisualizer = _instance.GetNode<Control>("./TabContainer/EnvelopeVisualizer");
	}
	public ControlConsole(string path, Vector2I position, Vector2I size, float rotation) : base(path, position, size,
		rotation) { }
	public override void _Input(InputEvent @event)
	{
		if(Input.IsActionJustPressed("element_click") && new Rect2(rectBeginPosition*32, rectEndPosition*32).HasPoint(GetGlobalMousePosition()))
		{
			if (!_canvasChild.Visible)
			{
				foreach (var child in _canvasChild.GetChildren())
				{
					_canvasChild.RemoveChild(child);
				}	
				_canvasChild.SetVisible(true);
				_canvasChild.AddChild(_instance);
				_envelopeVisualizer.Call("set_tubeArray", GetBeamline());
			}
		}
	}
}

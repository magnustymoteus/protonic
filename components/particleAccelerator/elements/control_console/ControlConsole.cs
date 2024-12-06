using Godot;
using System.Linq;
using Godot.Collections;
using protonic;
using protonic.utils;

public partial class ControlConsole : Element
{
	private Control _canvasChild, _envelopeVisualizer;
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
		Control instance = task.Instantiate<Control>();
		_canvasChild = GetNode<Control>("/root/Node2D/CanvasLayer/mainUI");
		_canvasChild.AddChild(instance);
		_canvasChild.SetAnchorsPreset(Control.LayoutPreset.Center);
		_envelopeVisualizer = instance.GetNode<Control>("./TabContainer/EnvelopeVisualizer");
	}
	public ControlConsole(string path, Vector2I position, Vector2I size, float rotation) : base(path, position, size,
		rotation) { }
	public override void _UnhandledInput(InputEvent @event)
	{
		if(Input.IsActionJustPressed("element_click"))
		{
			if (!_canvasChild.Visible)
			{
				// TODO: control console window sometimes not appearing after click
				_canvasChild.SetVisible(true);
				_envelopeVisualizer.Call("set_tubeArray", GetBeamline());
			}
			else
			{
				_canvasChild.SetVisible(false);
			}
		}
	}
}

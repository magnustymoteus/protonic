using Godot;
using System;
using protonic;

public partial class ControlConsole : Element
{
	private Control _canvasChild;

	public override void _Ready()
	{
		var task = GD.Load<PackedScene>("res://components/particleAccelerator/elements/control_console/control_console.tscn");
		Control instance = task.Instantiate<Control>();
		_canvasChild = GetNode<Control>("/root/Node2D/CanvasLayer/mainUI");
		_canvasChild.AddChild(instance);
		_canvasChild.SetAnchorsPreset(Control.LayoutPreset.Center);
	}
	public ControlConsole(string path, Vector2I position, Vector2I size, float rotation) : base(path, position, size,
		rotation) { }
	public override void _UnhandledInput(InputEvent @event)
	{
		if(Input.IsActionJustPressed("element_click"))
		{
			if (!_canvasChild.Visible)
			{
				_canvasChild.SetVisible(true);
			}
			else
			{
				_canvasChild.SetVisible(false);
			}
		}
	}
}

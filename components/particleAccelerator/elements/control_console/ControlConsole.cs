using Godot;
using System;
using protonic;

public partial class ControlConsole : Element
{
	private bool _windowOpen;
	public ControlConsole(string path, Vector2I position, Vector2I size, float rotation) : base(path, position, size,
		rotation)
	{
		
	}
	public override void _UnhandledInput(InputEvent @event)
	{
		if(Input.IsActionJustPressed("element_click") && !_windowOpen)
		{
			var task = GD.Load<PackedScene>("res://components/particleAccelerator/elements/control_console/control_console.tscn");
			Control instance = task.Instantiate<Control>();
			instance.SetScale(new Vector2(0.5f, 0.5f));
			instance.SetPosition(new Vector2(1920.0f/4.0f, 1080.0f/4.0f));
			AddChild(instance);
			_windowOpen = true;
			instance.GrabFocus();
		}
	}
}

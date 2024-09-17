using Godot;
using System;

public partial class BuildButton : CheckButton
{
	[Export] private Node2D buildUI;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Toggled += OnToggle;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnToggle(bool toggle)
	{
		buildUI.SetVisible(toggle);
	}
}

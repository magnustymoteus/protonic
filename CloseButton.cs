using Godot;
using System;

public partial class CloseButton : Button
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Pressed += Close;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void Close()
	{
		GetParent<Node2D>().SetVisible(false);		
	}
}

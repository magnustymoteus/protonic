using Godot;
using System;

public partial class InfoUi : Control
{
	[Export] private Button closeButton; 
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		closeButton.Pressed += Close;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Close()
	{
		SetVisible(false);
	}
}

using Godot;
using System;

public partial class MenuButton: Button
{
	// Called when the node enters the scene tree for the first time.
	[Export] public PackedScene Scene;
	[Export] public bool QuitOnPress;
	public override void _Ready()
	{
		Pressed += ChangeScene;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void ChangeScene()
	{
		if(QuitOnPress) GetTree().Quit();
		else if(Scene != null) GetTree().ChangeSceneToPacked(Scene);
	}
}

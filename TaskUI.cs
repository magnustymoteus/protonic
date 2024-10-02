using Godot;
using System;
using protonic.utils;
public partial class TaskUI : Control
{
	// Called when the node enters the scene tree for the first time.
	public Tree TaskTree;
	public override void _Ready()
	{
		TaskTree = GetChild<Tree>(0);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}

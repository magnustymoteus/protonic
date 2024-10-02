using Godot;
using System;
using protonic.utils;
public partial class TaskUI : Control
{
	// Called when the node enters the scene tree for the first time.
	private Godot.Tree TaskTree;
	public override void _Ready()
	{
		TaskTree = GetChild<Godot.Tree>(0);
		TreeItem root = TaskTree.CreateItem();
		root.SetText(0,"Tasks");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}

using Godot;
using System;

public partial class TaskTree : Tree
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Connect("item_selected", new Callable(this, nameof(OnItemSelected)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnItemSelected()
	{
		TaskUI parent = GetParent<TaskUI>();
		if(GetSelected().GetChildCount() == 0) parent.SelectTask(GetSelected().GetIndex());
		else
		{
			DeselectAll();
			parent.DeselectAllTasks();
		}
	}
}

using Godot;
using System;

public partial class Tree : Godot.Tree
{
	[Export] private Node2D world;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var root = CreateItem();
		SetHideRoot(true);
		var beam_tube = CreateItem(root);
		var straight_tube = CreateItem(beam_tube);
		var left_tube = CreateItem(beam_tube);
		var right_tube = CreateItem(beam_tube);
		var injector = CreateItem(root);
		var monitor = CreateItem(root);
		
		beam_tube.SetText(0, "Beamline Tube");
		straight_tube.SetText(0, "Straight");
		left_tube.SetText(0, "Left");
		right_tube.SetText(0, "Right");
		injector.SetText(0, "Particle Injector");
		monitor.SetText(0, "Particle Monitor");

		Connect("item_selected", new Callable(this, nameof(OnItemSelected)));
	}

	private void OnItemSelected()
	{
		TreeItem selected = GetSelected();
		if (selected.GetChildCount() == 0)
		{
			world.Call("switchComponent", selected.GetText(0));
		}
		else DeselectAll();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}

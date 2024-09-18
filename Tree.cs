using Godot;
using System;
using System.Linq;

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
			world.Call("switchComponent", GetPath(selected));
		}
		else DeselectAll();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private string TransformText(string text)
	{
		return text.ToLower().Replace(' ', '_');
	}
	private string GetPath(TreeItem item)
	{
		string result = TransformText(item.GetText(0));
		TreeItem currentItem = item;
		while (currentItem.GetParent() != null)
		{
			currentItem = currentItem.GetParent();
			result = TransformText(currentItem.GetText(0)) + "/"+result;
		}

		return result;
	}
}

using Godot;
using System;

public partial class OptionButton : Godot.OptionButton
{
	[Export] private Node2D world;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	
	{
		Connect("item_selected", new Callable(this, nameof(OnItemSelected)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	private void OnItemSelected(int id)
	{
		world.Call("switchComponent", id);
	}
}

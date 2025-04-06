using Godot;
using System;
using Godot.Collections;
using protonic;
using System.Collections.Generic;

public partial class Map : ColorRect
{
	[Export] private World world;

	[Export] private ColorRect followRect;

	private HashSet<BeamlineTube> _beamlines = new HashSet<BeamlineTube>();
	private HashSet<Wire> _wires = new HashSet<Wire>();

	private BeamlineTube _currentBeamline = null;
	private Wire _currentWire = null;

	public void CreateNewBeamline()
	{
		_currentBeamline = new BeamlineTube();
		_beamlines.Add(_currentBeamline);
		AddChild(_currentBeamline);
	}

	public void CreateNewWire()
	{
		_currentWire = new Wire();
		_wires.Add(_currentWire);
		AddChild(_currentWire);
	}

	public BeamlineTube GetCurrentBeamline()
	{
		return _currentBeamline;
	}

	public Wire GetCurrentWire()
	{
		return _currentWire;
	}
	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		
	}
	
	public Vector2 GetTiledMousePos()
	{
		Vector2 mousePos = GetGlobalMousePosition(); 
		Vector2 tilePos = new Vector2(world.TileSize.X * Mathf.Floor(mousePos.X / world.TileSize.X),
			world.TileSize.Y * Mathf.Floor(mousePos.Y / world.TileSize.Y));
		return tilePos;
	}
	public void AddBeamline(string type)
	{
		if(GetCurrentBeamline() == null) CreateNewBeamline();
		Vector2 pos = GetTiledMousePos() + new Vector2(16.0f, 16.0f);
		GetCurrentBeamline().AddPoint(pos, type);
	}

	public void AddWire()
	{
		if(GetCurrentWire() == null) CreateNewWire();
		Vector2 pos = GetTiledMousePos() + new Vector2(16.0f, 16.0f);
		GetCurrentWire().AddPoint(pos);
	}
	public void ResetDraw()
	{
		_currentBeamline = null;
	}
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (Input.IsActionPressed("place") && !Input.IsActionPressed("draw"))
		{
			if(world.CurrentElementName.Contains("beamline")) AddBeamline(world.CurrentElementName);
			else world.ClickedOnMap();
		}
		else if (Input.IsActionPressed("draw"))
		{
			ResetDraw();
		}
		else if (Input.IsActionPressed("delete"))
		{
			world.DeleteElement();
		}
	}
}

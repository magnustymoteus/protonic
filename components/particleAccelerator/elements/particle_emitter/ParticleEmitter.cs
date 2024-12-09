using Godot;
using System;
using protonic;

public partial class ParticleEmitter : Element
{
	public ParticleEmitter(string path, Vector2I position, Vector2I size, float rotation) : base(path, position, size,
		rotation) { }

	public void Emit()
	{
		Electron electron = new Electron(beginPosition, new Vector2(1,0));
		GetTree().GetRoot().AddChild(electron);
	}
}

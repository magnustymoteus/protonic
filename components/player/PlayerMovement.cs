using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export]
	public int Speed { get; set; } = 400;

	[Export] public AnimationTree AnimationTree;

	private Tween _tween;

	public void GetInput()
	{

		Vector2 inputDirection = Input.GetVector("left", "right", "up", "down");
		bool movementChange = inputDirection * Speed != Velocity;
		Velocity = inputDirection * Speed;

		if (movementChange)
		{
			_tween?.Kill();
			_tween = CreateTween();
			_tween.TweenProperty(AnimationTree, "parameters/BlendSpace1D/blend_position", Velocity != Vector2.Zero ? 1.0f : 0.0f,
				0.1f);
			_tween.Parallel().TweenProperty(AnimationTree, "parameters/TimeScale/scale", 1.5f, 0.7);
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		MoveAndSlide();
	}
}

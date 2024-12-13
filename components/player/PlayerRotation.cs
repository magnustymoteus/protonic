using Godot;
using System;

public partial class PlayerRotation : CharacterBody3D
{
	public const float Speed = 5.0f;
	public const float JumpVelocity = 4.5f;

	public override void _PhysicsProcess(double delta)
	{

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
		float angleRadians = Mathf.Atan2(inputDir.X, inputDir.Y);
		if(inputDir != Vector2.Zero) Rotation = new Vector3(Rotation.X, angleRadians, Rotation.Z);
	}
}

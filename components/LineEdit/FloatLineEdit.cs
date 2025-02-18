using Godot;
using System;

public partial class FloatLineEdit : LineEdit
{
	// Called when the node enters the scene tree for the first time.
	[Signal] public delegate void ValueChangedEventHandler(float value);

	public float Value;
	public override void _Ready()
	{
		TextChanged += OnTextChanged;
		Value = float.Parse(Text);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnTextChanged(string text)
	{
		bool success = float.TryParse(text, out float result);
		if (!success)
		{
			SetText(Value.ToString());
			return;
		}
		Value = result;
		EmitSignal(SignalName.ValueChanged, Value);
	}
}

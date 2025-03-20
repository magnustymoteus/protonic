using Godot;
using System;

public partial class ControlConsoleWindow : Control
{
	[Export] private FloatLineEdit AlphaBox, BetaBox, EmittanceBox;

	[Export] private Control EnvelopeVisualizer, PhaseEllipseVisualizer;

	[Export] private Button ApplyButton, StreamButton, CloseButton;

	[Export] private Label EmitterStatus;
	
	private float _alpha, _beta, _emittance;

	public ControlConsole ControlConsole;
	// Called when the node enters the scene tree for the first time.
	
	public override void _Ready()
	{
		SetAlpha(AlphaBox.Value);
		SetBeta(BetaBox.Value);
		SetEmittance(EmittanceBox.Value);
		AlphaBox.ValueChanged += SetAlpha;
		BetaBox.ValueChanged += SetBeta;
		EmittanceBox.ValueChanged += SetEmittance;

		ApplyButton.Pressed += Apply;
		StreamButton.Pressed += StreamSwitch;
		CloseButton.Pressed += Close;
	}

	public void UpdateVisualization()
	{
		var arr = ControlConsole.ConnectedEmitter.connectedBeamline.BeamlineType;
		arr.Reverse();
		EnvelopeVisualizer.Call("set_tubeArray", arr);
		SetEmitterStatus();
	}

	public void SetEmitterStatus()
	{
		ParticleEmitter emitter = ControlConsole.ConnectedEmitter;
		if (emitter != null)
		{
			EmitterStatus.Text = "Online";
			EmitterStatus.SetModulate(Colors.Green);
			StreamButton.SetDisabled(false);
		}
		else
		{
			EmitterStatus.Text = "Offline";
			EmitterStatus.SetModulate(Colors.Red);
			StreamButton.SetDisabled(true);
		}
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void Close()
	{
		GetParent<Control>().SetVisible(false);
		GetParent<Control>().ReleaseFocus();
	}

	private void Apply()
	{
		EnvelopeVisualizer.Call("set_params", _alpha, _beta, _emittance);
		EnvelopeVisualizer.Call("replot");
		
		PhaseEllipseVisualizer.Call("set_params", _alpha, _beta, _emittance);
		PhaseEllipseVisualizer.Call("replot");
	}
	private void StreamSwitch()
	{
		if (EmitterStatus.Text == "Online")
		{
			EnvelopeVisualizer.Call("switch_emitter");
			EnvelopeVisualizer.Call("replot");
			
			EmitterStatus.Text = "Emitting";
			EmitterStatus.SetModulate(Colors.Yellow);
			
			StreamButton.Text = "Stop";
		}
		else if (EmitterStatus.Text == "Emitting")
		{
			//SetEmitterStatus();
			StreamButton.Text = "Stream";
		}
	}
	
	public void SetAlpha(float alpha)
	{
		_alpha = alpha;
	}

	public void SetBeta(float beta)
	{
		_beta = beta;
	}

	public void SetEmittance(float emittance)
	{
		_emittance = emittance;
	}
	
}

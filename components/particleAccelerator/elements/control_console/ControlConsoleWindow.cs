using Godot;
using System;

public partial class ControlConsoleWindow : Control
{
	[Export] private SpinBox AlphaBox, BetaBox, EmittanceBox;

	[Export] private Control EnvelopeVisualizer, PhaseEllipseVisualizer;

	[Export] private Button ApplyButton, CloseButton;

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

		ApplyButton.Pressed += ApplyChange;
		CloseButton.Pressed += Close;
	}

	public void UpdateVisualization()
	{
		EnvelopeVisualizer.Call("set_tubeArray", ControlConsole.GetFilteredConnectedElements<BeamlineTube>());
		ApplyChange();
		SetEmitterStatus();
	}

	public void SetEmitterStatus()
	{
		ParticleEmitter emitter = ControlConsole.GetEmitter();
		if (emitter != null)
		{
			EmitterStatus.Text = "Online";
			EmitterStatus.SetModulate(Colors.Green);
			emitter.Emit();
		}
		else
		{
			EmitterStatus.Text = "Offline";
			EmitterStatus.SetModulate(Colors.Red);
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
	private void ApplyChange()
	{
		EnvelopeVisualizer.Call("replot", _alpha, _beta, _emittance);
		PhaseEllipseVisualizer.Call("replot", _alpha, _beta, _emittance);
	}
	public void SetAlpha(double alpha)
	{
		_alpha = (float)alpha;
	}

	public void SetBeta(double beta)
	{
		_beta = (float)beta;
	}

	public void SetEmittance(double emittance)
	{
		_emittance = (float)emittance;
	}
	
}

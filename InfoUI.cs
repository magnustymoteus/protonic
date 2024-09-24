using Godot;
using System;
using System.Linq;
using protonic.utils;
using static protonic.utils.FileLoader;

public partial class InfoUI : Control
{
	[Export] private RichTextLabel currentInfoText;
	[Export] private ColorRect infoRect;

	private bool fadeInInfo;

	[Export] private Button closeButton; 
	// Called when the node enters the scene tree for the first time.
	public void PopupInfo(string componentPath)
	{
		SetOpacity(infoRect, 0.0f);
		currentInfoText.SetText(FileLoader.SearchFile(componentPath, "info.txt"));
		infoRect.GetParent<Control>().SetVisible(true);
		fadeInInfo = true;
	}
	public override void _Ready()
	{
		closeButton.Pressed += Close;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (fadeInInfo)
		{
			Color color = infoRect.GetModulate();
			color.A = 0.8f;
			infoRect.SetModulate(infoRect.GetModulate().Lerp(color, 2.0f*(float)delta));
			if (infoRect.GetModulate().A >= 0.8f) fadeInInfo = false;
		}
	}

	public void SetOpacity(ColorRect currentColorRect, float opacity)
	{
		Color color = currentColorRect.GetModulate();
		color.A = opacity;
		currentColorRect.SetModulate(color);
	}

	public void Close()
	{
		SetVisible(false);
	}
}

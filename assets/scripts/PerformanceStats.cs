using Godot;
using System;
using System.Collections.Generic;

public partial class PerformanceStats : Label
{
	private double fps = 0;
	private double timer = 0;
	private int frameCount = 0;

	public override void _Process(double delta)
	{
		frameCount++;
		timer += delta;
		if (timer >= 0.1f)
		{
			fps = frameCount / timer;
			Text = "FPS: " + fps.ToString("0");

			timer = 0;
			frameCount = 0;
		}
	}
}

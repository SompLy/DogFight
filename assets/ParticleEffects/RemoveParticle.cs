using Godot;
using System;

public partial class RemoveParticle : GpuParticles2D
{
	void _OnFinished()
	{
		GetParent().QueueFree();
	}
}

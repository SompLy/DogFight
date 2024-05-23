using Godot;
using System;
using System.Collections.Generic;
using System.Numerics;
using Vector2 = Godot.Vector2;

public partial class MultiTargetDynamicCamera2D : Camera2D
{
	private List<Vector2> _targets	 = new List<Vector2>();
	private float 		  _moveSpeed = 0.5f;
	private float 		  _zoomSpeed = 0.05f;
	private float 		  _minZoom	 = 0.5f;
	private float 		  _maxZoom	 = 5.0f;
	private Vector2 	  _margin	 = new Vector2( 150.0f, 100.0f );

	private Vector2 _screenSize;

	private float _lerpPosAmount = 0.0f;
	private float _lerpZoomAmount = 0.0f;
	public override void _Ready()
	{
		_screenSize = GetViewportRect().Size;
	}
	
	public override void _Process( double delta )
	{
		if ( _targets.Count < 1 )
			return;
		Vector2 targetsCentre = Vector2.Zero;
		foreach ( Vector2 target in _targets )
			targetsCentre += target;
		targetsCentre /= _targets.Count;
		Position = targetsCentre;

		// Find zoom that will contain all targets
		Rect2 rect = new Rect2( Position, Vector2.One );
		foreach ( Vector2 target in _targets )
			rect = rect.Expand( target );
		rect = rect.GrowIndividual( _margin.X, _margin.Y, _margin.X, _margin.Y );
		float z;
		if ( rect.Size.X > rect.Size.Y * _screenSize.Aspect() )
			z = Mathf.Clamp( _screenSize.X / rect.Size.X , _minZoom, _maxZoom );
		else
			z = Mathf.Clamp( _screenSize.Y / rect.Size.Y, _minZoom, _maxZoom );
		Vector2 zoomVec;
		zoomVec.X = Mathf.Lerp( Zoom.X, z, 1 );
		zoomVec.Y = Mathf.Lerp( Zoom.Y, z, 1 );
		Zoom = zoomVec;
	}

	public void AddTarget( Vector2 target )
	{
		if ( !_targets.Contains( target ) )
			_targets.Add( target );
	}
	public void UpdateTarget( Vector2 target, int index )
	{
		_targets[ index ] = target;
		GD.Print( _targets[index] );
	}
	public void RemoveTarget( Vector2 target )
	{
		if ( !_targets.Contains( target ) )
			_targets.Remove( target );
	}
}

using Godot;
using System;
using System.Collections.Generic;
using System.Net.Mime;

public partial class Map : Node2D
{
	private MultiTargetDynamicCamera2D _camera;
	private          Sprite2D    _fg;
	private          Sprite2D    _bg;
	private          List<float> _lineList   = new List<float>();
	private readonly Color       TRANSPARENT = new Color( 0, 0, 0, 0 );
	
	public List<Controller> Players = new List<Controller>();
	
	public override void _Ready()
	{
		_camera = GetParent().GetNode<MultiTargetDynamicCamera2D>( "MultiTargetDynamicCamera2D" );
		_fg = GetNode<Sprite2D>( "FG" );
		_bg = GetNode<Sprite2D>( "BG" );
		
		_GenerateMap();
		GetNode<Collision>( "Collision" ).InitMap( _lineList, _fg.Texture.GetWidth(), _fg.Texture.GetHeight() );
	}

	public Vector2 CollisionNormalPoint( Vector2 pos )
	{
		return GetNode<Collision>( "Collision" ).CollisionNormalPoint( pos );
	}

	private void _GenerateMap()
	{
		FastNoiseLite noise = new FastNoiseLite();
		noise.Seed           = ( int )GD.Randi();
		noise.FractalOctaves = 2;
		noise.Frequency      = 0.002f;
		noise.NoiseType      = FastNoiseLite.NoiseTypeEnum.Simplex;

		Image fgImage = _fg.Texture.GetImage();
		Image bgImage = _bg.Texture.GetImage();

		for ( int x = 0; x < fgImage.GetWidth(); x++ )
		{
			// + 1 to not get negative numbers
			// Multiply to move the lowest point possible
			// Offset to get more space at the top
			float high = ( noise.GetNoise1D( x ) + 1 ) * fgImage.GetHeight() * 0.1f + fgImage.GetHeight() * 0.3f;

			_lineList.Add( high );

			for ( int y = 0; y < high; y++ )
			{
				fgImage.SetPixel( x, y, TRANSPARENT );
				bgImage.SetPixel( x, y, TRANSPARENT );
			}
		}

		_fg.Texture = ImageTexture.CreateFromImage( fgImage );
		_bg.Texture = ImageTexture.CreateFromImage( bgImage );
	}

	public void Explosion( Vector2 pos, int radius, int ownerIndex )
	{
		_camera.ApplyScreenShake( radius * 0.5f );
		if ( Players.Count < 1 )
			Players.AddRange( new []{ GetNode<Controller>( "../Player1" ),
				GetNode<Controller>( "../Player2" ) } );
		
		Image fgImage = _fg.Texture.GetImage();
		Image bgImage = _bg.Texture.GetImage();
		
		GetNode<Collision>( "Collision" ).Explosion( pos, radius );
		
		for ( int x = -radius; x <= radius; x++ )
		{
			for ( int y = -radius; y <= radius; y++ )
			{
				if ( new Vector2( x, y ).Length() > radius )
					continue;

				Vector2 pixel = pos + new Vector2( x, y );
				if ( pixel.X < 0 || pixel.X >= fgImage.GetWidth() )
					continue;
				if ( pixel.Y < 0 || pixel.Y >= fgImage.GetHeight() )
					continue;

				// Sample from toher image and make abit darker so it gives a cool effect
				Color colot = bgImage.GetPixel( (int)pixel.X, (int)pixel.Y );
				colot.R *= 0.3f;
				colot.G *= 0.3f;
				colot.B *= 0.3f;
				fgImage.SetPixel( ( int )pixel.X, ( int )pixel.Y, colot );

				if ( IsInstanceValid( Players[ 0 ] ) &&  
				     Mathf.Round( Players[ 0 ].Position.X ) == Mathf.Round( pixel.X ) &&
				     Mathf.Round( Players[ 0 ].Position.Y ) == Mathf.Round( pixel.Y ) )
				{
					Players[ 0 ].InternalVelocity += new Vector2( 0.0f, -7.0f );
					Players[ 0 ].Health			 -= 1;
				}          
				if ( IsInstanceValid( Players[ 1 ] ) && 
					Mathf.Round( Players[ 1 ].Position.X ) == Mathf.Round( pixel.X ) &&
					Mathf.Round( Players[ 1 ].Position.Y ) == Mathf.Round( pixel.Y ) &&
					ownerIndex == 0 )
				{                 
					Players[ 1 ].InternalVelocity += new Vector2( 0.0f, -7.0f );
					Players[ 1 ].Health           -= 1;
				}
			}
		}
		
		_fg.Texture = ImageTexture.CreateFromImage( fgImage );
	}
}

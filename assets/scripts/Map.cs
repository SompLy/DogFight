using Godot;
using System;
using System.Collections.Generic;
using System.Net.Mime;

public partial class Map : Node2D
{
	private          Sprite2D    _fg;
	private          Sprite2D    _bg;
	private          List<float> _lineList   = new List<float>();
	private readonly Color       TRANSPARENT = new Color( 0, 0, 0, 0 );

	private List<Player> _players = new List<Player>();
	
	public override void _Ready()
	{
		_fg = GetNode<Sprite2D>( "FG" );
		_bg = GetNode<Sprite2D>( "BG" );
		_players.AddRange( new []{ GetNode<Player>( "../Player1" ), GetNode<Player>( "../Player2" ) } );
		
		_GenerateMap();
		GetNode<Collision>( "Collision" ).InitMap( _lineList, _fg.Texture.GetWidth(), _fg.Texture.GetHeight() );
	}

	public Vector2 CollisionNormalPoint( Vector2 pos )
	{
		return GetNode<Collision>( "Collision" ).CollisionNormalPoint( pos );
	}
	public Vector2 CollisionNormalBox( CollisionShape2D box, Vector2 dir)
	{
		return GetNode<Collision>( "Collision" ).CollisionNormalBox( box, dir );
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
			float high = ( ( float )( noise.GetNoise1D( x ) + 1 ) * fgImage.GetHeight() * 0.4f ) +
			             fgImage.GetHeight() * 0.08f;

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

	public void Explosion( Vector2 pos, int radius )
	{
		Image fgImage = _fg.Texture.GetImage();
		
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
				fgImage.SetPixel( ( int )pixel.X, ( int )pixel.Y, TRANSPARENT );

				if ( IsInstanceValid( _players[ 0 ] ) &&  
				     Mathf.Round( _players[ 0 ].Position.X ) == Mathf.Round( pixel.X ) &&
				     Mathf.Round( _players[ 0 ].Position.Y ) == Mathf.Round( pixel.Y ) )
				{
					_players[ 0 ].InternalVelocity += new Vector2( 0.0f, -7.0f );
					_players[ 0 ].Health			 -= 1;
				}          
				if ( IsInstanceValid( _players[ 1 ] ) && 
					Mathf.Round( _players[ 1 ].Position.X ) == Mathf.Round( pixel.X ) &&
					Mathf.Round( _players[ 1 ].Position.Y ) == Mathf.Round( pixel.Y ) )
				{                 
					_players[ 1 ].InternalVelocity += new Vector2( 0.0f, -7.0f );
					_players[ 1 ].Health           -= 1;
				}
			}
		}
		
		_fg.Texture = ImageTexture.CreateFromImage( fgImage );
	}
}

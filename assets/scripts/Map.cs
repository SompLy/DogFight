using Godot;
using System;
using System.Collections.Generic;
using System.Net.Mime;

public partial class Map : Node2D
{
	private          Sprite2D    fg;
	private          Sprite2D    bg;
	private          List<float> line        = new List<float>();
	private readonly Color       TRANSPARENT = new Color( 0, 0, 0, 0 );

	public override void _Ready()
	{
		fg = GetNode<Sprite2D>( "FG" );
		bg = GetNode<Sprite2D>( "BG" );
		_GenerateMap();
		GetNode<Collision>( "Collision" ).InitMap( line, fg.Texture.GetWidth(), fg.Texture.GetHeight() );
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

		Image fgImage = fg.Texture.GetImage();
		Image bgImage = bg.Texture.GetImage();

		for ( int x = 0; x < fgImage.GetWidth(); x++ )
		{
			float high = ( ( float )( noise.GetNoise1D( x ) + 1 ) * fgImage.GetHeight() * 0.4f ) +
			             fgImage.GetHeight() * 0.08f;

			line.Add( high );

			for ( int y = 0; y < high; y++ )
			{
				fgImage.SetPixel( x, y, TRANSPARENT );
				bgImage.SetPixel( x, y, TRANSPARENT );
			}
		}

		fg.Texture = ImageTexture.CreateFromImage( fgImage );
		bg.Texture = ImageTexture.CreateFromImage( bgImage );
	}

	public void Explosion( Vector2 pos, int radius )
	{
		GetNode<Collision>( "Collision" ).Explosion( pos, radius );

		Image fgImage = fg.Texture.GetImage();

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
			}
		}

		fg.Texture = ImageTexture.CreateFromImage( fgImage );
	}
}

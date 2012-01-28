using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GameTools.Noise2D
{
    class PerlinNoiseSettings2D
    {
        Random rng;

        public Point size;
        public Point startingPoint;
        public double frequencyMulti;
        public double persistence;
        public float zoom;        
        public int octaves;      
        public int seed;

        public PerlinNoiseSettings2D( )
        {
            rng = new Random( );

            size = new Point( 100, 100 );
            startingPoint = Point.Zero;  
          
            frequencyMulti = 2;
            persistence = 0.5;
            zoom = 40;
            octaves = 6;            
            seed = 0;   

            GenerateNewSeed( );
        }

        public PerlinNoiseSettings2D(PerlinNoiseSettings2D settings)
        {
            size = settings.size;
            startingPoint = settings.startingPoint;
          
            frequencyMulti = settings.frequencyMulti;
            persistence = settings.persistence;
            zoom = settings.zoom;
            octaves = settings.octaves;            
            seed = settings.seed;   
        }

        public void GenerateNewSeed( )
        {
            seed = rng.Next( );
        }
        
    }
}

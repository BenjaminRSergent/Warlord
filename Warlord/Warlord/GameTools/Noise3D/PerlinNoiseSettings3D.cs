using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using GameTools.Graph;

namespace GameTools.Noise3D
{
    class PerlinNoiseSettings3D
    {
        Random rng;

        public Vector3i size;
        public Vector3i startingPoint;
        public float frequencyMulti;
        public float persistence;
        public float zoom;        
        public int octaves;      
        public int seed;

        public PerlinNoiseSettings3D( )
        {
            rng = new Random( );

            size = new Vector3i( 100, 100, 100 );
            startingPoint = Vector3i.Zero;  
          
            frequencyMulti = 2;
            persistence = 0.5f;
            zoom = 40;
            octaves = 6;            
            seed = 0;   

            GenerateNewSeed( );
        }

        public PerlinNoiseSettings3D(PerlinNoiseSettings3D settings)
        {
            size = new Vector3i(settings.size);
            startingPoint = new Vector3i(settings.startingPoint);
          
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

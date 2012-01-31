﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Microsoft.Xna.Framework;
using GameTools.Graph;

namespace GameTools.Noise3D
{
    static class PerlinNoise3D
    {
        static public double[,,] GenPerlinNoise3D( PerlinNoiseSettings3D settings)
        {            
            double[,,] noise = new double[settings.size.X, settings.size.Y, settings.size.Z];

            if( settings.numThreads < 1 )
                settings.numThreads = 1;

            if(settings.numThreads == 1)
            {
                MakePerlinNoise3D( noise, Vector3i.Zero, settings );

                return noise;
            }

            int rowsPerThread = settings.size.X / settings.numThreads;
            int extraRows = settings.size.X % settings.numThreads;            

            PerlinNoiseSettings3D threadSettings = new PerlinNoiseSettings3D( settings );

            threadSettings.size.X = rowsPerThread;
            threadSettings.zoom = settings.zoom/4;

            Thread[] noiseGenThreads = new Thread[settings.numThreads];            

            for( int thread = 0; thread < settings.numThreads; thread++ )
            {
                int arrayStartX = rowsPerThread*thread;

                if( thread == settings.numThreads-1 )
                {
                    threadSettings.size.X += extraRows;
                }
                
                noiseGenThreads[thread] = new Thread( () => MakePerlinNoise3D( noise, new Vector3i(arrayStartX,0,0), threadSettings ) );
                noiseGenThreads[thread].Name = "PerlinNoise3D Thread " + thread;
                noiseGenThreads[thread].Start( );
            }             
          
            for( int thread = 0; thread < settings.numThreads; thread++ )
            {
                noiseGenThreads[thread].Join( );
            }

            return noise;
        }

        static void MakePerlinNoise3D( double[,,] toFill, Vector3i fillStart, PerlinNoiseSettings3D settings )
        {
            int width;
            int height;
            int length;

            int effectiveX;
            int effectiveY;
            int effectiveZ;

            Vector3f regionSize = new Vector3f();

            double frequency;
            double amplitude; 

            width  = settings.size.X;  
            height = settings.size.Y;
            length = settings.size.Z;

            regionSize.X = width / settings.zoom;
            regionSize.Y = height / settings.zoom;
            regionSize.Z = length / settings.zoom;
        
            for( int x = fillStart.X; x < fillStart.X+width; x++ )
            {
                for( int y = fillStart.Y; y < fillStart.Y+height; y++ )
                {
                    for( int z = fillStart.Z; z < fillStart.Z+length; z++ )
                    {
                        effectiveX = x + settings.startingPoint.X;
                        effectiveY = y + settings.startingPoint.Y;
                        effectiveZ = z + settings.startingPoint.Z;

                        frequency = 1;
                        amplitude = 1;

                        for( int oct = 0; oct < settings.octaves; oct++ )
                        {                    
                            double noise = SimpleNoise3D.GenInterpolatedNoise(effectiveX * frequency,
                                                                              effectiveY * frequency,
                                                                              effectiveZ * frequency,
                                                                              settings.seed);
                            toFill[x,y,z] += amplitude * noise;

                            frequency *= settings.frequencyMulti;
                            amplitude *= settings.persistence   ;
                        }       
                    }
                }
            }
        }
    }
}

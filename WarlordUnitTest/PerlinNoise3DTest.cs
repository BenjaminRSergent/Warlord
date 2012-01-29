using GameTools.Noise3D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using GameTools.Graph;
using Warlord.GameTools.Statistics;
using System.Collections.Generic;

namespace WarlordUnitTest
{
    
    
    /// <summary>
    ///This is a test class for PerlinNoise3DTest and is intended
    ///to contain all PerlinNoise3DTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PerlinNoise3DTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for MakePerlinNoise3D
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Warlord.exe")]
        public void MakePerlinNoise3DTest()
        {
            //Vector3i_Accessor size = new Vector3i_Accessor(100,100,1);
            //double[,,] toFill = new double[size.X,size.Y,size.Z];

            //Vector3i_Accessor fillStart = new Vector3i_Accessor(0,0,0);

            //PerlinNoiseSettings3D_Accessor settings = new PerlinNoiseSettings3D_Accessor( );

            //settings.size = size;
            //settings.seed = 1;
            //settings.zoom = 51;

            //PerlinNoise3D_Accessor.MakePerlinNoise3D(toFill, fillStart, settings);

            //double previousNoise = 0;

            //double[] differenceArray = new double[size.X*size.Y*size.Z];
            //List<double> betweenGapList = new List<double>( );
            //double[] linearFillArray = new double[size.X*size.Y*size.Z];

            //for( int x = 0; x < size.X; x++ )
            //{
            //    for( int y = 0; y < size.Y; y++ )
            //    {
            //        for( int z = 0; z < size.Z; z++ )
            //        {
            //            linearFillArray[x*(size.Y*size.Z) + y*size.Z + z] = toFill[x,y,z];
            //            differenceArray[x*(size.Y*size.Z) + y*size.Z + z] = toFill[x,y,z] - previousNoise;                       

            //            previousNoise = toFill[x,y,z];
            //        }
            //    }
            //}

            //int sinceJump = 1;
            //foreach( double difference in differenceArray )
            //{
            //    if( difference > 0.5 )
            //    {                    
            //        betweenGapList.Add( sinceJump );
            //        sinceJump = 1;
            //    }
            //    else
            //        sinceJump++;
            //}

            //Assert.Inconclusive("The adverage number was: " + Statistics.Adverage(linearFillArray) + "The std deviation was: " + Statistics.StdDeviation( linearFillArray));
            ////Assert.Inconclusive( "There were " + betweenGapList.Count + " jumps. The adverage between jumps was " + Statistics.Adverage(betweenGapList.ToArray( )) +
            //  //                   "The Std deviation was " + Statistics.StdDeviation(betweenGapList.ToArray( )) );
        }
    }
}

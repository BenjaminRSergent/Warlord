using GameTools.Noise2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Microsoft.Xna.Framework;
using Warlord.GameTools.Statistics;
using GameTools.Graph;
using System.Collections.Generic;

namespace WarlordUnitTest
{
    
    
    /// <summary>
    ///This is a test class for PerlinNoise2DTest and is intended
    ///to contain all PerlinNoise2DTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PerlinNoise2DTest
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
        ///A test for MakePerlinNoise2D
        ///</summary>
        [TestMethod()]
        [DeploymentItem("Warlord.exe")]
        public void MakePerlinNoise2DTest()
        {
            Vector2i_Accessor size = new Vector2i_Accessor(100,100);
            double[,] toFill = new double[size.X,size.Y];

            Point fillStart = new Point(0,0);

            PerlinNoiseSettings2D_Accessor settings = new PerlinNoiseSettings2D_Accessor( );

            settings.size = new Point(size.X,size.Y);
            settings.seed = 1;
            settings.zoom = 51;

            PerlinNoise2D_Accessor.MakePerlinNoise2D(toFill, fillStart, settings);

            double previousNoise = 0;

            double[] differenceArray = new double[size.X*size.Y];
            List<double> betweenGapList = new List<double>( );
            double[] linearFillArray = new double[size.X*size.Y];

            for( int x = 0; x < size.X; x++ )
            {
                for( int y = 0; y < size.Y; y++ )
                {
                    linearFillArray[x*(size.Y) + y] = toFill[x,y];
                    differenceArray[x*(size.Y) + y] = toFill[x,y] - previousNoise;

                    previousNoise = toFill[x,y];
                }
            }

            int sinceJump = 0;
            foreach( double difference in differenceArray )
            {
                if( difference > 0.1 )
                {                    
                    betweenGapList.Add( sinceJump );
                    sinceJump = 0;
                }
                else
                    sinceJump++;
            }


            Assert.Inconclusive( "The adverage between jumps was " + Statistics.Adverage(betweenGapList.ToArray( )) +
                                 "The Std deviation was " + Statistics.StdDeviation(betweenGapList.ToArray( )) );
        }
    }
}

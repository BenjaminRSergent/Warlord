using GameTools.Noise2D;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WarlordUnitTest
{
    
    
    /// <summary>
    ///This is a test class for SimpleNoise2DTest and is intended
    ///to contain all SimpleNoise2DTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SimpleNoise2DTest
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
        ///A test for GenDoubleNoise
        ///</summary>
        [TestMethod()]
        public void GenDoubleNoiseTest()
        {
            double min = -1;
            double max =  1;

            double actual;
            double sum;

            double largestPositive = Double.MinValue;
            double largestNegative = Double.MaxValue;

            int inputVariation = 1000;
            sum = 0;
            for( int x = 0; x < inputVariation ; x+=7 )
            {
                for( int y = 0; y < inputVariation; y+=7 )
                {
                    for( int seed = 0; seed < inputVariation; seed+=7 )
                    {
                        actual = SimpleNoise2D.GenDoubleNoise(x, y, seed);

                        sum += actual;

                        if( actual > largestPositive )
                            largestPositive = actual;

                        if( actual < largestNegative )
                            largestNegative = actual;

                        if( actual <= min || actual >= max )
                        {
                            Assert.Fail("noise out of range. result: " + actual + " is more than " + min + " and less than " + max );
                        }
                    }
                }
            }

            Assert.IsTrue( sum/(Math.Pow(inputVariation,3)) < 0.001 );
            Assert.IsTrue( largestNegative < -0.4 );
            Assert.IsTrue( largestPositive >  0.4 ); 
        }
    }
}

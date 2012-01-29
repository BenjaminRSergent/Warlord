using Warlord.GameTools;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Warlord.Event;
using GameTools;

namespace WarlordUnitTest
{
    
    
    /// <summary>
    ///This is a test class for MultipriorityQueueTest and is intended
    ///to contain all MultipriorityQueueTest Unit Tests
    ///</summary>
    [TestClass()]
    public class MultipriorityQueueTest
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
        ///A test for Add
        ///</summary>
        public void AddTestHelper<value, data>()
            where value : IComparable
        {
            MultipriorityQueue<int, GameEvent> target = new MultipriorityQueue<int,GameEvent>( );            
            
            GameEvent[] testEvents = new GameEvent[5];

            for( int k = 0; k < 5; k++ )
            {
                testEvents[k] = new GameEvent( new Optional<object>( ), "dummy_event", null, k );

                target.Add( testEvents[k].Delay, testEvents[k] );
            }           
            
            List<GameEvent> events = target.GetAndRemove( 3 );

            Assert.AreEqual( 4, events.Count );
            
            foreach( GameEvent theEvent in events )
            {
                Assert.IsTrue( theEvent.Delay <= 3 );
            }
        }

        [TestMethod()]
        public void AddTest()
        {
            AddTestHelper<GenericParameterHelper, GenericParameterHelper>();
        }

        /// <summary>
        ///A test for GetAndRemove
        ///</summary>
        public void GetAndRemoveTestHelper<value, data>()
            where value : IComparable
        {
            MultipriorityQueue<int, GameEvent> target = new MultipriorityQueue<int,GameEvent>( );            
            
            GameEvent[] testEvents = new GameEvent[5];

            for( int k = 0; k < 5; k++ )
            {
                testEvents[k] = new GameEvent( new Optional<object>( ), "dummy_event", null, k );

                target.Add( testEvents[k].Delay, testEvents[k] );
            }           
            
            List<GameEvent> events = target.GetAndRemove( 2 );

            Assert.AreEqual( 2, target.Count );
        }

        [TestMethod()]
        public void GetAndRemoveTest()
        {
            GetAndRemoveTestHelper<GenericParameterHelper, GenericParameterHelper>();
        }
    }
}

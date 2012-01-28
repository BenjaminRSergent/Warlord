using Warlord.Event;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace WarlordUnitTest
{
    
    
    /// <summary>
    ///This is a test class for WarlordEventManagerTest and is intended
    ///to contain all WarlordEventManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class WarlordEventManagerTest
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
        ///A test for SendDelayedEvents
        ///</summary>
        [TestMethod()]
        public void SendDelayedEventsTest()
        {
            WarlordEventManager target = new WarlordEventManager(); 
            GameEvent theEvent = new GameEvent(new Warlord.GameTools.Optional<object>( ), "actor_moved", 10 ); 

            DummySubscriber dummy = new DummySubscriber( target );           
            Assert.IsFalse( dummy.RecievedEvent );

            target.SendEvent( theEvent );
            Assert.IsFalse( dummy.RecievedEvent );

            target.SendDelayedEvents( 5 );
            Assert.IsFalse( dummy.RecievedEvent );

            target.SendDelayedEvents( 9 );
            Assert.IsFalse( dummy.RecievedEvent );

            target.SendDelayedEvents( 10 );
            Assert.IsTrue( dummy.RecievedEvent );

            Assert.AreEqual( dummy.TheEvent, theEvent );
        }

        /// <summary>
        ///A test for SendEvent
        ///</summary>
        [TestMethod()]
        public void SendEventTest()
        {
            WarlordEventManager target = new WarlordEventManager(); 
            GameEvent theEvent = new GameEvent(new Warlord.GameTools.Optional<object>( ), "actor_moved", 0 ); 

            DummySubscriber dummy = new DummySubscriber( target );
            
            Assert.IsFalse( dummy.RecievedEvent );
            target.SendEvent( theEvent );

            Assert.IsTrue( dummy.RecievedEvent );

            Assert.AreEqual( dummy.TheEvent, theEvent );
        }
    }
}

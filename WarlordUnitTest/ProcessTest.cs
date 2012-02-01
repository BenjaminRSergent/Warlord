using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;

namespace WarlordUnitTest
{
    [TestClass]
    public class ProcessTest
    {
        [TestMethod]
        public void TestProcessUpdater()
        {
            DummyProcess dummy = new DummyProcess( 10 );

            dummy.Update( new GameTime( ) );

            Assert.Inconclusive( "" + dummy.x );
        }
    }
}

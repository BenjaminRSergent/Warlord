using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Statistics;

namespace WarlordTests
{
    using NUnit.Framework;
    using Microsoft.Xna.Framework.Graphics;

    [TestFixture]
    public class ExampleTest
    {
        GraphicsDevice dummyGraphics;
        [SetUp]
        public void Init()
        {
            dummyGraphics = new GraphicsDevice(GraphicsAdapter.DefaultAdapter, GraphicsProfile.HiDef, new PresentationParameters());
        }

        [Test]
        public void Test( )
        {
            
        }
    }
}

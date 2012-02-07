using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.State;
using Warlord.Logic.Data.World;
using GameTools.Graph;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Threading;

namespace Warlord.Logic.States
{
    class DebugPlayingState : State<WarlordLogic>
    {
        RegionUpdater regionUpdater;
        Stopwatch stopWatch;

        KeyboardState prevkeyState;
        public DebugPlayingState(WarlordLogic owner, int drawDistance, Vector3i regionSize)
            : base(owner)
        {
            regionUpdater = new RegionUpdater(drawDistance, 27, regionSize);            

            stopWatch = new Stopwatch( );
            stopWatch.Start( );
        }
        public override void EnterState()
        {
            owner.EntityManager.AddPlayer(new Vector3(0, 80, 0));
            owner.ProcessManager.AttachProcess( regionUpdater );
        }
        public override void Update()
        {
            stopWatch.Restart( );
            owner.ProcessManager.Update( owner.MostRecentGameTime );

            KeyboardState keyState = Keyboard.GetState( );

            if( !prevkeyState.IsKeyDown( Keys.P ) && keyState.IsKeyDown( Keys.P ) )
            {                
                regionUpdater.Pause( );

                while( !regionUpdater.Waiting )
                    Thread.Sleep(1);

                using( BinaryWriter writer = new BinaryWriter(File.Open("testSave", FileMode.Create)))
                regionUpdater.Save( writer );          

                regionUpdater.Unpause( );
            }
            if( !prevkeyState.IsKeyDown( Keys.L ) && keyState.IsKeyDown( Keys.L ) )
            {   
                regionUpdater.Pause( );
                
                while( !regionUpdater.Waiting )
                    Thread.Sleep(1);

                using( BinaryReader reader = new BinaryReader(File.Open("testSave", FileMode.Open)))
                regionUpdater.Load( reader ); 

                regionUpdater.Unpause( );
            }
            prevkeyState = keyState;
        }
        public override void ExitState()
        {
            owner.ProcessManager.ShutDown( );
            owner.EntityManager.ShutDown( );
        }
    }
}

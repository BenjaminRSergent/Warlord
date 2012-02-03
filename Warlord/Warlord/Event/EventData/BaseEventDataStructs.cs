//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using GameTools.Graph;
//using Warlord.Logic.Data;
//using Microsoft.Xna.Framework;

//namespace Warlord.Event.EventData
//{
//    class ActorEventData : EventArgs
//    {
//        private uint actorId;        

//        public ActorEventData( uint actorId )
//        {
//            this.actorId = actorId;
//        }

//        public uint ActorId
//        {
//            get { return actorId; }
//        }
//    }
//    class GameTimeEventData : EventArgs
//    {
//        private GameTime gameTime;

//        public GameTimeEventData( GameTime gameTime )
//        {
//            this.gameTime = gameTime;
//        }

//        public GameTime GameTime
//        {
//            get { return gameTime; }
//        }
//    }
//    class RegionEventData : EventArgs
//    {
//        private Region region;

//        RegionEventData( Region region )
//        {
//            this.region = region;
//        }

//        public Region Region
//        {
//            get { return region; }            
//        }
//    }
//    class CameraMoveEventData : EventArgs
//    {
//        private Vector3f translationXYZ;

//        CameraMoveEventData( Vector2f translationXYZ )
//        {

//        }

//        public CameraMoveEventData( Vector3f translationXYZ )
//        {
//            this.translationXYZ = translationXYZ;
//        }

//        public Vector3f TranslationXYZ
//        {
//            get { return translationXYZ; }            
//        }
//    }
//    class CameraRotateEventData : EventArgs
//    {
//        private Vector2f rotationXY;

//        public Vector2f RotationXY
//        {
//            get { return rotationXY; }            
//        }
//    }
//}

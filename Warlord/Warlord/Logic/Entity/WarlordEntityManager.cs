using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using Warlord.GameTools;
using Microsoft.Xna.Framework;

namespace Warlord.Logic.Data.Entity
{
    class WarlordEntityManager : EntityManager
    {
        Player player;
        Dictionary<uint, Entity> entityMap;
        uint nextactorID;

        public WarlordEntityManager( )
        {
            entityMap = new Dictionary<uint,Entity>( );
            nextactorID = 0;
        }
        public Optional<Entity> GetEntity( uint id )
        {
            if( entityMap.ContainsKey(id))
                return new Optional<Entity>(entityMap[id]);
            else
                return new Optional<Entity>();
        }
        public void AddPlayer( Vector3 position)
        {
            player = new Player( GetNextactorID(), position );
            entityMap.Add( player.actorID, player );
        }
        public Entity Player
        {
            get
            {
                return player;
            }
        }
        private uint GetNextactorID()
        {
            uint id = nextactorID;
            nextactorID++;
            return id;            
        }
        public void ShutDown()
        {
            entityMap.Clear( );
        }
    }
}

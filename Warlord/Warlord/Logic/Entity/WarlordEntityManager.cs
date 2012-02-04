using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GameTools.Graph;
using Warlord.GameTools;

namespace Warlord.Logic.Data.Entity
{
    class WarlordEntityManager : EntityManager
    {
        Player player;
        Dictionary<uint, Entity> entityMap;
        uint nextID;

        public WarlordEntityManager( )
        {
            entityMap = new Dictionary<uint,Entity>( );
            nextID = 0;
        }
        public Optional<Entity> GetEntity( uint id )
        {
            if( entityMap.ContainsKey(id))
                return new Optional<Entity>(entityMap[id]);
            else
                return new Optional<Entity>();
        }
        public void AddPlayer( Vector3f vector3f)
        {
            player = new Player( GetNextID(), vector3f );
            entityMap.Add( player.ID, player );
        }
        public Entity Player
        {
            get
            {
                return player;
            }
        }
        private uint GetNextID()
        {
            uint id = nextID;
            nextID++;
            return id;            
        }
        public void ShutDown()
        {
            entityMap.Clear( );
        }
    }
}

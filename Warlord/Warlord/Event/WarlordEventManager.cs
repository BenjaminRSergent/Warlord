using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameTools;
using Microsoft.Xna.Framework;

namespace Warlord.Event
{
    class WarlordEventManager : EventManager
    {
        private HashSet<String> validEvents;
        private MultipriorityQueue<int, GameEvent> eventQueue;
        private Dictionary<String, List<EventReaction>> subscriberDirectory;

        private int currentTime;

        public WarlordEventManager( )
        {
            validEvents = new HashSet<string>( );
            ValidEventInitializer.SetValidEvents( ref validEvents );

            eventQueue = new MultipriorityQueue<int,GameEvent>( );

            subscriberDirectory = new Dictionary<string,List<EventReaction>>( );

            currentTime = 0;

            foreach( String eventName in validEvents )
            {
                subscriberDirectory.Add( eventName, new List<EventReaction>() );
            }

            subscriberDirectory["update"].Add(Update);
        }

        public void Subscribe(EventReaction eventReaction, String eventType)
        {
            if( !validEvents.Contains(eventType) )
                throw( new ArgumentException("The event: " + eventType + " is not a valid event") );

            subscriberDirectory[eventType].Add(eventReaction);
        }

        public void SendEvent(GameEvent theEvent)
        {
            if( !validEvents.Contains(theEvent.EventType) )
                throw( new ArgumentException("The event: " + theEvent.EventType + " is not a valid event") );

            if( theEvent.Delay == 0 )
                FireEvent( theEvent );
            else
                eventQueue.Add( currentTime+theEvent.Delay, theEvent );
        }
        public void Update( object sender, object currentGameTime )
        {
            GameTime gameTime = currentGameTime as GameTime;
            SendDelayedEvents( gameTime.ElapsedGameTime.Milliseconds );
        }
        public void SendDelayedEvents( int currentTime )
        {
            List<GameEvent> eventsToFire;

            if( eventQueue.Count > 0 )
            {
                eventsToFire = eventQueue.GetAndRemove( currentTime );

                foreach( GameEvent theEvent in eventsToFire )
                {
                    FireEvent( theEvent );
                }
            }                        
        }

        private void FireEvent( GameEvent theEvent )
        {
            foreach( EventReaction eventReaction in subscriberDirectory[theEvent.EventType] )
            {
                eventReaction( theEvent.Sender, theEvent.AdditionalData );
            }
        }
    }
}
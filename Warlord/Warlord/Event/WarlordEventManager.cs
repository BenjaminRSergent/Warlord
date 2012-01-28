using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameTools;
using Microsoft.Xna.Framework;

namespace Warlord.Event
{
    class WarlordEventManager : EventManager, EventSubscriber
    {
        private HashSet<String> validEvents;
        private MultipriorityQueue<int, GameEvent> eventQueue;
        private Dictionary<String, List<EventSubscriber>> subscriberDirectory;

        private int currentTime;

        public WarlordEventManager( )
        {
            validEvents = new HashSet<string>( );
            ValidEventInitializer.SetValidEvents( ref validEvents );

            eventQueue = new MultipriorityQueue<int,GameEvent>( );

            subscriberDirectory = new Dictionary<string,List<EventSubscriber>>( );

            currentTime = 0;

            foreach( String eventName in validEvents )
            {
                subscriberDirectory.Add( eventName, new List<EventSubscriber>() );
            }

            subscriberDirectory["update"].Add(this);
        }

        public void Subscribe(EventSubscriber listener, String eventType)
        {
            subscriberDirectory[eventType].Add(listener);
        }

        public void SendEvent(GameEvent theEvent)
        {
            if( theEvent.Delay == 0 )
                FireEvent( theEvent );
            else
                eventQueue.Add( currentTime+theEvent.Delay, theEvent );
        }

        public void SendDelayedEvents( int currentTime )
        {
            List<GameEvent> eventsToFire;

            this.currentTime = currentTime;

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
            foreach( EventSubscriber subscriber in subscriberDirectory[theEvent.EventType] )
            {
                subscriber.HandleEvent( theEvent );
            }
        }

        public void HandleEvent(GameEvent theEvent)
        {
            GameTime time;
            if( theEvent.EventType == "update" )
            {
                time = theEvent.AdditionalData as GameTime;

                SendDelayedEvents( time.TotalGameTime.Milliseconds );
            }
        }
    }
}
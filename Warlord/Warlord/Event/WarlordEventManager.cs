using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GameTools;

namespace Warlord.Event
{
    class WarlordEventManager : EventManager
    {
        private HashSet<String> validEvents;
        private MultipriorityQueue<int, Event> eventQueue;
        private Dictionary<String, List<EventSubscriber>> subscriberDirectory;

        private int currentTime;

        public WarlordEventManager( )
        {
            validEvents = new HashSet<string>( );
            ValidEventInitializer.SetValidEvents( validEvents );

            eventQueue = new MultipriorityQueue<int,Event>( );

            subscriberDirectory = new Dictionary<string,List<EventSubscriber>>( );

            currentTime = 0;

            foreach( String eventName in validEvents )
            {
                subscriberDirectory.Add( eventName, new List<EventSubscriber>() );
            }
        }

        public void Subscribe(EventSubscriber listener, String eventType)
        {
            subscriberDirectory[eventType].Add(listener);
        }

        public void SendEvent(Event theEvent)
        {
            if( theEvent.Delay == 0 )
                FireEvent( theEvent );
            else
                eventQueue.Add( currentTime+theEvent.Delay, theEvent );
        }

        public void SendDelayedEvents( int currentTime )
        {
            List<Event> eventsToFire;

            this.currentTime = currentTime;

            if( eventQueue.Count > 0 )
            {
                eventsToFire = eventQueue.GetAndRemove( currentTime );

                foreach( Event theEvent in eventsToFire )
                {
                    FireEvent( theEvent );
                }
            }                        
        }

        private void FireEvent( Event theEvent )
        {
            foreach( EventSubscriber subscriber in subscriberDirectory[theEvent.EventType] )
            {
                subscriber.HandleEvent( theEvent );
            }
        }
    }
}
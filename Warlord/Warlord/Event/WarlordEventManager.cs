using System;
using System.Collections.Generic;

using GameTools;
using Microsoft.Xna.Framework;

namespace Warlord.Event
{
    class WarlordEventManager : EventManager
    {
        private MultipriorityQueue<int, BaseGameEvent> eventQueue;
        private Dictionary<String, List<EventReaction>> subscriberDirectory;

        private int currentTime;

        public WarlordEventManager()
        {
            eventQueue = new MultipriorityQueue<int, BaseGameEvent>();

            subscriberDirectory = new Dictionary<string, List<EventReaction>>();

            currentTime = 0;
        }

        public void Subscribe(EventReaction eventReaction, String eventType)
        {
            if(!subscriberDirectory.ContainsKey(eventType))
            {
                subscriberDirectory.Add(eventType, new List<EventReaction>());
            }

            subscriberDirectory[eventType].Add(eventReaction);
        }

        public void SendEvent(BaseGameEvent theEvent)
        {
            if(subscriberDirectory.ContainsKey(theEvent.EventType))
            {
                if(theEvent.Delay == 0)
                    FireEvent(theEvent);
                else
                    eventQueue.Add(currentTime + theEvent.Delay, theEvent);
            }
        }
        public void Update(GameTime gameTime)
        {
            SendDelayedEvents(gameTime.ElapsedGameTime.Milliseconds);
        }
        public void SendDelayedEvents(int currentTime)
        {
            List<BaseGameEvent> eventsToFire;

            if(eventQueue.Count > 0)
            {
                eventsToFire = eventQueue.GetAndRemove(currentTime);

                foreach(BaseGameEvent theEvent in eventsToFire)
                {
                    FireEvent(theEvent);
                }
            }
        }

        private void FireEvent(BaseGameEvent theEvent)
        {
            foreach(EventReaction eventReaction in subscriberDirectory[theEvent.EventType])
            {
                eventReaction(theEvent);
            }
        }
    }
}
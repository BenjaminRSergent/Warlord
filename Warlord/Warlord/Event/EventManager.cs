using System;

namespace Warlord.Event
{
    delegate void EventReaction(BaseGameEvent theEvent);
    interface EventManager
    {
        void Subscribe(EventReaction eventReaction, String eventType);
        void SendEvent(BaseGameEvent theEvent);
        void SendDelayedEvents(int currentTime);
    }
}

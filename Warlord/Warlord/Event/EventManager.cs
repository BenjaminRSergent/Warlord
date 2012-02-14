using System;

namespace Warlord.Event
{
    delegate bool EventReaction(BaseGameEvent theEvent);
    interface EventManager
    {
        void Subscribe(EventReaction eventReaction, string eventType);
        void SendEvent(BaseGameEvent theEvent);
        void SendDelayedEvents(int currentTime);
    }
}

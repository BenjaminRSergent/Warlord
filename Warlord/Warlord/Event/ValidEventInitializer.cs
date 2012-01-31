using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord.Event
{
    static class ValidEventInitializer
    {
        static public void SetValidEvents( ref HashSet<String> validEvents )
        {
            validEvents.Add("update");
            validEvents.Add("draw");

            validEvents.Add("attacking");
            validEvents.Add("block_removed");

            validEvents.Add("building_block");
            validEvents.Add("block_added");

            validEvents.Add("actor_request_add");
            validEvents.Add("actor_added");

            validEvents.Add("actor_request_remove");
            validEvents.Add("actor_remove");

            validEvents.Add("actor_request_move");
            validEvents.Add("actor_moved");           
 
            validEvents.Add("region_added");
            validEvents.Add("region_removed");

            validEvents.Add("camera_move_request");
            validEvents.Add("camera_rotate_request");

            validEvents.Add("dummy_event");
        }
    }
}

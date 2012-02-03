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
            //Actor Events
            validEvents.Add("actor_request_add");            
            validEvents.Add("actor_added");
            validEvents.Add("actor_moved");
            validEvents.Add("actor_request_remove");
            validEvents.Add("actor_remove");


            //Game Time Events
            validEvents.Add("read_input");
            validEvents.Add("update");
            validEvents.Add("draw");
            
            // Block Events
            validEvents.Add("block_added");
            validEvents.Add("block_removed");          
           
            // Region Event s            
            validEvents.Add("region_added");
            validEvents.Add("region_removed");

            // Camera Events
            validEvents.Add("camera_move_request");
            validEvents.Add("camera_rotate_request");            

            validEvents.Add("dummy_event");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Warlord.GameTools
{
    class Optional<T>
    {
        private T data;        
        private bool valid;
        
        public Optional( )
        {
            valid = false;
        }
        public Optional( T data )
        {
            this.data = data;
            valid = true;
        }

        public T Data
        {
            get { return data; }
            set 
            { 
                data = value;
                valid = true;
            }
        }
        public bool Valid
        {
            get { return valid; }
        }

    }
}

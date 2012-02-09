
namespace Warlord.GameTools
{
    struct Optional<T>
    {
        private T data;
        private bool valid;

        public Optional(T data)
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

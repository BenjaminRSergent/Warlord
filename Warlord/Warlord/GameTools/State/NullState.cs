
namespace GameTools.State
{
    class NullState<OwnerType> : State<OwnerType>
    {
        public NullState(OwnerType owner)
            : base(owner)
        {

        }

        public override void EnterState()
        {
            //Do nothing
        }

        public override void Update()
        {
            //Do nothing
        }

        public override void ExitState()
        {
            //Do nothing
        }
    }
}

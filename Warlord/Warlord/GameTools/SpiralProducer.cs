using System.Diagnostics;
using GameTools.Graph;

namespace GameTools
{
    class SpiralProducer
    {
        private Direction currentDirection;
        private Vector3i currentPosition;
      
        private int tillDirectionChange;
        private int currentLevel;        

        public SpiralProducer( Vector3i startingPosition, Direction startingDirection )
        {
            NewSpiral( startingPosition, startingDirection );
        }
        public void NewSpiral( Vector3i startingPosition, Direction startingDirection )
        {
            this.currentPosition = startingPosition;
            this.currentDirection = startingDirection;

            tillDirectionChange = 1;
            currentLevel = 0;
        }

        public Vector3i GetNextPosition( )
        {
            if(tillDirectionChange == 0)
            {
                if( currentDirection == Direction.Right )
                {
                    currentDirection = Direction.Down;
                    tillDirectionChange = currentLevel*2;
                }
                else if( currentDirection == Direction.Down)
                {
                    currentDirection = Direction.Left;
                    tillDirectionChange = currentLevel*2;
                }
                else if( currentDirection == Direction.Left)
                {
                    currentDirection = Direction.Up;
                    tillDirectionChange = currentLevel*2+1;
                }
                else if( currentDirection == Direction.Up)
                {
                    currentLevel++;
                    currentDirection = Direction.Right;
                    tillDirectionChange = currentLevel*2-1;
                }
            }                
                
            currentPosition += GetMoveFromDirection( currentDirection );
            tillDirectionChange--;

            return currentPosition;
        }       
 
        private Vector3i GetMoveFromDirection( Direction direction )
        {
            switch(direction)
            {
                case Direction.Up:
                    return Vector3i.ZIncreasing;
                case Direction.Right:
                    return Vector3i.XIncreasing;
                case Direction.Down:
                    return Vector3i.ZDecreasing;
                case Direction.Left:
                    return Vector3i.XDecreasing;
                default:
                    Debug.Assert(false);
                    return Vector3i.Zero;                    
            }
        }
    }
}

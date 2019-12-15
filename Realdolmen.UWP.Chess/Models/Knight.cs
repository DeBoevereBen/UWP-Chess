using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    [Serializable]
    public class Knight : ChessPiece
    {
        public Knight()
        {
            Name = ChessPieceName.Knight;
        }

        public override MoveResult CanMove(Tile currentTile, Tile targetTile)
        {
            Coordinate currentLocation = currentTile.Location;
            Coordinate newLocation = targetTile.Location;

            if (!(base.CanMove(currentTile, targetTile) == MoveResult.CanMove))
                return MoveResult.CannotMove;

            int horizontalStep = Math.Abs(currentLocation.X - newLocation.X);
            int verticalStep = Math.Abs(currentLocation.Y - newLocation.Y);

            // if moving vertical or horizontal
            if (verticalStep <= 0 || horizontalStep <= 0)
                return MoveResult.CannotMove;

            // if step to large
            if (!(verticalStep <= 2) || !(horizontalStep <= 2))
                return MoveResult.CannotMove;

            // if not L shape
            if (verticalStep == 1 && horizontalStep != 2 || verticalStep == 2 && horizontalStep != 1)
                return MoveResult.CannotMove;

            return MoveResult.CanMove;
        }
    }
}

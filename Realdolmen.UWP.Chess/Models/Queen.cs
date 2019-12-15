using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    [Serializable]
    public class Queen : ChessPiece
    {
        public Queen()
        {
            Name = ChessPieceName.Queen;
        }

        public override MoveResult CanMove(Tile currentTile, Tile targetTile)
        {
            Coordinate currentLocation = currentTile.Location;
            Coordinate newLocation = targetTile.Location;

            if (!(base.CanMove(currentTile, targetTile) == MoveResult.CanMove))
                return MoveResult.CannotMove;

            int horizontalStep = Math.Abs(currentLocation.X - newLocation.X);
            int verticalStep = Math.Abs(currentLocation.Y - newLocation.Y);

            if (verticalStep != 0 && horizontalStep != 0 && verticalStep != horizontalStep)
                return MoveResult.CannotMove;

            return MoveResult.CheckIfObstructed;
        }
    }
}

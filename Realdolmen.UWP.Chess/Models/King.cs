using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    [Serializable]
    public class King : ChessPiece
    {
        public King()
        {
            Name = ChessPieceName.King;
        }

        public override MoveResult CanMove(Tile currentTile, Tile targetTile)
        {
            Coordinate currentLocation = currentTile.Location;
            Coordinate newLocation = targetTile.Location;

            if (!(base.CanMove(currentTile, targetTile) == MoveResult.CanMove))
                return MoveResult.CannotMove;

            int horizontalStep = Math.Abs(currentLocation.X - newLocation.X);
            int verticalStep = Math.Abs(currentLocation.Y - newLocation.Y);

            var isPossiblyCastling = currentTile.Piece?.Name == ChessPieceName.King &&
                targetTile.Piece?.Name == ChessPieceName.Rook &&
                currentTile.Piece?.Color == targetTile.Piece?.Color &&
                currentTile.Piece.FirstMove && targetTile.Piece.FirstMove;

            if (!isPossiblyCastling && (horizontalStep > 1 || verticalStep > 1))
                return MoveResult.CannotMove;

            return isPossiblyCastling ? MoveResult.CheckIfCanCastle : MoveResult.CanMove;
        }
    }
}

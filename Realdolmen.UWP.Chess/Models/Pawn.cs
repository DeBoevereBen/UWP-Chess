using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    [Serializable]
    public class Pawn : ChessPiece
    {
        public Pawn()
        {
            Name = ChessPieceName.Pawn;
        }

        public override MoveResult CanMove(Tile currentTile, Tile targetTile)
        {
            Coordinate currentLocation = currentTile.Location;
            Coordinate newLocation = targetTile.Location;

            if (!(base.CanMove(currentTile, targetTile) == MoveResult.CanMove))
                return MoveResult.CannotMove;

            // if moving backwards
            if (currentLocation.Y < newLocation.Y && Color.Equals(Color.White))
                return MoveResult.CannotMove;
            else if (newLocation.Y < currentLocation.Y && Color.Equals(Color.Black))
                return MoveResult.CannotMove;


            int allowedVerticalStep = FirstMove ? 2 : 1;
            int horizontalStep = Math.Abs(currentLocation.X - newLocation.X);
            int verticalStep = Math.Abs(currentLocation.Y - newLocation.Y);

            // if moving more places than allowed
            if (verticalStep > allowedVerticalStep || horizontalStep > 1)
                return MoveResult.CannotMove;

            bool willCapture = targetTile.Piece == null ? false : !targetTile.Piece.Color.Equals(Color);

            // if going diagonal when not capturing
            if (horizontalStep > 0 && !willCapture)
                return MoveResult.CannotMove;

            // if capturing and nog going diagonal
            if (willCapture && horizontalStep == 0)
                return MoveResult.CannotMove;

            if (willCapture && (horizontalStep != 1 || verticalStep != 1))
                return MoveResult.CannotMove;

            MoveResult res = FirstMove ? MoveResult.CheckIfObstructed : MoveResult.CanMove;

            if (!FirstMove && (targetTile.Location.Y == 0 || targetTile.Location.Y == (Constants.Dimensions - 1)))
                return MoveResult.Promote;

            return willCapture ? MoveResult.CanMove : res;
        }
    }
}

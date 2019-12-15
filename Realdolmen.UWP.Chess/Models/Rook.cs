﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    [Serializable]
    public class Rook : ChessPiece
    {
        public Rook()
        {
            Name = ChessPieceName.Rook;
        }

        public override MoveResult CanMove(Tile currentTile, Tile targetTile)
        {
            Coordinate currentLocation = currentTile.Location;
            Coordinate newLocation = targetTile.Location;

            if (!(base.CanMove(currentTile, targetTile) == MoveResult.CanMove))
                return MoveResult.CannotMove;

            int horizontalStep = Math.Abs(currentLocation.X - newLocation.X);
            int verticalStep = Math.Abs(currentLocation.Y - newLocation.Y);

            // if moving diagonally
            if (verticalStep > 0 && horizontalStep > 0)
                return MoveResult.CannotMove;

            return verticalStep == 1 || horizontalStep == 1 ? MoveResult.CanMove : MoveResult.CheckIfObstructed;
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Realdolmen.UWP.Chess.Models
{
    public abstract class ChessPiece
    {
        [Key]
        public int PieceId { get; set; }
        public ChessPieceName Name { get; set; }
        public Color Color { get; set; }
        public bool FirstMove { get; set; } = true;
        
        public virtual MoveResult CanMove(Tile currentTile, Tile targetTile)
        {
            Coordinate currentLocation = currentTile.Location;
            Coordinate newLocation = targetTile.Location;
            // if outside the board
            if (newLocation.Y < 0 || newLocation.Y >= Constants.Dimensions || newLocation.X < 0 || newLocation.X >= Constants.Dimensions)
                return MoveResult.CannotMove;

            // if not moving
            if (currentLocation.Equals(newLocation))
                return MoveResult.CannotMove;

            // if tile has piece of same color unless king and rook try castling
            var isPossiblyCastling = currentTile.Piece?.Name == ChessPieceName.King &&
                targetTile.Piece?.Name == ChessPieceName.Rook &&
                currentTile.Piece?.Color == targetTile.Piece?.Color &&
                currentTile.Piece.FirstMove && targetTile.Piece.FirstMove;

            if ((targetTile.Piece != null && Color.Equals(targetTile.Piece.Color)) && !isPossiblyCastling)
                return MoveResult.CannotMove;

            return MoveResult.CanMove;
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}

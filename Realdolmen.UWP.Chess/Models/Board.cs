using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Realdolmen.UWP.Chess.Models
{
    [Serializable]
    public class Board
    {
        [Key]
        public int BoardId { get; set; }
        public Color PlayersTurn { get; set; } = Color.White;
        public virtual ICollection<Tile> Tiles { get; set; } = new List<Tile>();
        public GameOver GameOver { get; set; }

        public Board()
        {
        }

        public void InitTiles()
        {
            var id = 0;
            for (int row = 0; row < Constants.Dimensions; row++)
            {
                for (int col = 0; col < Constants.Dimensions; col++)
                {
                    ChessPiece piece = null;
                    if (row == 1 || row == Constants.Dimensions - 2)
                    {
                        piece = new Pawn
                        {
                            Color = row == 0 || row == 1 ? Color.Black : Color.White
                        };
                    }
                    else if (row == 0 || row == Constants.Dimensions - 1)
                    {
                        var color = row == 0 || row == 1 ? Color.Black : Color.White;
                        switch (col)
                        {
                            case 0:
                            case 7:
                                piece = new Rook
                                {
                                    Color = color
                                };
                                break;
                            case 1:
                            case 6:
                                piece = new Knight
                                {
                                    Color = color
                                };
                                break;
                            case 2:
                            case 5:
                                piece = new Bishop
                                {
                                    Color = color
                                };
                                break;
                            case 4:
                                piece = new King
                                {
                                    Color = color
                                };
                                break;
                            case 3:
                                piece = new Queen
                                {
                                    Color = color
                                };
                                break;
                            default:
                                piece = null;
                                break;
                        }
                    }

                    Tiles.Add(new Tile
                    {
                        Location = new Coordinate(col, row),
                        Piece = piece,
                        TileId = id++
                    });
                }
            }
        }

        public Move MovePiece(Coordinate current, Coordinate target, bool fakeMove = false)
        {
            var currentTile = GetTileByLocation(current);
            var targetTile = GetTileByLocation(target);
                
            var isCastling = currentTile.Piece?.Name == ChessPieceName.King &&
                targetTile.Piece?.Name == ChessPieceName.Rook &&
                currentTile.Piece?.Color == targetTile.Piece?.Color;

            currentTile.Piece.FirstMove = false;

            var m = new Move
            {
                CurrentPlayersTurn = PlayersTurn,
                IsCastlingMove = isCastling
            };


            if (isCastling)
            {
                targetTile.Piece.FirstMove = false;

                var castlingLeft = targetTile.Location.X < currentTile.Location.X;
                var kingStepX = castlingLeft ? -2 : 2;
                var rookStepX = castlingLeft ? 3 : -2;

                var newKingTile = GetTileByLocation(new Coordinate(currentTile.Location.X + kingStepX, currentTile.Location.Y));
                var newRookTile = GetTileByLocation(new Coordinate(targetTile.Location.X + rookStepX, targetTile.Location.Y));

                newKingTile.Piece = currentTile.Piece;
                newRookTile.Piece = targetTile.Piece;
                currentTile.Piece = null;
                targetTile.Piece = null;


                m.CurrentTile = newKingTile;
                m.TargetTile = newRookTile;
            }
            else
            {
                if (currentTile.Piece.GetType().Equals(typeof(Pawn)))
                {
                    if (currentTile.Piece.CanMove(currentTile, targetTile).Equals(MoveResult.Promote))
                    {
                        PromotePawn(currentTile);
                    }
                }


                if (!fakeMove)
                {
                    if (targetTile.Piece != null && targetTile.Piece.GetType().Equals(typeof(King)))
                    {
                        GameOver = new GameOver
                        {
                            Finished = true,
                            Winner = currentTile.Piece.Color
                        };
                    }
                }

                targetTile.Piece = currentTile.Piece;
                currentTile.Piece = null;


                m.CurrentTile = currentTile;
                m.TargetTile = targetTile;
            }
            m.NextPlayersTurn = PlayersTurn;

            if (!fakeMove)
            {
                if (!GameOver.Finished)
                {
                    PlayersTurn = PlayersTurn == Color.White ? Color.Black : Color.White;

                        var kingIsCheck =
                        IsTileIsUnderAttack(Tiles.Single(x => x.Piece != null && x.Piece.Name == ChessPieceName.King && x.Piece.Color == PlayersTurn).TileId, PlayersTurn);

                    var nextPlayerCanDoMove = false;

                    foreach (var tileOfNextPlayingColor in Tiles.Where(x => x.Piece != null && x.Piece.Color == PlayersTurn).ToList())
                    {
                        var availableMoves = GetAvailableTileIds(tileOfNextPlayingColor.TileId);
                        nextPlayerCanDoMove = availableMoves.Count > 0;
                        if (nextPlayerCanDoMove)
                        {
                            break;
                        }
                    }

                    if (!nextPlayerCanDoMove)
                    {
                        GameOver = new GameOver
                        {
                            Finished = true,
                            Winner = PlayersTurn == Color.White ? Color.Black : Color.White,
                            GameOverType = kingIsCheck && !nextPlayerCanDoMove ? GameOverType.Checkmate : GameOverType.Stalemate
                        };
                    }

                }
            }

            return m;
        }

        public bool CanMove(Tile current, Tile target)
        {
            var canMove = CanMove(current.Location, target.Location);

            var isPossiblyCastling = current.Piece?.Name == ChessPieceName.King &&
                target.Piece?.Name == ChessPieceName.Rook &&
                current.Piece?.Color == target.Piece?.Color &&
                current.Piece.FirstMove && target.Piece.FirstMove;

            var ids = GetAvailableTileIds(current.TileId, !isPossiblyCastling);

            return canMove && ids.Contains(target.TileId);
        }

        private bool CanMove(Coordinate current, Coordinate target, bool checkOtherPlayer = false)
        {
            Tile currentTile = GetTileByLocation(current);
            Tile targetTile = GetTileByLocation(target);


            if (currentTile == null || targetTile == null)
            {
                return false;
            }
            if (currentTile.Piece == null || (PlayersTurn != currentTile.Piece.Color && !checkOtherPlayer))
            {
                return false;
            }
            MoveResult result = currentTile.Piece.CanMove(currentTile, targetTile);

            switch (result)
            {
                case MoveResult.CanMove:
                    return true;
                case MoveResult.CannotMove:
                    return false;
                case MoveResult.Promote:
                case MoveResult.CheckIfObstructed:
                    return IsNotObstructed(currentTile, targetTile);
                case MoveResult.CheckIfCanCastle:
                    return checkOtherPlayer ? true : CheckIfKingCanCastle(currentTile).Contains(targetTile.TileId); // todo fix this
                default:
                    return false;
            }
        }

        private bool CheckIfKingIsVulnerable(Tile currentTile, Tile targetTile)
        {
            Color currentlyPlaying = PlayersTurn;
            int kingsTileId = Tiles.Where(t => (t.Piece != null) && (t.Piece.Name == ChessPieceName.King) && (t.Piece.Color == currentlyPlaying)).Select(t => t.TileId).FirstOrDefault();

            foreach (Tile t in Tiles.Where(t => t.Piece != null && t.Piece.Color != currentlyPlaying).ToList())
            {
                if (GetAvailableTileIds(t.TileId, false).Exists(x => x == kingsTileId))
                {
                    return true;
                }
            }

            return false;
        }

        public List<int> GetAvailableTileIds(int tileId, bool checkIfKingIsVulnerable = true)
        {
            List<int> availableMoves = new List<int>();

            Tile currentTile = Tiles.Where(t => t.TileId == tileId).Single();

            if (currentTile == null || currentTile.Piece == null)
            {
                return availableMoves;
            }

            foreach (Tile t in Tiles)
            {
                if (CanMove(currentTile.Location, t.Location, checkOtherPlayer: !checkIfKingIsVulnerable))
                {
                    var addToList = true;
                    if (checkIfKingIsVulnerable)
                    {
                        ICollection<Tile> originalTiles = Tiles.Select(tile => JsonConvert.DeserializeObject<Tile>(JsonConvert.SerializeObject(tile, new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        }), new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.Auto
                        })).ToList();

                        MovePiece(currentTile.Location, t.Location, fakeMove: true);

                        var target = Tiles.Single(x => x.TileId == t.TileId);

                        var kingAtRisk = CheckIfKingIsVulnerable(currentTile, target);

                        Tiles = originalTiles;
                        currentTile = Tiles.Where(tt => tt.TileId == tileId).Single();

                        addToList = !kingAtRisk;
                    }
                    if (addToList)
                    {
                        availableMoves.Add(t.TileId);
                    }
                }
            }

            if (checkIfKingIsVulnerable && currentTile.Piece.Name == ChessPieceName.King)
            {
                var rookTileIds = CheckIfKingCanCastle(currentTile);
                availableMoves.AddRange(rookTileIds);
            }

            return availableMoves;
        }

        private List<int> CheckIfKingCanCastle(Tile kingTile)
        {
            List<int> availableRookTiles = new List<int>();
            // if not a king or king has already moved 
            if (kingTile?.Piece == null || kingTile.Piece.Name != ChessPieceName.King || !kingTile.Piece.FirstMove)
            {
                return availableRookTiles;
            }

            // get rooks that have same color and didn't already move
            List<Tile> rookTiles = Tiles.Where(t =>
            t.Piece != null &&
            t.Piece.Name == ChessPieceName.Rook &&
            t.Piece.Color == kingTile.Piece.Color &&
            t.Piece.FirstMove).ToList();

            if (rookTiles.Count == 0)
            {
                return availableRookTiles;
            }

            var tilesToRemove = new List<Tile>();
            foreach (var rookTile in rookTiles)
            {
                var stepX = rookTile.Location.X < kingTile.Location.X ? 1 : -1;
                var x = rookTile.Location.X + stepX;
                var pathClear = true;
                var underAttack = false;

                while (x != kingTile.Location.X && pathClear && !underAttack)
                {
                    var targetTile = GetTileByLocation(new Coordinate(x, rookTile.Location.Y));
                    pathClear = targetTile.IsFree;
                    if (x != 1 && pathClear)
                    {
                        underAttack = IsTileIsUnderAttack(targetTile.TileId, kingTile.Piece.Color);
                    }
                    x += stepX;
                }

                if (!pathClear || underAttack)
                {
                    tilesToRemove.Add(rookTile);
                }
            }

            rookTiles = rookTiles.Where(t => !tilesToRemove.Contains(t)).ToList();


            return rookTiles.Select(t => t.TileId).ToList();
        }

        private bool IsTileIsUnderAttack(int tileId, Color currentlyPlaying)
        {
            var tileToCheck = Tiles.Single(t => t.TileId == tileId);

            foreach (Tile tile in Tiles.Where(t => t.Piece != null && t.Piece.Color != currentlyPlaying).ToList())
            {
                if (GetAvailableTileIds(tile.TileId, false).Exists(x => x == tileId))
                {
                    return true;
                }
            }

            return false;
        }

        private void PromotePawn(Tile currentTile)
        {
            var piece = currentTile.Piece;
            if (piece.GetType().Equals(typeof(Pawn)))
            {
                var queen = new Queen
                {
                    Color = piece.Color,
                    Name = ChessPieceName.Queen
                };
                currentTile.Piece = queen;
            }
        }

        public void RemovePiece(Coordinate pos)
        {
            GetTileByLocation(pos).Piece = null;
        }

        private bool IsNotObstructed(Tile currentTile, Tile targetTile)
        {
            Coordinate begin = currentTile.Location;
            int stepX = begin.X > targetTile.Location.X ? -1 : 1;
            int stepY = begin.Y > targetTile.Location.Y ? -1 : 1;

            var horizontal = begin.Y != targetTile.Location.Y && begin.X == targetTile.Location.X;

            if (horizontal)
            {
                for (int y = begin.Y + stepY; stepY < 0 ? y > targetTile.Location.Y : y < targetTile.Location.Y; y += stepY)
                {
                    if (!GetTileByLocation(new Coordinate(begin.X, y)).IsFree)
                    {
                        return false;
                    }
                }
                return true;
            }

            var vertical = begin.Y == targetTile.Location.Y && begin.X != targetTile.Location.X;
            if (vertical)
            {
                for (int x = begin.X + stepX; stepX < 0 ? x > targetTile.Location.X : x < targetTile.Location.X; x += stepX)
                {

                    if (!GetTileByLocation(new Coordinate(x, begin.Y)).IsFree)
                    {
                        return false;
                    }
                }
                return true;
            }

            var diagonal = currentTile.Location.X != targetTile.Location.X && currentTile.Location.Y != targetTile.Location.Y;

            if (diagonal)
            {
                int x = begin.X + stepX;
                int y = begin.Y + stepY;

                while ((stepX < 0 ? x > targetTile.Location.X : x < targetTile.Location.X) || (stepY < 0 ? y > targetTile.Location.Y : y < targetTile.Location.Y))
                {
                    if (!GetTileByLocation(new Coordinate(x, y)).IsFree)
                    {
                        return false;
                    }
                    x += stepX;
                    y += stepY;
                }
                return true;
            }
            return false;
        }

        public Tile GetTileByLocation(Coordinate c)
        {
            return Tiles.Where(t => t.Location.X == c.X && t.Location.Y == c.Y).Single();
        }
    }

    public struct GameOver
    {
        public Color Winner { get; set; }
        public bool Finished { get; set; }
        public GameOverType GameOverType { get; set; }
    }
}

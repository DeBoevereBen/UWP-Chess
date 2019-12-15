using System;
using System.Collections;
using System.Collections.Generic;

namespace Realdolmen.UWP.Chess.Models
{
    [Serializable]
    public class Coordinate
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Coordinate() { }

        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            Coordinate other = obj as Coordinate;

            if (ReferenceEquals(null, other))
                return false;

            return X == other.X && Y == other.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}";
        }
    }
}
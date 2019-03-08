using System;
using Newtonsoft.Json;

namespace StarterSnake.ApiModel {
    public struct Coord : IEquatable<Coord> {
        [JsonProperty("x")]
        public readonly int X;
        [JsonProperty("y")]
        public readonly int Y;

        public Coord(int x, int y) {
            X = x; Y = y;
        }

        public override string ToString() {
            return "(" + X + ", " + Y + ")";
        }

        public static Coord operator+(Coord a, Coord b) {
            return new Coord(a.X + b.X, a.Y + b.Y);
        }

        public Coord Advanced(Direction d) {
            switch (d) {
                case Direction.North:
                    return new Coord(X, Y - 1);
                case Direction.East:
                    return new Coord(X + 1, Y);
                case Direction.South:
                    return new Coord(X, Y + 1);
                case Direction.West:
                    return new Coord(X - 1, Y);
                default:
                    throw new ArgumentException("Invalid value for Direction");
            }
        }

        #region IEquatable
        public override bool Equals(object other) {
            if (other is Coord p) {
                return Equals(p);
            } else {
                return false;
            }
        }

        public bool Equals(Coord other) {
            return X == other.X && Y == other.Y;
        }

        public static bool operator==(Coord a, Coord b) {
            return a.Equals(b);
        }

        public static bool operator!=(Coord a, Coord b) {
            return !a.Equals(b);
        }

        // Also always implement new hashcode if equals updated
        public override int GetHashCode() {
            return X * 113 + Y;
        }
        #endregion IEquatable
    }
}
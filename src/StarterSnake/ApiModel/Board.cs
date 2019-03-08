using System.Collections.Generic;
using Newtonsoft.Json;

namespace StarterSnake.ApiModel {
    public sealed class Board {
        [JsonProperty("width")]
        public readonly int Width;
        [JsonProperty("height")]
        public readonly int Height;
        [JsonProperty("food")]
        public List<Coord> Food;
        [JsonProperty("snakes")]
        public List<Snake> Snakes;

        public bool OnBoard(Coord c) {
            return OnBoard(c.X, c.Y);
        }

        public bool OnBoard(int x, int y) {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
    }
}
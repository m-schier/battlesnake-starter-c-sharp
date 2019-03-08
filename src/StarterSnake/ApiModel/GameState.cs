using Newtonsoft.Json;

namespace StarterSnake.ApiModel {
    public sealed class GameState {
        [JsonProperty("game")]
        public readonly Game Game;
        [JsonProperty("turn")]
        public readonly int Turn;
        [JsonProperty("board")]
        public readonly Board Board;
        [JsonProperty("you")]
        public readonly Snake You;
    }
}
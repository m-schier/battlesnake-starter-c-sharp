using System;

using StarterSnake.ApiModel;

using Newtonsoft.Json;

namespace StarterSnake.Service {
    /// <summary>
    /// Reponse object for API move requests
    /// </summary>
    internal class ResponseMove {
        [JsonProperty("move")]
        public readonly string Move;

        public ResponseMove(Direction d) {
            Move = FormatDirection(d);
        }

        /// <summary>
        /// Get the string encoded direction as used by the API for the given direction enum
        /// </summary>
        public static string FormatDirection(Direction d) {
            switch (d) {
            case Direction.North:
                return "up";
            case Direction.East:
                return "right";
            case Direction.South:
                return "down";
            case Direction.West:
                return "left";
            default:
                throw new ArgumentException();
            }
        }
    }
}
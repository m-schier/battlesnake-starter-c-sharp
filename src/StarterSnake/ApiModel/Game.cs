using System;

using Newtonsoft.Json;

namespace StarterSnake.ApiModel {

    /// <summary>
    /// The game instance description. This class implements the IEquatable interface
    /// and can therefore be efficiently compared to other Games and hashed if you want to implement
    /// stateful logic.
    /// </summary>
    public sealed class Game : IEquatable<Game> {
        [JsonProperty("id")]
        public readonly string ID;

        #region IEquatable
        public override bool Equals(object other) {
            if (other is Game g) {
                return Equals(g);
            } else {
                return false;
            }
        }

        public bool Equals(Game other) {
            return other != null && ID == other.ID;
        }

        public static bool operator==(Game a, Game b) {
            if (a == null) {
                return b == null;
            } else {
                return a.Equals(b);
            }
        }

        public static bool operator!=(Game a, Game b) {
            return !(a == b);
        }

        // Also always implement new hashcode if equals updated
        public override int GetHashCode() {
            return ID.GetHashCode();
        }
        #endregion IEquatable
    }
}
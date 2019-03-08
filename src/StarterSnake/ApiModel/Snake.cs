using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace StarterSnake.ApiModel {
    /// <summary>
    /// Snake representation. Use the <see cref="Snake.ID"/> member to check for equality with other snakes.
    /// </summary>
    public sealed class Snake {
        [JsonProperty("id")]
        public readonly string ID;
        [JsonProperty("name")]
        public readonly string Name;
        [JsonProperty("health")]
        public readonly int Health;

        /// <summary>
        /// List of all body parts as returned by the API. Note that this list may contain duplicates at the end
        /// if the snake has not fully grown
        /// </summary>
        [JsonProperty("body")]
        public readonly List<Coord> Body;

        /// <summary>
        /// Current real length of this snake in parts. Different to the count of body parts as the engine
        /// may repeat tail parts if the tail has not fully grown.
        /// </summary>
        /// <value></value>
        [JsonIgnore]
        public int EffectiveLength {
            get {
                return Body.Count - GrowthLeft;
            }
        }

        /// <summary>
        /// Current head coordinate
        /// </summary>
        [JsonIgnore]
        public Coord Head {
            get { return Body[0]; }
        }

        /// <summary>
        /// Current tail coordinate
        /// </summary>
        [JsonIgnore]
        public Coord Tail {
            get { return Body[Body.Count - 1]; }
        }

        /// <summary>
        /// Number of turns this snake will grow left
        /// </summary>
        [JsonIgnore]
        public int GrowthLeft {
            get {
                // API stores duplicate body parts at end of snake if growing,
                // thus can determine growth left

                int result = 0;
                var tail = Tail;

                for (int i = Body.Count - 2; i >= 0; --i) {
                    if (Body[i] == tail) ++result;
                    else break;
                }
                
                return result;
            }
        }

        public override string ToString() {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Snake, ID=");
            sb.Append(ID);
            sb.Append(", Health=");
            sb.Append(Health);
            sb.Append(", GrowthLeft=");
            sb.Append(GrowthLeft);
            sb.Append(", Body=[");

            for (int i = 0; i < Body.Count; ++i) {
                if (i != 0) sb.Append(", ");
                sb.Append(Body[i]);
            }

            sb.Append("]>");
            return sb.ToString();
        }
    }
}
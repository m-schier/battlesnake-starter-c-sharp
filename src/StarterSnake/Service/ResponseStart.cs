using System.Drawing;

using Newtonsoft.Json;

namespace StarterSnake.Service {
    /// <summary>
    /// Response object for API start requests
    /// </summary>
    internal class ResponseStart {
        [JsonProperty("color")]
        public readonly string Color;

        [JsonProperty("headtype")]
        public string Head {get; set;} = "pixel";

        [JsonProperty("tailtype")]
        public string Tail {get; set;} = "pixel";

        public ResponseStart(Color color) {
            Color = string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B);
        }
    }
}
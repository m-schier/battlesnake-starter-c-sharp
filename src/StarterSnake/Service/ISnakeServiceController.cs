using System.Drawing;

using StarterSnake.ApiModel;

namespace StarterSnake.Service {
    /// <summary>
    /// Interface that must be implemented by all controllers registered with the HTTP snake service.
    /// A service controller is instantiated per service instance, thus controlling multiple snake instances.
    /// </summary>
    public interface ISnakeServiceController {

        /// <summary>
        /// Called when a new game is started for a snake.
        /// </summary>
        /// <param name="s">Game state of this request</param>
        /// <returns>Color your snake wants to use</returns>
        Color Start(GameState s);

        /// <summary>
        /// Called when a snake should decide how to move. Note that this method may be called on multiple
        /// snakes if your service is processing more than one snake in parallel.
        /// </summary>
        /// <param name="s">Game state of this request</param>
        /// <returns>Next move of your snake</returns>
        Direction Move(GameState s);

        /// <summary>
        /// Called when a snake's game has ended.
        /// </summary>
        /// <param name="s">Game state of this request</param>
        void End(GameState s);
    }
}
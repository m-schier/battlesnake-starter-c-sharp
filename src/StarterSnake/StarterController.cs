using System;
using System.Drawing;

using StarterSnake.ApiModel;

namespace StarterSnake {
    public class StarterController : Service.ISnakeServiceController {

        public static Direction[] DIRECTIONS = new Direction[] {Direction.North, Direction.West, Direction.South, Direction.East};

        private Random random;

        public StarterController() {
            random = new Random();
        }

        public Color Start(GameState state) {
            // Let's go with the classic
            // Replace with a color of your choice
            return Color.CornflowerBlue;
        }

        public Direction Move(GameState state) {
            // Return random direction
            // You might want to improve on this
            return DIRECTIONS[random.Next(DIRECTIONS.Length)];
        }

        public void End(GameState state) {
            // No operation
        }
    }
}
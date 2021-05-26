using UnityEngine;

namespace Helsing.Client
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public static class DirectionExtension
    {
        public static Direction ToDirection(this Vector2 vector2)
        {
            var temp = vector2.normalized;

            if (temp.x > 0 && temp.y == 0) return Direction.Right;
            if (temp.x < 0 && temp.y == 0) return Direction.Left;
            if (temp.x == 0 && temp.y > 0) return Direction.Up;
            if (temp.x == 0 && temp.y < 0) return Direction.Down;
            throw new System.Exception($"Failed to get direction from Vector2 ({vector2.x}, {vector2.y})");
        }
    }
}
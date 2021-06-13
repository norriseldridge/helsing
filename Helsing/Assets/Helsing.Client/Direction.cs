using UnityEngine;

namespace Helsing.Client
{
    public enum Direction
    {
        None,
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
#if UNITY_EDITOR
            if (Application.isPlaying)
                throw new System.Exception($"Failed to get direction from Vector2 ({vector2.x}, {vector2.y})");
            else
                return Direction.None;
#endif
        }
    }
}
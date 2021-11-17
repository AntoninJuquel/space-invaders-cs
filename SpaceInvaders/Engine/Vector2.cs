using System;

namespace SpaceInvaders
{
    class Vector2
    {
        public double x, y;
        public Vector2(double _x = 0, double _y = 0)
        {
            x = _x;
            y = _y;
        }

        public double Norme()
        {
            return Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new Vector2(v1.x + v2.x, v1.y + v2.y);
        public static Vector2 operator -(Vector2 v1, Vector2 v2) => new Vector2(v1.x - v2.x, v1.y - v2.y);
        public static Vector2 operator -(Vector2 v1) => new Vector2(-v1.x, -v1.y);
        public static Vector2 operator *(Vector2 v, double x) => new Vector2(v.x * x, v.y * x);
        public static Vector2 operator *(double x, Vector2 v) => v * x;
        public static Vector2 operator /(Vector2 v, double x) => v * (1 / x);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Up => new Vector2(0, -1);
        public static Vector2 Down => new Vector2(0, 1);
    }
}

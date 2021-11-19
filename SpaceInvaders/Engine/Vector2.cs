using System;

namespace SpaceInvaders.Engine
{
    internal class Vector2
    {
        public double X, Y;

        public Vector2(double x = 0, double y = 0)
        {
            X = x;
            Y = y;
        }

        public double Norme()
        {
            return Math.Sqrt(Math.Pow(X, 2) + Math.Pow(Y, 2));
        }

        public static Vector2 operator +(Vector2 v1, Vector2 v2) => new Vector2(v1.X + v2.X, v1.Y + v2.Y);
        public static Vector2 operator -(Vector2 v1, Vector2 v2) => new Vector2(v1.X - v2.X, v1.Y - v2.Y);
        public static Vector2 operator -(Vector2 v1) => new Vector2(-v1.X, -v1.Y);
        public static Vector2 operator *(Vector2 v, double x) => new Vector2(v.X * x, v.Y * x);
        public static Vector2 operator *(double x, Vector2 v) => v * x;
        public static Vector2 operator /(Vector2 v, double x) => v * (1 / x);
        public static Vector2 Right => new Vector2(1, 0);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Up => new Vector2(0, -1);
        public static Vector2 Down => new Vector2(0, 1);
    }
}
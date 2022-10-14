namespace Common
{
    public struct CollisionData
    {
        public bool Up;
        public bool Down;
        public bool Right;
        public bool Left;
        public bool Diagonal;

        public override string ToString()
        {
            return "[UP: " + Up + "]    [DOWN: " + Down + "]   [RIGHT: " + Right + "]    [LEFT: " + Left + "]   [DIAGONAL:" + Diagonal +"]";
        }
    }
}
using BoxesProject.API;
using System;

namespace BoxesProject
{
    internal class Box : IComparable<Box>, IUIBox
    {
        public double Width { get; set; }
        public double Height { get; set; }

        public int CompareTo(Box other)
        {
            if (Width == other.Width) return Height.CompareTo(other.Height);
            return Width.CompareTo(other.Width);
        }

        public Box IncreaseBy(double maximumExceedanceAllowed)
        {
            double exceed = 1.0 + maximumExceedanceAllowed;
            Box box = new Box() { Width = Width * exceed, Height = Height * exceed};

            return box;
        }
    }
}

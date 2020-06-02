using System;
using System.Collections.Generic;

namespace Business
{
    public struct Place
    {
        public string Name { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public Place(string name, double x, double y)
        {
            Name = name;
            X = x;
            Y = y;
        }
    }

    public class Town
    {
        public List<Place> Locations { get; private set; } = new List<Place>();

        public Town(params Place[] places)
        {
            for (int i = 0; i < places.Length; i++)
            {
                Locations.Add(places[i]);
            }
        }

        public static double GetTimeBetweenPlaces(Place a, Place b)
        {
            double time = Math.Sqrt(Math.Pow((a.X - b.X), 2) + Math.Pow((a.Y - b.Y), 2));
            return time;
        }
    }
}
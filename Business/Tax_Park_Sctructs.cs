using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
    public struct Price
    {
        public double MinDist;
        public double MinPrice;
        public double PricePerKm;
        public Price(double md, double mp, double ppkm)
        {
            MinDist = md;
            MinPrice = mp;
            PricePerKm = ppkm;
        }
    }

    public struct Order
    {
        public Place location;
        public Place destination;
        public Order(Place locat, Place dest)
        {
            location = locat;
            destination = dest;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Transactions;

namespace Business
{
    public class Tax_Park: IEnumerable
    {
        private event UserStateHandler UserCarArrived;
        private event UserStateHandler UserCarGone;
        private event UserStateHandler ParkTripStarted;
        private event UserStateHandler ParkTripEnded;
        private event UserStateHandler OrderStarted;
        
        public Town WorkTown { get; private set; }
        public string Name { get; private set; }
        private List<Taxer> Taxers = new List<Taxer>();
        private List<Client> Clients = new List<Client>();
        private Price Prices;
        public Order CurrentOrder { get; private set; }
        public Client CurrentClient { get; private set; }

        public Tax_Park(string name, Town town, double md, double mp, double phm)
        {
            Name = name;
            WorkTown = town;
            Prices = new Price(md, mp, phm);
        }

        public void SetCurrentOrder(Place loc, Place dest)
        {
            CurrentOrder = new Order(loc, dest);
        }

        public void SetCurrentClient(Client client)
        {
            CurrentClient = client;
        }

        public IEnumerator GetEnumerator()
        {
            return (from t in Taxers select t.Name).GetEnumerator();
        }

        public void SetEventHanlders(UserStateHandler userCarArrived, UserStateHandler userCarGone,
            UserStateHandler parkTripStarted, UserStateHandler parkTripEnded, UserStateHandler orderStarted)
        {
            UserCarArrived = userCarArrived;
            UserCarGone = userCarGone;
            ParkTripStarted = parkTripStarted;
            ParkTripEnded = parkTripEnded;
            OrderStarted = orderStarted;
        }
        
        public void AddTaxers(params Taxer[] taxers)
        {
            if (taxers.Length > 0)
            {
                for (int i = 0; i < taxers.Length; i++)
                {
                    Taxers.Add(taxers[i]);
                    Taxers[Taxers.Count - 1].GoneToTheClient += NotifyParkTripStarted;
                    Taxers[Taxers.Count - 1].GoneToTheClient += NotifyUserCarGone;
                    Taxers[Taxers.Count - 1].ComeToTheClient += NotifyUserCarArrived;
                    Taxers[Taxers.Count - 1].StartTrip += NotifyOrderStarted;
                    Taxers[Taxers.Count - 1].EndTrip += NotifyParkTripEnded;
                    Taxers[Taxers.Count - 1].Open();
                }
            }else
            {
                throw new ArgumentException("Empty list of taxers was set!");
            }
        }

        public void AddClient(Client client)
        {
            Clients.Add(client);
        }

        public bool CheckUser(int id)
        {
            if (Clients.Any())
            {
                for (int i = 0; i < Clients.Count; i++)
                {
                    if (Clients[i].Id == id || Clients[i].PhoneNumber == id) { return true; }
                }
            }
            return false;
        }


        public Client FindClient(int n)
        {
            for (int i = 0; i < Clients.Count; i++)
            {
                if (Clients[i].Id == n) { return Clients[i]; } 
                if (Clients[i].PhoneNumber == n) { return Clients[i]; }
            }
            return null;
        }

        public bool IsTaxersDay(Taxer tax, WorkDays day)
        {
            if (tax.Schedule.schedule.Contains(day))
            {
                return true;
            }
            return false;
        }

        public void DelTaxerDay(Taxer tax, WorkDays day)
        {
            tax.Schedule.schedule.Remove(day);
        }

        public void AddTaxerDay(Taxer tax, WorkDays day)
        {
            tax.Schedule.schedule.Add(day);
        }



        public Taxer FindTaxer(string n)
        {
            if (string.IsNullOrEmpty(n)) { return null; }
            if (n.All(x => Char.IsDigit(x)))
            {
                for (int i = 0; i < Taxers.Count; i++)
                {
                    if (Taxers[i].Id == Convert.ToInt32(n)) { return Taxers[i]; }
                }
            }
            else if (n.All(x => Char.IsLetter(x)))
            {
                for (int i = 0; i < Taxers.Count; i++)
                {
                    if (Taxers[i].Name == n) { return Taxers[i]; }
                }
            }
            return null;
        }

        public int GetTaxerSalaryPerTrip(Taxer tax)
        {
            return tax.SalaryPerTrip;
        }

        public void SetTaxerSalaryPerTrip(Taxer tax, int s)
        {
            tax.SalaryPerTrip = s;
        }

        public double GetPrice(Place a, Place b, Client client) 
        {
            double distance = Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
            if (distance <= Prices.MinDist) { return Prices.MinPrice; } else
            {
                return Math.Round(Prices.MinPrice + ((distance - Prices.MinDist) * Prices.PricePerKm) - (Prices.MinPrice + ((distance - Prices.MinDist) * Prices.PricePerKm))*client.Discount/10);
            }
        }


        private void NotifyParkTripStarted(object taxer, UserEventArgs args)
        {
            ParkTripStarted?.Invoke(taxer, args);
        }

        private void NotifyParkTripEnded(object taxer, UserEventArgs args)
        {
            args._Client.TripSucceeded();
            args._Taxer.Salary += args._Taxer.SalaryPerTrip;
            ParkTripEnded?.Invoke(taxer, args);

        }

        private void NotifyUserCarGone(object taxer, UserEventArgs args)
        {
            UserCarGone?.Invoke(taxer, args);
        }

        private void NotifyUserCarArrived(object taxer, UserEventArgs args)
        {
            UserCarArrived?.Invoke(taxer, args);
        }


        public void NotifyOrderStarted(object user, UserEventArgs args)
        {
            OrderStarted?.Invoke(user, args);
        }

        public void TakeOrder(Client client, Place location, Place destination)
        {
            AddClient(client);
            Taxer FreeTaxer = Taxers.OrderBy(x => x.ItStatus).ElementAt(Taxers.Count - 1);
            FreeTaxer.TakeTrip(client, location, destination);
        }

        public WorkWeek GetTaxerSchedule(Taxer taxer)
        {
            return taxer.Schedule;
        }

        public double GetTaxerSalary(Taxer taxer)
        {
            return taxer.Salary;
        }

        public void SetTaxerSalary(Taxer taxer, double s)
        {
            taxer.Salary = s;
        }

        public void DelTaxer(Taxer tax)
        {
            Taxers.Remove(tax);
            tax.Close();
        }



    }
}
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;


namespace Business
{
    public class Taxer : User, IAccount
    {
        internal event UserStateHandler ComeToTheClient;
        internal event UserStateHandler StartTrip;
        internal event UserStateHandler GoneToTheClient;
        internal event UserStateHandler EndTrip;
        public string Car {get; private set; }         
        internal double Salary { get; set; } = 0;     
        internal int SalaryPerTrip { get; set; } = 100;
        internal WorkWeek Schedule { get; set; }
        internal Status ItStatus { get; private set; } = Status.Free;          
        internal Place ItLocation { get; private set; } = new Place("Base", 0.0, 0.0);
        private Client CurrentClient { get;  set; }

        // Init method
        public Taxer(string name, string car, WorkChange type, List<WorkDays> week) : base(name)
        {
            Name = name;
            Car = car;
            Schedule = new WorkWeek(type, week);
        }

        public Taxer()             
        {
            Car = "Red mercedes";
            Salary = 1000;
            Schedule = new WorkWeek(WorkChange.Day, new List<WorkDays> { WorkDays.Monday });
        }

        internal async void TakeTrip(Client client, Place location, Place destination)
        {
            ItStatus = Status.Engaged;
            CurrentClient = client;
            GoneToTheClient?.Invoke(this, new UserEventArgs($"{Name}'s taken an order to client {client.Id} from {location.Name} to {destination.Name}", CurrentClient, this));
            await Task.Run(() => TripSimulator(location, destination));
        }


        private void TripSimulator(Place location, Place destination)
        {
            Thread.Sleep((int)Town.GetTimeBetweenPlaces(ItLocation, location)*100);
            ComeToTheClient?.Invoke(this, new UserEventArgs($"Your {Car} car has just gone to you!", CurrentClient, this));
            var r = new Random();
            Thread.Sleep(r.Next(2,4) * 1000);
            StartTrip?.Invoke(this, new UserEventArgs($"{Name} has successfully taken the {CurrentClient.Name} {CurrentClient.PhoneNumber} client", CurrentClient, this));
            Thread.Sleep((int)Town.GetTimeBetweenPlaces(location, destination)*100);
            ItStatus = Status.Free;
            ItLocation = new Place("Base", 0.0, 0.0);

            EndTrip?.Invoke(this, new UserEventArgs($"{Name} has successfully given a lift to the {CurrentClient.Name} {CurrentClient.Id} client", CurrentClient, this));
            CurrentClient = null;
        }

        public override void Open()       
        {
            base.OnOpened(new UserEventArgs($"New taxer's workplace with an ID {Id} " +
                                            $"has been successfully opened!"));
        }

        public override void Close()      
        {
            base.OnClosed(new UserEventArgs($"The taxer's workplace with an ID {Id} " +
                                            $"has been successfully closed!"));
        }



    }
}
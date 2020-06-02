namespace Business
{
    public class Client : User, IAccount
    {
        internal int Trips { get; private set; } = 0;                      
        internal double Discount { get; private set; } = 0;         
        internal int PhoneNumber { get; private set; } = 0;

        public Client(string name, int phone = 0, double discount = 0) : base(name)
        {
            Discount = discount;
            PhoneNumber = phone;
        }

        public override void Open()       
        {
            base.OnOpened(new UserEventArgs($"New client's account with an ID {Id} " +
                                            $"has been successfully opened!"));
        }

        public override void Close()       
        {
            base.OnClosed(new UserEventArgs($"The client's account with an ID {Id} " +
                                            $"has been successfully closed!"));
        }

        
        private void UpdateDisc()
        {
            if (Trips < 20)
            {
                Discount++;
            }
            else
            {
                Discount =  Discount + 1 / (Discount - Trips);
            }
        }
        
        internal void TripSucceeded()               
        {
            Trips++;
            UpdateDisc();
        }

       
    }
}
using System;

namespace Business
{
    public delegate void UserStateHandler(object obj, UserEventArgs arg);

    public class UserEventArgs : EventArgs
    {
        public string _Message { get; private set; }
        public Client _Client { get; private set; }

        public Taxer _Taxer { get; private set; }
        public UserEventArgs(string m)
        {
            _Message = m;
        }
        public UserEventArgs(string m, Client client, Taxer taxer)
        {
            _Message = m;
            _Client = client;
            _Taxer = taxer;
        }
    }
}

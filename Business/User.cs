using System;
using System.Security.Cryptography.X509Certificates;

namespace Business
{
    public abstract class User : IAccount
    {
        protected event UserStateHandler Opened;
        protected event UserStateHandler Closed;

        public string Name {get; protected set; }  
        internal static int counter = 0;                         
        public int Id { get; protected set; }      

        public User(string name)                   
        {
            Name = name;
            counter++;
            Id = counter;
        }

        public User()                              
        {
            Id = counter++;
            Name = "Unnamed" + Id.ToString();
        }

        protected void CallEvent(UserStateHandler ev, UserEventArgs arg)  
        {
            if (arg != null)
            {
                ev?.Invoke(this, arg);
            }
        }

        protected void OnOpened(UserEventArgs arg)   
        {
            CallEvent(Opened, arg);
        }

        protected void OnClosed(UserEventArgs arg)  
        {
            CallEvent(Closed, arg);
        }

        public virtual void Open()                   
        {
            OnOpened(new UserEventArgs($"New account with an ID {Id} has been successfully opened!"));
        }

        public virtual void Close()                  
        {
            OnClosed(new UserEventArgs($"An account with an ID {Id} has been successfully closed!"));
        }


    }
}
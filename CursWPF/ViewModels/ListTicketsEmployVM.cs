using CursLib.Models;
using CursWPF.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CursWPF.ViewModels
{
   
        public class ListTicketsEmployVM : BaseTools
        {
            public Ticket SelectedItem { get; set; }
            private List<Ticket> ticket;
            private List<User> user;
            private List<Trip> trip;
            private User users;
            private Trip trips;
            public Ticket tickets { get; set; }

            public string Title { get; set; }
            public string FirstName { get; set; }



            public List<Ticket> Ticket
            {
                get => ticket;
                set
                {
                    ticket = value;

                    Signal();
                }
            }

            public List<User> User
            {
                get => user;
                set
                {
                    user = value;

                    Signal();
                }
            }

            public List<Trip> Trip
            {
                get => trip;
                set
                {
                    trip = value;

                    Signal();
                }
            }


            public CommandVM DeleteTickets { get; set; }
            public CommandVM Save { get; set; }


            public ListTicketsEmployVM(User user)
            {
                Task.Run(async () =>
                {
                    var json = await HttpApi.Post("Tickets", "ListTickets", user);
                    Ticket = HttpApi.Deserialize<List<Ticket>>(json);

                    var json2 = await HttpApi.Post("Users", "ListUsers", null);
                    User = HttpApi.Deserialize<List<User>>(json2);

                    var json3 = await HttpApi.Post("Trips", "ListTrips", null);
                    Trip = HttpApi.Deserialize<List<Trip>>(json3);
                });

               
                
            }
        }
    }


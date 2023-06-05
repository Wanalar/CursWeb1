using CursLib.Models;
using CursWPF.Pages;
using CursWPF.Tools;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Xceed.Wpf.Toolkit.Primitives;

namespace CursWPF.ViewModels
{
    public class MainMenuAdminVM: BaseTools
    {
        private List<Bus> bus;
        private List<Trip> trips;
        private Bus buses;
        private Page currentPage;
        public Page CurrentPage
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal();
            }
        }
        public CommandVM nav_infosEmploy { get; set; }
        public CommandVM nav_busesEmploy { get; set; }
        public CommandVM nav_usersEmploy { get; set; }
        public CommandVM nav_tickets { get; set; }
        public CommandVM Save { get; set; }
        public CommandVM DeleteTrip { get; set; }

        public string BusStop { get; set; }
        public string DepartureTime { get; set; }
        public string ArivalTime { get; set; }

        public int Cost { get; set; }

        public Bus SelectedBus
        {
            get => buses;
            set
            {
                buses = value;
                Signal();
            }
        }
        public Trip SelectedItem { get; set; }

        public List<Trip> Trip
        {
            get => trips;
            set
            {
                trips = value;

                Signal();
            }
        }
        public List<Bus> Bus
        {
            get => bus;
            set
            {
                bus = value;

                Signal();
            }
        }
        public User User { get; set; }

        public MainMenuAdminVM(User user) {


            User = user;
            Task.Run(async () =>
            {
                var json = await HttpApi.Post("Trips", "ListTrips", null);
                Trip = HttpApi.Deserialize<List<Trip>>(json);

                var json2 = await HttpApi.Post("Buses", "ListBuses", null);
                Bus = HttpApi.Deserialize<List<Bus>>(json2);
            });


            nav_busesEmploy = new CommandVM(() =>
            {
                CurrentPage = new ListBusesEmploy();
            });
            

            nav_usersEmploy = new CommandVM(() =>
            {
                CurrentPage = new ListUsersEmploy();
            }); 

            nav_tickets = new CommandVM(() =>
            {
                CurrentPage = new ListTickets();
       
            });

            nav_infosEmploy = new CommandVM(() =>
            {
                CurrentPage = new ListInfoEmploy();
            });
            Save = new CommandVM(async () =>
            {
                var json = await HttpApi.Post("Trips", "save", new Trip
                {
                    BusStop = BusStop,
                    ArivalTime = ArivalTime,
                    DepartureTime = DepartureTime,
                    Bus = SelectedBus.BusId,
                    Cost = Cost
                });
                Trip result = HttpApi.Deserialize<Trip>(json);

                MessageBox.Show("Сохранилось");
            });

            DeleteTrip = new CommandVM(async () =>
            {
                var json = await HttpApi.Post("Trips", "delete", SelectedItem.TripId);

                Task.Run(async () =>
                {
                    var json = await HttpApi.Post("Trips", "ListTrips", null);
                    Trip = HttpApi.Deserialize<List<Trip>>(json);
                });
            });
        }
    }
}

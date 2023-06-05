using CursLib.Models;
using CursWPF.Pages;
using CursWPF.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace CursWPF.ViewModels
{
    public class MainMenuGuestVM : BaseTools
    {
        public Page CurrentPage 
        {
            get => currentPage;
            set
            {
                currentPage = value;
                Signal();
            }
        }
        
        

        
        

        

        private List<Bus> bus;
        private List<Trip> trips;
        private Page currentPage;

        public Trip trip { get; set; }


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

        public CommandVM EditTrip { get; set; }
        public CommandVM nav_busesEmploy { get; set; }
        public CommandVM nav_ticketsEmploy { get; set; }
        public CommandVM nav_personalcabinet { get; set; }
        public CommandVM nav_Trips { get; set; }

        public User User { get; set; }

        public MainMenuGuestVM(User user)
        {
            User = user;
            CurrentPage = new ListTripsPage(User);
            Task.Run(async () =>
            {
                var json = await HttpApi.Post("Trips", "ListTrips", null);
                Trip = HttpApi.Deserialize<List<Trip>>(json);

                var json2 = await HttpApi.Post("Buses", "ListBuses", null);
                Bus = HttpApi.Deserialize<List<Bus>>(json2);
            });

            nav_ticketsEmploy = new CommandVM(() =>
            {
                CurrentPage = new ListTicketsEmploy(User);
            });
            nav_personalcabinet = new CommandVM(() =>
            {
                CurrentPage = new PersonalCabinetPage(User);
            });
            nav_Trips = new CommandVM(() =>
            {
                CurrentPage = new ListTripsPage(User);
            });
        }



    }
}

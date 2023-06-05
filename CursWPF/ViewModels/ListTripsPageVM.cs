using CursLib.Models;
using CursWPF.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CursWPF.ViewModels
{
    public class ListTripsPageVM: BaseTools
    {
        private List<Trip> trips;
        private List<Bus> bus;
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
        public Trip SelectedItem { get; set; }
        public ListTripsPageVM(CursLib.Models.User user)
        {
            Task.Run(async () =>
            {
                var json = await HttpApi.Post("Trips", "ListTrips", null);
                Trip = HttpApi.Deserialize<List<Trip>>(json);

                var json2 = await HttpApi.Post("Buses", "ListBuses", null);
                Bus = HttpApi.Deserialize<List<Bus>>(json2);


            });
        }
    }

}

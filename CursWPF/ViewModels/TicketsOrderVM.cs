using CursLib.Models;
using CursWeb;
using CursWPF.Tools;
using Microsoft.AspNetCore.SignalR.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace CursWPF.ViewModels
{
    public class TicketsOrderVM : BaseTools
    {
        private Trip selectedTrip;
        private Trip trip;

        public User User { get; set; }
        public Ticket Ticket { get; set; }
        public Bus Bus { get; set; }
        public int Busid { get; set; }
        public decimal EntryCost { get; set; }
        public Trip Trip 
        { 
            get => trip;
            set
            {
                trip = value;
                Signal();
            }
        }

        public Trip SelectedTrip
        {
            get => selectedTrip;
            set
            {
                selectedTrip = value;
                Signal();
            }
        }
        public DateTime DateDeparture { get; set; } = DateTime.Now;

        public TicketsOrderVM(User user, Window win)
        {
            User = user;
            Trips = Avto_VakzalContext.GetInstance().Trips.ToList();
            Cost = new CommandVM(() =>
            {
                Trip = Avto_VakzalContext.GetInstance().Trips.FirstOrDefault(s => s.TripId == SelectedTrip.TripId);
            });
            Save = new CommandVM(() =>
            {
                var user = Avto_VakzalContext.GetInstance().Users.FirstOrDefault(s => s.UserId == User.UserId);
                if (SelectedTrip != null)
                {
                    if(Trip == null)
                    {
                        MessageBox.Show("Для начала узнайте стоимость. У нас приятные цены!");
                        return;
                    }
                    else
                    {
                        if (EntryCost == Trip.Cost)
                        {
                            if (EntryCost > user.Bill || user.Bill == null || DateDeparture.Date <= DateTime.Now.Date)
                            {
                                MessageBox.Show("У вас недостаточно средств или вы выбрали дату раньше сегодняшней");
                                return;
                            }
                            else
                            {
                                Bus = Avto_VakzalContext.GetInstance().Buses.FirstOrDefault(s => s.BusId == SelectedTrip.Bus);
                                if (Bus.Site != 0)
                                {
                                    Ticket = new Ticket() { Iduser = user.UserId, Trip = SelectedTrip.TripId, Date = DateDeparture.Date };
                                    Avto_VakzalContext.GetInstance().Tickets.Add(Ticket);
                                    Avto_VakzalContext.GetInstance().SaveChanges();

                                    user = Avto_VakzalContext.GetInstance().Users.FirstOrDefault(s => s.UserId == user.UserId);
                                    user.Bill = user.Bill - EntryCost;
                                    Avto_VakzalContext.GetInstance().Users.Update(user);
                                    Avto_VakzalContext.GetInstance().SaveChanges();

                                    Bus.Site = Bus.Site - 1;
                                    Avto_VakzalContext.GetInstance().Buses.Update(Bus);
                                    Avto_VakzalContext.GetInstance().SaveChanges();
                                    MessageBox.Show("Билет успешно куплен");
                                    SendEmailAsync();
                                    win.Close();
                                    return;
                                }
                                else
                                {
                                    MessageBox.Show("Мест нет, пока оформить невозможно");
                                    return;
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("Введите нужную сумму");
                            return;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Ошибка, выберите маршрут");
                    return;
                }
            });

            
        }

        private async Task SendEmailAsync()
        {
            var lastTicket = Avto_VakzalContext.GetInstance().Tickets.ToList().LastOrDefault( s => s.Iduser == User.UserId);
            var normalCost = Convert.ToString(lastTicket.TripNavigation.Cost);
            var normalDate = Convert.ToString(lastTicket.Date);
            MailAddress from = new MailAddress("persaevkirill@gmail.com", "Автовокзал");
            MailAddress to = new MailAddress($"{User.Email}");
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Покупка билета";
            m.Body = $"Вы купили билет на маршрут {lastTicket.TripNavigation.BusStop}.\nCумма покупки: {normalCost.Split(',')[0]} рублей.\nИНФОРМАЦИЯ О ПОЕЗДКЕ:\nДата рейса: {normalDate.Split(' ')[0]}.\nНомер автобуса: {lastTicket.TripNavigation.BusNavigation.Number}.\nВремя отбытия автобуса: {lastTicket.TripNavigation.DepartureTime}.\nВремя прибытия на конечную станцию: {lastTicket.TripNavigation.ArivalTime}.";
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("persaevkirill@gmail.com", "gwjf fyvu jpzh fyvc");
            smtp.EnableSsl = true;
            await smtp.SendMailAsync(m);
        }
        public List<Trip> Trips { get; set; }
        

        public CommandVM Save { get; set; }
        public CommandVM Cost { get; set; }

    }

}

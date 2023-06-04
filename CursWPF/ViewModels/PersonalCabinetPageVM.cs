using CursLib.Models;
using CursWeb;
using CursWPF.Tools;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CursWPF.ViewModels
{
    public class PersonalCabinetPageVM : BaseTools
    {
        private User user;
        private decimal money;

        public User User
        {
            get => user;
            set
            {
                user = value;
                Signal();
            }
        }
        public decimal Money 
        {
            get => money;
            set
            {
                money = value;
                Signal();
            }
        }
        public CommandVM AddMoney { get; set; }
        public PersonalCabinetPageVM(CursLib.Models.User user)
        {
            User = user;
            AddMoney = new CommandVM(() =>
            {
                if (Money > 0)
                {
                    var oldBill = Avto_VakzalContext.GetInstance().Users.FirstOrDefault(s => s.UserId == User.UserId);
                    if (oldBill.Bill == null)
                    {
                        oldBill.Bill = Money;
                        Avto_VakzalContext.GetInstance().Users.Update(oldBill);
                        Avto_VakzalContext.GetInstance().SaveChanges();
                        MessageBox.Show("Вы пополнили счёт");
                        Money = 0;
                        Signal(nameof(Money));
                        User = Avto_VakzalContext.GetInstance().Users.FirstOrDefault(s => s.UserId == User.UserId);
                        return;
                    }
                    else
                    {
                        oldBill.Bill = oldBill.Bill += Money;
                        Avto_VakzalContext.GetInstance().Users.Update(oldBill);
                        Avto_VakzalContext.GetInstance().SaveChanges();
                        MessageBox.Show("Вы пополнили счёт");
                        Money = 0;
                        Signal(nameof(Money));
                        User = Avto_VakzalContext.GetInstance().Users.FirstOrDefault(s => s.UserId == User.UserId);
                        return;
                    }

                }
            });
        }
    }
}

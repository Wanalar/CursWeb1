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
        private User oldBill;

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
        public User OldBill 
        {
            get => oldBill;
            set
            {
                oldBill = value;
                Signal();
            }
        }
        public PersonalCabinetPageVM(CursLib.Models.User user)
        {
            User = user;
            Signal(nameof(User.Bill));
            OldBill = Avto_VakzalContext.GetInstance().Users.FirstOrDefault(s => s.UserId == User.UserId);
            AddMoney = new CommandVM(() =>
            {
                if (Money > 0)
                {

                    if (OldBill.Bill == null)
                    {
                        OldBill.Bill = Money;
                        Avto_VakzalContext.GetInstance().Users.Update(OldBill);
                        Avto_VakzalContext.GetInstance().SaveChanges();
                        MessageBox.Show("Вы пополнили счёт");
                        Money = 0;
                        Signal(nameof(Money));
                        OldBill = Avto_VakzalContext.GetInstance().Users.FirstOrDefault(s => s.UserId == User.UserId);
                        return;
                    }
                    else
                    {
                        OldBill.Bill = OldBill.Bill += Money;
                        Avto_VakzalContext.GetInstance().Users.Update(OldBill);
                        Avto_VakzalContext.GetInstance().SaveChanges();
                        MessageBox.Show("Вы пополнили счёт");
                        Money = 0;
                        Signal(nameof(Money));
                        OldBill = Avto_VakzalContext.GetInstance().Users.FirstOrDefault(s => s.UserId == User.UserId);
                        return;
                    }

                }
            });
        }
    }
}

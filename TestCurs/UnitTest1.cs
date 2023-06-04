using CursLib.Models;
using CursWeb;

namespace UnitTest1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestTrySaveUser()
        {
            using (var context = new Avto_VakzalContext())
            {
                var user = new User() { FirstName = "Альберт", SecondName = "Валерьевич", Patronymic = "Похомов", Login = "Vladow12", Password = "1234" };
                Avto_VakzalContext.GetInstance().Users.Add(user);
                Avto_VakzalContext.GetInstance().SaveChanges();
                var result = context.Users.FirstOrDefault(s => s.FirstName == user.FirstName && s.SecondName == user.SecondName && s.Patronymic == user.Patronymic && s.Login == user.Login && s.Password == user.Password);
                Assert.IsNull(result);
            }
        }

        [Test]
        public void TestTryRemoveUser()
        { 
            using (var context = new Avto_VakzalContext())
            {
                var user = new User() { Login = "Vladow12", Password = "1234" };
                context.Users.Add(user);
                context.SaveChanges();
                context.Users.Remove(user);
                context.SaveChanges();
                var result = context.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
                Assert.IsNull(result);
            }
        }

        public void TestUpdate()
        {
            using (var context = new Avto_VakzalContext())
            {
                var user = new User() { FirstName = "Цой Вмктор Робертович", Bill = 18500 };
                context.Add(user);
                context.SaveChanges();
                user.Bill = 13300;
                context.SaveChanges();
                var result = context.Users.FirstOrDefault(s => s.FirstName == "Цой Вмктор Робертович");
                Assert.IsNotNull(result);
                Assert.AreEqual(user.Bill, result.Bill);
            }
        }



    }
}
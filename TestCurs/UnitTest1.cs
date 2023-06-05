using CursWeb;
using CursWPF.ViewModels;

namespace UnitTest1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Delete_User_Test()
        {
            using (var context = new Avto_VakzalContext())
            {
                var user = new User() { Login = "Влад", Password = "Бубик" };
                context.Users.Add(user);
                context.SaveChanges();
                context.Users.Remove(user);
                context.SaveChanges();
                var result = context.Users.FirstOrDefault(u => u.Login == user.Login && u.Password == user.Password);
                Assert.IsNull(result);
            }
        }

        [Test]
        public void Update_User_Test()
        {
            using (var context = new Avto_VakzalContext())
            {
                var user = new User() { LastName = "Бибка", PhoneNumber = 8964738573 };
                context.Add(user);
                context.SaveChanges();
                user.PhoneNumber = 8957384835;
                context.SaveChanges();
                var result = context.Users.FirstOrDefault(s => s.LastName == "Бибка");
                Assert.IsNotNull(result);
                Assert.AreEqual(user.PhoneNumber, result.PhoneNumber);
            }
        }

        [Test]
        public void Save_User_Test()
        {
            using (var context = new Avto_VakzalContext())
            {
                var user = new User() { FirstName = "Валерий", LastName = "Абобка", PatronomycName = "Сильвесторович" };
                user05_1Context.GetInstance().Users.Add(user);
                user05_1Context.GetInstance().SaveChanges();
                var result = context.Users.FirstOrDefault(s => s.LastName == user.LastName && s.FirstName ==
                user.FirstName && s.PatronomycName == user.PatronomycName);
                Assert.IsNotNull(result);
            }
        }
        [Test]
        public void Update_User2_Test()
        {
            using (var context = new Avto_VakzalContext())
            {
                var user = new User() { FirstName = "Артём", LastName = "Прошкин", PatronomycName = "Романович" };
                user05_1Context.GetInstance().Users.Add(user);
                user05_1Context.GetInstance().SaveChanges();
                var lastUser = user05_1Context.GetInstance().Users.FirstOrDefault(s => s.LastName == "Прошкин" && s.FirstName == "Артём" && s.PatronomycName == "Романович");
                user05_1Context.GetInstance().Users.Update(lastUser);
                user05_1Context.GetInstance().SaveChanges();
                var updateUser = user05_1Context.GetInstance().Users.FirstOrDefault(s => s.LastName == "Прошкин" && s.FirstName == "Артём" && s.PatronomycName == "Романович");
                Assert.IsNotNull(updateUser);
            }
        }

        [Test]
        public void User_Creation_Correct_Test()
        {
            var user = new MainMenuAdminVM();
            Assert.IsNotNull(user);
        }

        [Test]
        public async Task Command_Save_Check()
        {
            var vm = new ListUserVM();
            Assert.IsNotNull(vm.Save);
        }

        [Test]
        public async Task Command_Delete_Check()
        {
            var vm = new ListUserVM();
            Assert.IsNotNull(vm.DeleteUser);
        }


    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BeautySalonVogue.Data;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void PositiveMethod()
        {
            string testEmail = "artiomShpak@mail.ru";
            bool answer = BeautySalonVogue.Manager.IsValidEmail(testEmail);
            Assert.AreEqual(true, answer);
        }
        [TestMethod]
        public void NegativeMethod()
        {
            string testEmail = "artiomShpakmail.ru";
            bool answer = BeautySalonVogue.Manager.IsValidEmail(testEmail);
            Assert.AreEqual(false, answer);
        }
        [TestMethod]
        public void DeleteDataMethod()
        {
            bool result = false;
            try
            {
                Client clientToDelete = BeautySalonBaseEntities.getContext().Client.Where(p => p.Id == 102).First();
                BeautySalonBaseEntities.getContext().Client.Remove(clientToDelete);
                result = true;
            }
            catch
            {
                result = false;
            }
            Assert.AreEqual(true, result);
        }
    }
}

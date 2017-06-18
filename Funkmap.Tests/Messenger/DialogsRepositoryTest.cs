using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Repositories;
using Funkmap.Messenger.Data.Repositories.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Funkmap.Tests.Messenger.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Messenger
{
    [TestClass]
    public class DialogsRepositoryTest
    {
        private IDialogRepository _dialogRepository;

        [TestInitialize]
        public void Initialize()
        {
            _dialogRepository = new DialogRepository(MessengerDbProvider.DropAndCreateDatabase.GetCollection<DialogEntity>(CollectionNameProvider.DialogsCollectionName));
        }

        [TestMethod]
        public void GetAllDialogs()
        {
            var allDialogs = _dialogRepository.GetAllAsync().Result;
            Assert.AreEqual(allDialogs.Count, 7);
        }

        [TestMethod]
        public void GetUserDialogs()
        {
            var dialogs = _dialogRepository.GetUserDialogs("rogulenkoko").Result;
            Assert.AreEqual(dialogs.Count, 4);

            dialogs = _dialogRepository.GetUserDialogs("test").Result;
            Assert.AreEqual(dialogs.Count, 2);
        }
    }
}

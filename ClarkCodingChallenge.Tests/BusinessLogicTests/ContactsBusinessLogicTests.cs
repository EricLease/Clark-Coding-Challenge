using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Bogus;
using ClarkCodingChallenge.DataAccess;
using ClarkCodingChallenge.DataAccess.Models;
using ClarkCodingChallenge.BusinessLogic;

namespace ClarkCodingChallenge.Tests.BusinessLogicTests
{
    [TestClass]
    public class ContactsBusinessLogicTests
    {
        private const string _fixedLastName = "Fixed";
        private const int _fixedLastNameCnt = 5;
        private const int _randomLastNameCnt = 10;

        private readonly Random _rng;
        private int _id = 0;
        private List<Contact> _contacts;
        private List<Contact> _fixedLastNameContacts;
        private List<Contact> _randomLastNameContacts;

        private IContactsRepository _repository;
        private ContactsService _service;

        public ContactsBusinessLogicTests()
        {
            _rng = new Random();
            CreateContacts();
            SetupRepository();
            _service = new ContactsService(_repository);
        }

        [TestMethod]
        public void GetContact_ReturnsExistingContact_WhenIdValid()
        {
            var id = _rng.Next(0, _id);
            var contact = _service.GetContact(id);

            Assert.AreEqual(contact.Id, id);
            Assert.IsNotNull(contact.FirstName);
            Assert.IsNotNull(contact.LastName);
            Assert.IsNotNull(contact.Email);
        }

        [TestMethod]
        public void GetContact_ReturnsNewContact_WhenIdNegative()
        {
            var id = -1;
            var contact = _service.GetContact(id);

            Assert.AreEqual(contact.Id, id);
            Assert.IsNull(contact.FirstName);
            Assert.IsNull(contact.LastName);
            Assert.IsNull(contact.Email);
        }

        [TestMethod]
        public void GetContact_ReturnsNewContact_WhenIdOutOfRange()
        {
            var id = _id;
            var contact = _service.GetContact(id);

            Assert.AreEqual(contact.Id, -1);
            Assert.IsNull(contact.FirstName);
            Assert.IsNull(contact.LastName);
            Assert.IsNull(contact.Email);
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void GetExistingContacts_ReturnsFilteredList_WhenFilterSupplied(int sortOrder)
        {
            var filter = _fixedLastName;
            var expectedCount = _fixedLastNameContacts.Count;
            var expected = sortOrder == 1
                ? _fixedLastNameContacts.OrderByDescending(c => c.LastName).ThenByDescending(c => c.FirstName).ToList()
                : _fixedLastNameContacts.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
            var contacts = _service.GetExistingContacts(sortOrder, filter);

            Assert.AreEqual(contacts.Count, expectedCount);

            for (var i = 0; i < expectedCount; i++)
            {
                var c1 = contacts[i];
                var c2 = expected[i];
                Assert.AreEqual(c1.Id, c2.Id);
                Assert.AreEqual(c1.FirstName, c2.FirstName);
                Assert.AreEqual(c1.LastName, c2.LastName);
                Assert.AreEqual(c1.Email, c2.Email);
            }
        }

        [TestMethod]
        [DataRow(0)]
        [DataRow(1)]
        public void GetExistingContacts_ReturnsUnfilteredList_WhenFilterOmitted(int sortOrder)
        {
            string filter = null;
            var expectedCount = _contacts.Count;
            var expected = sortOrder == 1
                ? _contacts.OrderByDescending(c => c.LastName).ThenByDescending(c => c.FirstName).ToList()
                : _contacts.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).ToList();
            var contacts = _service.GetExistingContacts(sortOrder, filter);

            Assert.AreEqual(contacts.Count, expectedCount);

            for (var i = 0; i < expectedCount; i++)
            {
                var c1 = contacts[i];
                var c2 = expected[i];
                Assert.AreEqual(c1.Id, c2.Id);
                Assert.AreEqual(c1.FirstName, c2.FirstName);
                Assert.AreEqual(c1.LastName, c2.LastName);
                Assert.AreEqual(c1.Email, c2.Email);
            }
        }

        private void CreateContacts()
        {
            var fixedLastNameFaker = new Faker<Contact>()
                .StrictMode(true)
                .RuleFor(c => c.Id, f => _id++)
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => _fixedLastName)
                .RuleFor(c => c.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName));
            var randomLastNameFaker = new Faker<Contact>()
                .StrictMode(true)
                .RuleFor(c => c.Id, f => _id++)
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, (f, u) => f.Internet.Email(u.FirstName, u.LastName));
            
            _fixedLastNameContacts = fixedLastNameFaker.Generate(_fixedLastNameCnt);
            _randomLastNameContacts = randomLastNameFaker.Generate(_randomLastNameCnt);
            _contacts = new List<Contact>();
            _contacts.AddRange(_fixedLastNameContacts);
            _contacts.AddRange(_randomLastNameContacts);
        }

        private void SetupRepository()
        {
            var mockRepo = new Mock<IContactsRepository>();

            mockRepo.Setup(r => r.GetContact(It.IsNotIn<int>(0, _id - 1))).Returns((Contact)null);

            for (var i = 0; i < _id; i++)
            {
                mockRepo.Setup(r => r.GetContact(i)).Returns(_contacts[i]);
            }

            mockRepo
                .Setup(r => r.GetContacts(ListSortDirection.Ascending, _fixedLastName))
                .Returns(_fixedLastNameContacts.OrderBy(c => c.LastName).ThenBy(c=> c.FirstName).AsQueryable());

            mockRepo
                .Setup(r => r.GetContacts(ListSortDirection.Descending, _fixedLastName))
                .Returns(_fixedLastNameContacts.OrderByDescending(c => c.LastName).ThenByDescending(c => c.FirstName).AsQueryable());

            mockRepo
                .Setup(r => r.GetContacts(ListSortDirection.Ascending, null))
                .Returns(_contacts.OrderBy(c => c.LastName).ThenBy(c => c.FirstName).AsQueryable());

            mockRepo
                .Setup(r => r.GetContacts(ListSortDirection.Descending, null))
                .Returns(_contacts.OrderByDescending(c => c.LastName).ThenByDescending(c => c.FirstName).AsQueryable());

            _repository = mockRepo.Object;
        }
    }
}

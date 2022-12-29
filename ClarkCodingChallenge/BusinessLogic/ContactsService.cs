using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ClarkCodingChallenge.DataAccess;
using ClarkCodingChallenge.Models;

namespace ClarkCodingChallenge.BusinessLogic
{
    public class ContactsService : IContactsService
    {
        private readonly IContactsRepository _contacts;

        public ContactsService(IContactsRepository contacts)
        {
            _contacts = contacts;
        }

        public ContactViewModel GetContact(int id)
        {
            var contact = _contacts.GetContact(id);

            return contact == null
               ? new ContactViewModel { Id = -1 }
               : new ContactViewModel
               {
                   Id = contact.Id,
                   FirstName = contact.FirstName,
                   LastName = contact.LastName,
                   Email = contact.Email
               };
        }

        public IList<ContactViewModel> GetExistingContacts(int sort, string filter = null)
        {
            var sortDirection = sort == 1
                ? ListSortDirection.Descending : ListSortDirection.Ascending;

            return _contacts
                .GetContacts(sortDirection, filter)
                .Select(c => new ContactViewModel
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email
                })
                .ToList();
        }

        public ContactsViewModel GetContactsViewModel(int id, int sort, string filter = null)
        {
            return new ContactsViewModel
            {
                Contact = GetContact(id),
                SortDirection = sort,
                Filter = filter
            };
        }

        public bool SaveContact(ContactViewModel contact)
            => contact.Id > -1
                ? _contacts.UpdateContact(contact.Id, contact.FirstName, contact.LastName, contact.Email)
                : (_contacts.CreateContact(contact.FirstName, contact.LastName, contact.Email)?.Id ?? -1) > -1;
    }
}

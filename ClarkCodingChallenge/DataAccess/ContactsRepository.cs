using System;
using System.Linq;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using ClarkCodingChallenge.DataAccess.Models;

namespace ClarkCodingChallenge.DataAccess
{
    public class ContactsRepository : IContactsRepository
    {
        private readonly DataContext _context;

        public ContactsRepository()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "DevDB")
                .Options;

            _context = new DataContext(options);
        }

        public IQueryable<Contact> GetContacts(
            ListSortDirection sortDirection = ListSortDirection.Ascending,
            string filter = null)
        {
            filter = filter?.Trim().ToLowerInvariant();

            var applyFilter = !string.IsNullOrWhiteSpace(filter);
            var results = _context.Contacts
                .Where(c => !applyFilter || c.LastName.ToLowerInvariant() == filter);

            if (sortDirection == ListSortDirection.Ascending)
            {
                return results
                    .OrderBy(c => c.LastName)
                    .ThenBy(c => c.FirstName);
            }

            return results
                .OrderByDescending(c => c.LastName)
                .ThenByDescending(c => c.FirstName);
        }

        public Contact GetContact(int id) => _context.Contacts.Find(id);

        public Contact CreateContact(string first, string last, string email)
        {
            var contact = _context.Contacts.Add(new Contact()
            {
                FirstName = first?.Trim(),
                LastName = last?.Trim(),
                Email = email?.Trim()
            });

            _context.SaveChanges();

            return contact.Entity;
        }

        public bool DeleteContact(int id)
        {
            if (id < 0) return false;

            var contact = _context.Contacts.Find(id);

            if (contact == null) return false;

            _context.Contacts.Remove(contact);
            _context.SaveChanges();

            return true;
        }

        public bool UpdateContact(int id, string first, string last, string email)
        {
            var contact = _context.Contacts.Find(id);

            if (contact == null) return false;

            contact.FirstName = first?.Trim();
            contact.LastName = last?.Trim();
            contact.Email = email?.Trim();

            _context.SaveChanges();

            return true;
        }

        #region Dispose pattern implementation

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion Dispose pattern implementation
    }
}

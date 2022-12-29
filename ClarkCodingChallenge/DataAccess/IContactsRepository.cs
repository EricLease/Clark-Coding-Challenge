using System;
using System.ComponentModel;
using System.Linq;
using ClarkCodingChallenge.DataAccess.Models;

namespace ClarkCodingChallenge.DataAccess
{
    public interface IContactsRepository : IDisposable
    {
        IQueryable<Contact> GetContacts(ListSortDirection sortDirection = ListSortDirection.Ascending, string filter = null);
        Contact GetContact(int id);
        Contact CreateContact(string first, string last, string email);
        bool UpdateContact(int id, string first, string last, string email);
        bool DeleteContact(int id);
    }
}
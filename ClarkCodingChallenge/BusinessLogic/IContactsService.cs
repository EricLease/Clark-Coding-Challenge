using System.Collections.Generic;
using ClarkCodingChallenge.Models;

namespace ClarkCodingChallenge.BusinessLogic
{
    public interface IContactsService
    {
        ContactViewModel GetContact(int id);
        ContactsViewModel GetContactsViewModel(int id, int sort,  string filter = null);
        IList<ContactViewModel> GetExistingContacts(int sort, string filter = null);
        bool SaveContact(ContactViewModel vm);
    }
}
using System.Collections.Generic;

namespace ClarkCodingChallenge.Models
{
    public class ContactsViewModel
    {
        public ContactViewModel Contact { get; set; }
        public int SortDirection { get; set; }
        public string Filter { get; set; }
    }
}

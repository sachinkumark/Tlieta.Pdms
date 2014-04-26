﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tlieta.Pdms.DB;

namespace Tlieta.Pdms.Web.Models
{
    public class ContactData : BaseData
    {
        public List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();
            try
            {
                contacts = (from c in entities.Contacts select c).OrderBy(x => x.ContactName).ToList();
            }
            catch { }
            return contacts;
        }

        public bool Add(Contact model)
        {
            try
            {
                entities.Contacts.Add(model);
                entities.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool Update(Contact model)
        {
            try
            {
                Contact contact = entities.Contacts.Where(x => x.ContactId == model.ContactId).SingleOrDefault();
                if (contact != null)
                {
                    entities.Entry(contact).CurrentValues.SetValues(model);
                    entities.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch { return false; }
        }

        public bool Delete(int Id)
        {
            var result = entities.Contacts.Where(x => x.ContactId == Id);
            if (result.Count() > 0)
            {
                Contact contact = result.First();
                entities.Contacts.Remove(contact);
                entities.SaveChanges();
            }
            return true;
        }
    }
}
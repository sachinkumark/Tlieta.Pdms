﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Tlieta.Pdms.Web.Models
{
    public class ContactData
    {
        PDMSDatabaseEntities entities = new PDMSDatabaseEntities();

        public List<Contact> GetContacts()
        {
            List<Contact> contacts = new List<Contact>();
            try
            {
                contacts = (from c in entities.Contacts select c).ToList();
            }
            catch { }
            return contacts;
        }

        public bool AddContact(Contact model)
        {
            try
            {
                entities.Contacts.Add(model);
                entities.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public bool UpdateContact(Contact model)
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
    }
}
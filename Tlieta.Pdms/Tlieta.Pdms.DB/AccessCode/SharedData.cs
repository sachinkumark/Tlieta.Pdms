﻿using System.Linq;
using System.Collections.Generic;
using System;

namespace Tlieta.Pdms.DB
{
    public class SharedData : BaseData
    {
        public SharedData()
        {
        }

        public string GetSerialKey()
        {
            try
            {
                string serialkey = "";
                List<Product> products = (from p in entities.Products select p).ToList();
                if (products.Count > 0)
                {
                    serialkey = products.First().ProductKey;
                }
                else
                {
                    serialkey = FingerPrint.Value();
                    entities.Products.Add(new Product()
                    {
                        ProductKey = serialkey
                    });
                    entities.SaveChanges();
                }
                return serialkey;
            }
            catch (Exception x)
            {
                throw x;
            }
        }
    }
} 

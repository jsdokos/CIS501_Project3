﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    [Serializable()]
    public class Transaction
    {
        private List<SubTransaction> itemsPurchased;
        private Customer customerName;
        private decimal totalPrice;


        private void addNewSubTransaction(Book bookToAdd, int numberToAdd)
        {
            itemsPurchased.Add(new SubTransaction(bookToAdd, numberToAdd));
            totalPrice += bookToAdd.price*numberToAdd;
        }

    }
}
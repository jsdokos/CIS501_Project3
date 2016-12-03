﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    public class Customer
    {
        private string firstName;
        private string lastName;
        private string userName;
        private string password;
        private string email;
        private string address;
        private string phoneNumber;

        private List<Transaction> pastTransactions;
        private List<SubTransaction> currentCart;

        private List<Book> wishList;

        public Customer(string firstName, string lastName, string userName, string password, string email, string address,
            string phoneNumber)
        {

            this.firstName = firstName;
            this.lastName = lastName;
            this.userName = userName;
            this.password = password;
            this.email = email;
            this.address = address;
            this.phoneNumber = phoneNumber;
        }

    }

}
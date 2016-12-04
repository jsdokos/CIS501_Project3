﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    public class ControlBookShop
    {
        public List<Customer> listOfCustomers;
        public List<Book> listOfBooks;
        public List<Transaction> listOfPendingTransactions;
        public List<Transaction> listOfCompleteTransactions;
        public Dictionary<string,Customer> LoginDictionary = new Dictionary<string, Customer>();
        public Customer LoggedinCustomer = null;

        public ControlBookShop()
        {
            listOfCustomers = new List<Customer>();
            listOfBooks = new List<Book>();
            listOfPendingTransactions = new List<Transaction>();
            listOfCompleteTransactions = new List<Transaction>();
        }

        public bool isCustomerLoggedIn
        {
            get
            {
                if (LoggedinCustomer == null)
                    return false;
                else
                    return true;
            }
        }

        public ControlBookShop(List<Customer> listOfCustomers, List<Book> listOfBooks, List<Transaction> listOfPendingTransactions, List<Transaction> listOfCompleteTransactions)
        {
            this.listOfCustomers = listOfCustomers;
            this.listOfBooks = listOfBooks;
            this.listOfPendingTransactions = listOfPendingTransactions;
            this.listOfCompleteTransactions = listOfCompleteTransactions;

            //LoginDictionary.Add();
        }

        //might be able to remove
        public void updateObject(List<Customer> listOfCustomers, List<Book> listOfBooks,
            List<Transaction> listOfPendingTransactions, List<Transaction> listOfCompleteTransactions)
        {
            this.listOfCustomers = listOfCustomers;
            this.listOfBooks = listOfBooks;
            this.listOfPendingTransactions = listOfPendingTransactions;
            this.listOfCompleteTransactions = listOfCompleteTransactions;
        }

        public void addCustomerTodictionary(string username, Customer cust)
        {
            LoginDictionary.Add(username, cust);
        }

        public Customer findCustomerLogin(string user, string pass)
        {
            foreach (Customer cust in this.listOfCustomers)
            {
                if (cust.userName.Equals(user) && cust.password.Equals(pass))
                {
                    LoggedinCustomer = cust;

                    return cust;
                }
            }
            throw new BookShopException("Customer does not exist.");
        }

        public void logoutCustomer()
        {
            if (LoggedinCustomer != null)
            {
                LoggedinCustomer = null;
            }
            else
            {
                throw new BookShopException("There is no cutomer logged in.");
            }
        }

        public void addNewCustomer(string firstName, string lastName, string userName, string password, string email, string address,
            string phoneNumber)
        {
            findDuplicateCustomers(userName);
            listOfCustomers.Add(new Customer(firstName, lastName,  userName,  password, email, address, phoneNumber));
        }

        public void editCustomerInformation(ref CustomerDialog customerDialog)
        {
            if (LoggedinCustomer != null)
            {
                customerDialog.FirstName = LoggedinCustomer.firstName;
                customerDialog.LastName = LoggedinCustomer.lastName;
                customerDialog.UserName = LoggedinCustomer.userName;
                customerDialog.Password = LoggedinCustomer.password;
                customerDialog.EMailAddress = LoggedinCustomer.email;
                customerDialog.Address = LoggedinCustomer.address;
                customerDialog.TelephoneNumber = LoggedinCustomer.phoneNumber;
            }
        }

        public void addEditedCustomer(ref CustomerDialog customerDialog)
        {
            LoggedinCustomer.firstName = customerDialog.FirstName ;
            LoggedinCustomer.lastName = customerDialog.LastName ;
            LoggedinCustomer.userName = customerDialog.UserName ;
            LoggedinCustomer.phoneNumber = customerDialog.Password ;
            customerDialog.EMailAddress = LoggedinCustomer.email;
            LoggedinCustomer.address = customerDialog.Address;
            LoggedinCustomer.phoneNumber = customerDialog.TelephoneNumber;
        }

        public void editCurrentCustomer(string firstName, string lastName, string userName, string password, string email, string address,
            string phoneNumber)
        {
            LoggedinCustomer.firstName = firstName;
            LoggedinCustomer.lastName = lastName;
            LoggedinCustomer.userName = userName;
            LoggedinCustomer.password = password;
            LoggedinCustomer.email = email;
            LoggedinCustomer.address = address;
            LoggedinCustomer.phoneNumber = phoneNumber;
        }

        public void findDuplicateCustomers(string userName)
        {
            foreach (Customer cust in listOfCustomers)
            {
                if (userName.Equals(cust.userName))
                {
                    throw new BookShopException("Your username has already been registered. Please try again.");
                }
            }
        }

        public void addBookToCustomerCart(string isbn)
        {
            if (isCustomerLoggedIn)
            {
                LoggedinCustomer.addBookToCart(findBookByISBN(isbn));
            }
            else
            {
                throw new BookShopException("There is no customer logged in currently.");
            }
        }

        public void addBookToCustomerWishList(string isbn)
        {
            if (isCustomerLoggedIn)
            {
                LoggedinCustomer.addBookToWishList(findBookByISBN(isbn));
            }
            else
            {
                throw new BookShopException("There is no customer logged in currently.");
            }
        }

        public void removeBookFromCustomerWishList(string isbn)
        {
            if (isCustomerLoggedIn)
            {
                LoggedinCustomer.addBookToWishList(findBookByISBN(isbn));
            }
            else
            {
                throw new BookShopException("There is no customer logged in currently.");
            }
        }

        public Book findBookByISBN(string isbn)
        {
            foreach (Book bookToFind in listOfBooks)
            {
                if (bookToFind.isbn.Equals(isbn))
                    return bookToFind;
            }
            throw new BookShopException("Unable to find that book.");
        }

        public void updateBookInformationDialog(ref BookInformationDialog infoDialog, ref ListBooksDialog listDialog)
        {
            //bookInformationDialog.BookTitle = BookShopControl.listOfBooks[listBooksDialog.SelectedIndex].name;
            //bookInformationDialog.Author = BookShopControl.listOfBooks[listBooksDialog.SelectedIndex].author;
            //bookInformationDialog.Publisher = BookShopControl.listOfBooks[listBooksDialog.SelectedIndex].publisher;
            //bookInformationDialog.ISBN = BookShopControl.listOfBooks[listBooksDialog.SelectedIndex].isbn;
            //bookInformationDialog.Date = BookShopControl.listOfBooks[listBooksDialog.SelectedIndex].date;
            //bookInformationDialog.Price = BookShopControl.listOfBooks[listBooksDialog.SelectedIndex].price;
            //bookInformationDialog.Stock = BookShopControl.listOfBooks[listBooksDialog.SelectedIndex].stock;

            infoDialog.BookTitle = listOfBooks[listDialog.SelectedIndex].name;
            infoDialog.Author = listOfBooks[listDialog.SelectedIndex].author;
            infoDialog.Publisher = listOfBooks[listDialog.SelectedIndex].publisher;
            infoDialog.ISBN = listOfBooks[listDialog.SelectedIndex].isbn;
            infoDialog.Date = listOfBooks[listDialog.SelectedIndex].date;
            infoDialog.Price = listOfBooks[listDialog.SelectedIndex].price;
            infoDialog.Stock = listOfBooks[listDialog.SelectedIndex].stock;
        }

        public void showCartInformation(ref CartDialog cart)
        {
            //if (!BookShopControl.isCustomerLoggedIn)
            //{
            //    throw new BookShopException("You are not logged in.");
            //}
            //if (BookShopControl.LoggedinCustomer.currentCart.subTransactionCount <= 0)
            //{
            //    throw new BookShopException("There are no items in your cart.");
            //}
            //for (int i = 0; i < BookShopControl.LoggedinCustomer.currentCart.subTransactionCount; i++)
            //{
            //    cartDialog.AddDisplayItems("\"" + BookShopControl.LoggedinCustomer.currentCart.itemsPurchased[i].purchaseBook.name + "\" BY " +
            //        BookShopControl.LoggedinCustomer.currentCart.itemsPurchased[i].purchaseBook.author + " " + BookShopControl.LoggedinCustomer.currentCart.itemsPurchased[i].numberPurchased + "   $"
            //        + BookShopControl.LoggedinCustomer.currentCart.itemsPurchased[i].subTransactionPrice);
            //}
            //cartDialog.AddDisplayItems("=======================================================");
            //cartDialog.AddDisplayItems("Total Price : " + BookShopControl.LoggedinCustomer.currentCart.totalPrice);

            if (!isCustomerLoggedIn)
            {
                throw new BookShopException("You are not logged in.");
            }
            if (LoggedinCustomer.currentCart.subTransactionCount <= 0)
            {
                throw new BookShopException("There are no items in your cart.");
            }
            for (int i = 0; i < LoggedinCustomer.currentCart.subTransactionCount; i++)
            {
                cart.AddDisplayItems("\"" + LoggedinCustomer.currentCart.itemsPurchased[i].purchaseBook.name + "\" BY " +
                   LoggedinCustomer.currentCart.itemsPurchased[i].purchaseBook.author + " " + LoggedinCustomer.currentCart.itemsPurchased[i].numberPurchased + "   $"
                    + LoggedinCustomer.currentCart.itemsPurchased[i].subTransactionPrice);
            }
            cart.AddDisplayItems("=======================================================");
            cart.AddDisplayItems("Total Price : " + LoggedinCustomer.currentCart.totalPrice);
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace edu.ksu.cis.masaaki
{
    public class ControlBookShop
    {
        public List<Customer> listOfCustomers;
        public List<Book> listOfBooks;
        public List<Transaction> listOfPendingTransactions;
        public List<Transaction> listOfCompleteTransactions;

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

        public int amountOfPendingTransactions
        {
            get { return listOfPendingTransactions.Count; }
        }

        public int amountOfCompleteTransactions
        {
            get { return listOfCompleteTransactions.Count; }
        }

        public void editBookStaffView(BookDialog dia, ListBooksDialog list)
        {
            dia.BookTitle = listOfBooks[list.SelectedIndex].name;
            dia.Author = listOfBooks[list.SelectedIndex].author;
            dia.Publisher = listOfBooks[list.SelectedIndex].publisher;
            dia.ISBN = listOfBooks[list.SelectedIndex].isbn;
            dia.Price = listOfBooks[list.SelectedIndex].price;
            dia.Date = listOfBooks[list.SelectedIndex].date;
            dia.Stock = listOfBooks[list.SelectedIndex].stock;
        }

        public void updateBookInformationStaff(BookDialog bd, ListBooksDialog list)
        {
            listOfBooks[list.SelectedIndex].name = bd.BookTitle;
            listOfBooks[list.SelectedIndex].author = bd.Author;
            listOfBooks[list.SelectedIndex].publisher = bd.Publisher;
            listOfBooks[list.SelectedIndex].isbn = bd.ISBN;
            listOfBooks[list.SelectedIndex].price = bd.Price;
            listOfBooks[list.SelectedIndex].date = bd.Date;
            listOfBooks[list.SelectedIndex].stock = bd.Stock;
        }

        public void findCustomerLogin(string user, string pass)
        {
            foreach (Customer cust in this.listOfCustomers)
            {
                if (cust.userName.Equals(user) && cust.password.Equals(pass))
                {
                    LoggedinCustomer = cust;
                    return;
                    //return cust;
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

        public void addNewCustomer(string firstName, string lastName, string userName, string password, string email, string address, string phoneNumber)
        {
            findDuplicateCustomers(userName);
            listOfCustomers.Add(new Customer(firstName, lastName, userName, password, email, address, phoneNumber));
        }

        public void addNewBook(string name, string author, string publisher, string isbn, decimal price, string date,int stock)
        {
            try
            {
                if (findDuplicateBook(isbn) == null)
                    listOfBooks.Add(new Book(name, author, publisher, isbn,  price, date, stock));
                else
                {
                    throw new BookShopException("A book with that ISBN has already been registered.");
                }
            }
            catch (BookShopException bsex)
            {
                //MessageBox.Show("A book with that ISBN has already been registered.");
                MessageBox.Show(bsex.ErrorMessage);
            }
        }

        public Book findDuplicateBook(string isbn)
        {
            foreach (Book bookToFind in listOfBooks)
            {
                if (bookToFind.isbn.Equals(isbn))
                    return bookToFind;
            }
            return null;
        }

        public void printBookToBookdialog(ListBooksDialog lbd)
        {
            foreach (Book displayBook in listOfBooks)
            {
                lbd.AddDisplayItems(displayBook.ToString());
            }
        }

        public void updateWishListDialog(WishListDialog wld)
        {
            foreach (Book displayBook in LoggedinCustomer.wishList)
            {
                wld.AddDisplayItems("\"" + displayBook.name + "\" BY " + displayBook.author);
            }
        }

        public void populateCustomerDialog(CustomerDialog customerDialog)
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

        public void removeBookFromCustomerCart(string isbn)
        {
            if (isCustomerLoggedIn)
            {
                LoggedinCustomer.removeBookFromCart(findBookByISBN(isbn));
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
                LoggedinCustomer.removeBookFromWishList(findBookByISBN(isbn));
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

        public void updateBookInformationDialog(BookInformationDialog infoDialog, ListBooksDialog listDialog)
        {
            infoDialog.BookTitle = listOfBooks[listDialog.SelectedIndex].name;
            infoDialog.Author = listOfBooks[listDialog.SelectedIndex].author;
            infoDialog.Publisher = listOfBooks[listDialog.SelectedIndex].publisher;
            infoDialog.ISBN = listOfBooks[listDialog.SelectedIndex].isbn;
            infoDialog.Date = listOfBooks[listDialog.SelectedIndex].date;
            infoDialog.Price = listOfBooks[listDialog.SelectedIndex].price;
            infoDialog.Stock = listOfBooks[listDialog.SelectedIndex].stock;
        }

        public void updateWishListDialog(BookInWishListDialog bws, WishListDialog wishListDialog)
        {
            bws.BookTitle = LoggedinCustomer.wishList[wishListDialog.SelectedIndex].name;
            bws.Author = LoggedinCustomer.wishList[wishListDialog.SelectedIndex].author;
            bws.Publisher = LoggedinCustomer.wishList[wishListDialog.SelectedIndex].publisher;
            bws.ISBN = LoggedinCustomer.wishList[wishListDialog.SelectedIndex].isbn;
            bws.Date = LoggedinCustomer.wishList[wishListDialog.SelectedIndex].date;
            bws.Price = LoggedinCustomer.wishList[wishListDialog.SelectedIndex].price;
            bws.Stock = LoggedinCustomer.wishList[wishListDialog.SelectedIndex].stock;
        }

        public void updateCustomerDialog(CustomerDialog cd, ListCustomersDialog lcd)
        {
            cd.FirstName = listOfCustomers[lcd.SelectedIndex].firstName;
            cd.LastName = listOfCustomers[lcd.SelectedIndex].lastName;
            cd.UserName = listOfCustomers[lcd.SelectedIndex].userName;
            cd.Password = listOfCustomers[lcd.SelectedIndex].password;
            cd.EMailAddress = listOfCustomers[lcd.SelectedIndex].email;
            cd.Address = listOfCustomers[lcd.SelectedIndex].address;
            cd.TelephoneNumber = listOfCustomers[lcd.SelectedIndex].phoneNumber;
        }

        public void showCartInformation(CartDialog cart)
        {
            if (!isCustomerLoggedIn)
            {
                throw new NullReferenceException("You are not logged in.");
            }
            if (LoggedinCustomer.currentCart.subTransactionCount <= 0)
            {
                throw new NullReferenceException("There are no items in your cart.");
            }
            for (int i = 0; i < LoggedinCustomer.currentCart.subTransactionCount; i++)
            {
                cart.AddDisplayItems(LoggedinCustomer.currentCart.itemsPurchased[i].ToString());               
            }
            cart.AddDisplayItems("=======================================================");
            cart.AddDisplayItems("Total Price : $" + LoggedinCustomer.currentCart.totalPrice);
        }

        public void showPastCartInformation(ShowTransactionDialog cart, Transaction tran)
        {
            if (!isCustomerLoggedIn)
            {
                throw new NullReferenceException("You are not logged in.");
            }

            for (int i = 0; i < tran.subTransactionCount; i++)
            {
                cart.AddDisplayItems(tran.itemsPurchased[i].ToString());
            }
            cart.AddDisplayItems("=======================================================");
            cart.AddDisplayItems("Total Price : $" + tran.totalPrice);
        }

        public void listTransactionDetails(SelectDialog cart, Transaction tran)
        {

            for (int i = 0; i < tran.subTransactionCount; i++)
            {
                cart.AddDisplayItems("\"" + tran.itemsPurchased[i].purchaseBook.name + "\" BY " +
                   tran.itemsPurchased[i].purchaseBook.author + " " + tran.itemsPurchased[i].numberPurchased + "   $"
                    + tran.itemsPurchased[i].subTransactionPrice);
            }
            cart.AddDisplayItems("=======================================================");
            cart.AddDisplayItems("Total Price : $" + tran.totalPrice);
        }


        public void checkOutCustomer()
        {
            listOfPendingTransactions.Add(LoggedinCustomer.currentCart);
            LoggedinCustomer.pastTransactions.Add(LoggedinCustomer.currentCart);
            LoggedinCustomer.currentCart = new Transaction(LoggedinCustomer);
        }

        public void approveTransaction(Transaction transactionToBeApproved)
        {
            listOfCompleteTransactions.Add(transactionToBeApproved);
            listOfPendingTransactions.Remove(transactionToBeApproved);
        }

        public void removeTransactionFromPending(Transaction transactionTobeRemoved)
        {
            listOfPendingTransactions.Remove(transactionTobeRemoved);
            transactionTobeRemoved.customerName.pastTransactions.Remove(transactionTobeRemoved);
        }

        public void removeTransactionFromComplete(Transaction transactionTobeRemoved)
        {
            listOfCompleteTransactions.Remove(transactionTobeRemoved);
            transactionTobeRemoved.customerName.pastTransactions.Remove(transactionTobeRemoved);
        }

        public void serializeData(string fileName)
        {
            using (Stream fs = File.Open(fileName, FileMode.Create))
            {
                BinaryFormatter fo = new BinaryFormatter();
                //Tuple<List<Customer>, List<Book>, List<Transaction>, List<Transaction>> tupleToSerialize = 
                //    new Tuple<List<Customer>, List<Book>, List<Transaction>, List<Transaction>>(listOfCustomers, listOfBooks, listOfPendingTransactions, listOfCompleteTransactions);
                fo.Serialize(fs, listOfCustomers);
                fo.Serialize(fs, listOfBooks);

                fo.Serialize(fs, listOfPendingTransactions);
                fo.Serialize(fs, listOfCompleteTransactions);
            }
        }

        public void deSerializeData(string fileName)
        {
            using (Stream fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Read))
            {
                BinaryFormatter fo = new BinaryFormatter();
                //Tuple<List<Customer>, List<Book>, List<Transaction>, List<Transaction>> tupleToDeserialize =
                //    (Tuple<List<Customer>, List<Book>, List<Transaction>, List<Transaction>>) fo.Deserialize(fs);

                listOfCustomers = (List<Customer>)fo.Deserialize(fs);
                listOfBooks = (List<Book>)fo.Deserialize(fs);

                listOfPendingTransactions = (List<Transaction>)fo.Deserialize(fs);
                listOfCompleteTransactions = (List<Transaction>)fo.Deserialize(fs);
            }
        }

    }
}

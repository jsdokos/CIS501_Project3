using System;
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

        public void addBookToCart()
        {
            
        }
    }
}

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

        public ControlBookShop()
        {
            listOfCustomers = new List<Customer>();
            listOfBooks = new List<Book>();
            listOfPendingTransactions = new List<Transaction>();
            listOfCompleteTransactions = new List<Transaction>();
        }

        public ControlBookShop(List<Customer> listOfCustomers, List<Book> listOfBooks, List<Transaction> listOfPendingTransactions, List<Transaction> listOfCompleteTransactions)
        {
            this.listOfCustomers = listOfCustomers;
            this.listOfBooks = listOfBooks;
            this.listOfPendingTransactions = listOfPendingTransactions;
            this.listOfCompleteTransactions = listOfCompleteTransactions;

    }       
    }
}

using System;
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

        public Transaction(Customer cust, Book bookToAdd)
        {
            customerName = cust;
            addNewSubTransaction(bookToAdd, 1);
        }

        public Transaction(Customer cust)
        {
            customerName = cust;
            totalPrice = 0;
            itemsPurchased = new List<SubTransaction>();
        }

        public void addBook(Book bookToAdd)
        {
            itemsPurchased.Add(new SubTransaction(bookToAdd, 1));
        }

        public void addNewSubTransaction(Book bookToAdd, int numberToAdd)
        {
            foreach (SubTransaction sub in itemsPurchased)
            {
                if (sub.purchaseBook.isbn == bookToAdd.isbn)
                {
                    sub.numberPurchased++;
                    totalPrice += sub.purchaseBook.price;
                    return;
                }
            }

            itemsPurchased.Add(new SubTransaction(bookToAdd, numberToAdd));
            totalPrice += bookToAdd.price*numberToAdd;
        }

    }
}

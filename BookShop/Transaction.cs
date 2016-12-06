using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    [Serializable]
    public class Transaction
    {
        public List<SubTransaction> itemsPurchased;
        public Customer customerName;
        public decimal totalPrice;

        public Transaction(Customer cust)
        {
            customerName = cust;
            totalPrice = 0;
            itemsPurchased = new List<SubTransaction>();
        }

        public int subTransactionCount
        {
            get { return itemsPurchased.Count; }
        }


        public void addNewSubTransaction(Book bookToAdd, int numberToAdd)
        {
            if (bookToAdd.stock <= 0)
            {
                throw new BookShopException("There are no more books left. Sorry!");
            }
            foreach (SubTransaction sub in itemsPurchased)
            {
                if (sub.purchaseBook.isbn == bookToAdd.isbn)
                {
                    sub.numberPurchased++;
                    bookToAdd.stock--;
                    totalPrice += sub.purchaseBook.price;
                    return;
                }
            }

            itemsPurchased.Add(new SubTransaction(bookToAdd, numberToAdd));
            bookToAdd.stock--;
            totalPrice += bookToAdd.price*numberToAdd;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SubTransaction sub in itemsPurchased)
            {
                sb.Append(sub.purchaseBook.name + "(" + sub.numberPurchased + ") ");
            }
            return sb.ToString();
        }

        public void removeSubTransaction(Book bookToRemove, int numberToRemove)
        {
            SubTransaction temp = null;
            foreach (SubTransaction sub in itemsPurchased)
            {
                if (sub.purchaseBook.isbn == bookToRemove.isbn)
                {
                    if (sub.numberPurchased == 1)
                    {
                        temp = sub;
                        sub.purchaseBook.stock += numberToRemove;
                        totalPrice -= sub.purchaseBook.price;
                        itemsPurchased.Remove(temp);
                        return;
                    }
                    else
                    {
                        sub.numberPurchased--;
                        totalPrice -= sub.purchaseBook.price;
                        return;
                    }
                }
            }
        }

        public void removeSubTransaction(string isbn, int numberToRemove)
        {
            SubTransaction temp = null;
            foreach (SubTransaction sub in itemsPurchased)
            {
                if (sub.purchaseBook.isbn == isbn)
                {
                    if (sub.numberPurchased == 1)
                    {
                        temp = sub;
                        sub.purchaseBook.stock += numberToRemove;
                        totalPrice -= sub.purchaseBook.price;
                        itemsPurchased.Remove(temp);
                        return;
                    }
                    else
                    {
                        sub.numberPurchased--;
                        totalPrice -= sub.purchaseBook.price;
                        return;
                    }
                }
            }
        }
    }
}

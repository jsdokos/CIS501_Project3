using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace edu.ksu.cis.masaaki
{
    public class SubTransaction
    {
        public Book purchaseBook;
        public int numberPurchased;

        public SubTransaction(Book purchaseBook, int numberPurchased)
        {
            this.purchaseBook = purchaseBook;
            this.numberPurchased = numberPurchased;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            SubTransaction sub = (SubTransaction) obj;

            if (this.purchaseBook.isbn.Equals(sub.purchaseBook.isbn))
            {
                return true;
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    public class SubTransaction
    {
        private Book purchaseBook;
        private int numberPurchased;

        public SubTransaction(Book purchaseBook, int numberPurchased)
        {
            this.purchaseBook = purchaseBook;
            this.numberPurchased = numberPurchased;
        }
    }
}

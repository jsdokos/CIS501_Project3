using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace edu.ksu.cis.masaaki
{
    [Serializable]
    public class SubTransaction
    {
        public Book purchaseBook;
        public int numberPurchased;

        public SubTransaction(Book purchaseBook, int numberPurchased)
        {
            this.purchaseBook = purchaseBook;
            this.numberPurchased = numberPurchased;
        }

        public decimal subTransactionPrice
        {
            get { return purchaseBook.price*numberPurchased; }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("\"" + purchaseBook.name + "\" BY " + purchaseBook.author + " " + numberPurchased + "   $ " + subTransactionPrice);

            return sb.ToString();
        }
    }
}

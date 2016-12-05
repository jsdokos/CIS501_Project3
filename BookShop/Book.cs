using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    [Serializable]
    public class Book
    {
        public string name;
        public string author;
        public string publisher;
        public string isbn;
        public decimal price;
        public string date;
        public int stock;

        public Book(string name, string author, string publisher, string isbn, decimal price, string date, int stock)
        {
           this.name = name;
            this.author = author;
            this.publisher = publisher;
            this.isbn = isbn;
            this.price = price;
            this.date = date;
            this.stock = stock;
        }

        public override string ToString()
        {
            StringBuilder st = new StringBuilder();
            st.Append(name + " ");
            st.Append(author + " ");
            st.Append(publisher + " ");
            st.Append(isbn + " ");
            st.Append(price + " ");
            st.Append(stock + " ");

            return st.ToString();
        }
    }
}

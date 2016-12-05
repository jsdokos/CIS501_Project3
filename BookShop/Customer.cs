using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace edu.ksu.cis.masaaki
{
    [Serializable]
    public class Customer
    {
        public string firstName;
        public string lastName;
        public string userName;
        public string password;
        public string email;
        public string address;
        public string phoneNumber;

        public List<Transaction> pastTransactions;
        public Transaction currentCart;

        public List<Book> wishList;

        public Customer(string firstName, string lastName, string userName, string password, string email,
            string address, string phoneNumber)
        {

            this.firstName = firstName;
            this.lastName = lastName;
            this.userName = userName;
            this.password = password;
            this.email = email;
            this.address = address;
            this.phoneNumber = phoneNumber;

            currentCart = new Transaction(this);
            pastTransactions = new List<Transaction>();
            wishList = new List<Book>();
        }

        public void addBookToCart(Book bookToAdd)
        {
            currentCart.addNewSubTransaction(bookToAdd, 1);
        }

        public void removeBookFromCart(Book bookToRemove)
        {
            //currentCart.addNewSubTransaction(bookToRemove, 1);
            currentCart.removeSubTransaction(bookToRemove, 1);
        }

        public void addBookToWishList(Book bookToAdd)
        {
            //check to see if it is already on the wishlist
            foreach (Book bookOnwishList in wishList)
            {
                if (bookOnwishList.isbn == bookToAdd.isbn)
                {
                    throw new BookShopException("Book is already on wishlist.");
                }
            }
            wishList.Add(bookToAdd);
        }

        public void removeBookFromWishList(Book bookToRemove)
        {
            wishList.Remove(bookToRemove);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(firstName + " " + lastName + " " + userName+ " " + email + " " + address + " " + phoneNumber);

            return sb.ToString();
        }
    }   

}

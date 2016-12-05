using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace edu.ksu.cis.masaaki
{
    public partial class CustomerWindow : Form
    {
        // XXX add more fields if necessary

        CustomerDialog customerDialog;
        LoginDialog loginDialog;
        ListBooksDialog listBooksDialog;
        BookInformationDialog bookInformationDialog;
        CartDialog cartDialog;
        WishListDialog wishListDialog;
        BookInWishListDialog bookInWishListDialog;
        ListTransactionHistoryDialog listTransactionHistoryDialog;
        ShowTransactionDialog showTransactionDialog;
        ControlBookShop BookShopControl;

        public CustomerWindow()
        {
            InitializeComponent();
        }

        public CustomerWindow(ref ControlBookShop BookShopControl)
        {
            this.BookShopControl = BookShopControl;
            InitializeComponent();
        }

        public string updateLabel
        {
            set { lbLoggedinCustomer.Text = "Loggedin Customer : (" + value + ")"; }
        }

        // XXX You may add overriding constructors (constructors with different set of arguments).
        // If you do so, make sure to call :this()
        // public CustomerWindow(XXX xxx): this() { }
        // Without :this(), InitializeComponent() is not called
        private void CustomerWindow_Load(object sender, EventArgs e)
        {
            customerDialog = new CustomerDialog();
            loginDialog = new LoginDialog();
            listBooksDialog = new ListBooksDialog();
            bookInformationDialog = new BookInformationDialog();
            cartDialog = new CartDialog();
            wishListDialog = new WishListDialog();
            bookInWishListDialog = new BookInWishListDialog();
            listTransactionHistoryDialog = new ListTransactionHistoryDialog();
            showTransactionDialog = new ShowTransactionDialog();
        }

        private void bnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // throw exception if the customer is not found
                loginDialog.UserName = "";
                loginDialog.Password = "";
                // XXX Login Button event handler
                // First, you may want to check if anyone is logged in
                if (BookShopControl.LoggedinCustomer != null)
                    throw new BookShopException("Customer is already logged in");

                if (loginDialog.Display() == DialogReturn.Cancel) return;
                // XXX Login Button is pressed
                Customer loggedInCustomer = BookShopControl.findCustomerLogin(loginDialog.UserName, loginDialog.Password);

                if (loggedInCustomer == null)
                {
                    throw new BookShopException("Customer does not exist.");
                }
                else
                {
                    updateLabel = loggedInCustomer.userName;
                    MessageBox.Show("Login Succeeded.");
                }

            }
            catch (BookShopException bsex)
            {
                MessageBox.Show(this, bsex.ErrorMessage);
            }
        }

        private void bnAddCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                // throw exception if the customer id is already registered

                //foreach (Customer cust in BookShopControl.listOfCustomers)
                //{
                //    if (customerDialog.UserName.Equals(cust.userName))
                //    {
                //        throw new BookShopException("User has already been registered.");
                //    }
                //}
                //// XXX Register Button event handler
                customerDialog.ClearDisplayItems();
                //if (customerDialog.Display() == DialogReturn.Cancel) return;
                //// XXX pick up information from customerDialog by calling its properties
                //// and register a new customer
                //if (customerDialog.UserName != "")
                //    BookShopControl.listOfCustomers.Add(new Customer(customerDialog.FirstName, customerDialog.LastName,
                //        customerDialog.UserName,
                //        customerDialog.Password, customerDialog.EMailAddress, customerDialog.Address,
                //        customerDialog.TelephoneNumber));

                BookShopControl.findDuplicateCustomers(customerDialog.UserName);
                switch (customerDialog.Display())
                {
                    case DialogReturn.Cancel:
                        return;
                    case DialogReturn.Done:
                        BookShopControl.addNewCustomer(customerDialog.FirstName, customerDialog.LastName, customerDialog.UserName, 
                            customerDialog.Password, customerDialog.EMailAddress, customerDialog.Address, customerDialog.TelephoneNumber);
                        break;
                    default:
                        return;
                }

            }
            catch (BookShopException bsex)
            {
                MessageBox.Show(this, bsex.ErrorMessage);
            }
        }

        private void bnEditSelfInfo_Click(object sender, EventArgs e)
        {
            // XXX Edit Self Info button event handler
            BookShopControl.editCustomerInformation(ref customerDialog); //TODO name confusing need to change
            //if (customerDialog.Display() == DialogReturn.Cancel) return;
            // XXX Done button is pressed
            //if (customerDialog.Display() == DialogReturn.Done)
            //    BookShopControl.addEditedCustomer(ref customerDialog);
            try
            {
                if (BookShopControl.LoggedinCustomer != null)
                {
                    switch (customerDialog.Display())
                    {
                        case DialogReturn.Cancel:
                            return;
                        case DialogReturn.Done:
                            BookShopControl.editCurrentCustomer(customerDialog.FirstName, customerDialog.LastName, 
                                customerDialog.UserName, customerDialog.Password, customerDialog.EMailAddress, customerDialog.Address, customerDialog.TelephoneNumber);
                            break;
                        default:
                            return;
                    }
                }
                else
                {
                    throw new BookShopException("There is no user logged in.");
                }
            }
            catch (BookShopException bsex)
            {
                MessageBox.Show(this, bsex.ErrorMessage);
            }
        }
            

        private void bnBook_Click(object sender, EventArgs e)
        {
            // XXX List Books buton is pressed

            while (true)
            {
                try
                {
                    // to capture an exception from SelectedItem/SelectedIndex of listBooksDialog
                    //TODO add in error checking for if they don't select the apprioate button
                    listBooksDialog.ClearDisplayItems();
                    // XXX null is a dummy argument

                    foreach (Book displayBook in BookShopControl.listOfBooks)
                    {
                        listBooksDialog.AddDisplayItems(displayBook.ToString());
                    }

                    //listBooksDialog.AddDisplayItems(BookShopControl.listOfBooks.ToArray().ToString());

                    if (listBooksDialog.Display() == DialogReturn.Done) return;
                    // select is pressed
                    BookShopControl.updateBookInformationDialog(ref bookInformationDialog, ref listBooksDialog);

                    switch (bookInformationDialog.Display())
                    {
                        case DialogReturn.AddToCart: // Add to Cart
                            // XXX
                            BookShopControl.addBookToCustomerCart(BookShopControl.listOfBooks[listBooksDialog.SelectedIndex].isbn);
                            continue;

                        case DialogReturn.AddToWishList: // Add to Wishlist
                            // XXX
                            BookShopControl.addBookToCustomerWishList(BookShopControl.listOfBooks[listBooksDialog.SelectedIndex].isbn);
                            continue;

                        case DialogReturn.Done: // cancel
                            continue;
                        default:
                            return;
                    }
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnShowWishlist_Click(object sender, EventArgs e)
        {
            // XXX Show WishList Button event handler

            while (true)
            {
                try
                {
                    // to capture an excepton by SelectedItem/SelectedIndex of wishListDialog
                    wishListDialog.ClearDisplayItems();
                    //wishListDialog.AddDisplayItems(null); // XXX null is a dummy argument

                    if (!BookShopControl.isCustomerLoggedIn)
                    {
                        throw new BookShopException("You are not logged in.");
                    }
                    if (BookShopControl.LoggedinCustomer.wishList.Count <= 0)
                    {
                        throw new BookShopException("There are no items on your wishlist.");
                    }
                    foreach (Book displayBook in BookShopControl.LoggedinCustomer.wishList)
                    {
                        wishListDialog.AddDisplayItems("\"" + displayBook.name + "\" BY " + displayBook.author);
                    }

                    if (wishListDialog.Display() == DialogReturn.Done) return;
                    // select is pressed
                    //XXX 
                    //BookShopControl.updateBookInformationDialog(ref bookInformationDialog, ref listBooksDialog);
                    //TODO move into it's own method
                    bookInWishListDialog.BookTitle = BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex].name;
                    bookInWishListDialog.Author = BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex].author;
                    bookInWishListDialog.Publisher = BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex].publisher;
                    bookInWishListDialog.ISBN = BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex].isbn;
                    bookInWishListDialog.Date = BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex].date;
                    bookInWishListDialog.Price = BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex].price;
                    bookInWishListDialog.Stock = BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex].stock;

                    switch (bookInWishListDialog.Display())
                    {
                        case DialogReturn.AddToCart:
                            // XXX 
                            BookShopControl.addBookToCustomerCart(BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex].isbn);
                            Book temp = BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex];
                            bookInWishListDialog.Stock = temp.stock;
                            continue;
                        case DialogReturn.Remove:
                            // XXX
                            BookShopControl.removeBookFromCustomerWishList(BookShopControl.LoggedinCustomer.wishList[wishListDialog.SelectedIndex].isbn);
                            continue;
                        case DialogReturn.Done: // Done
                            continue;
                    }
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    return;
                }
                catch (NullReferenceException nex)
                {
                    //throw new BookShopException("There are no items on your wishlist.");
                    return;
                }
            }
        }

        private void bnShowCart_Click(object sender, EventArgs e)
        {
            // XXX Show Cart Button event handler
            while (true)
            {
                try
                {
                    // to capture an exception from SelectedIndex/SelectedItem of carDisplay
                    cartDialog.ClearDisplayItems();
                    //cartDialog.AddDisplayItems(null); // null is a dummy argument

                    //HERE
                    BookShopControl.showCartInformation(ref cartDialog);
                    switch (cartDialog.Display())
                    {
                        case DialogReturn.CheckOut: // check out
                            // XXX
                            BookShopControl.checkOutCustomer();
                            return;
                        case DialogReturn.ReturnBook: // remove a book
                            // XXX
                            //BookShopControl.removeBookFromCustomerCart(); TODO implement this
                            BookShopControl.removeBookFromCustomerCart(BookShopControl.LoggedinCustomer.currentCart.itemsPurchased[cartDialog.SelectedIndex].purchaseBook.isbn);
                            continue;

                        case DialogReturn.Done: // cancel
                            return;
                    }
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
                catch (NullReferenceException nex)
                {
                    //throw new BookShopException("There are no items on your wishlist.");
                    MessageBox.Show(this, nex.Message);
                    return;
                }
            }
        }

        private void bnTransactionHistory_Click(object sender, EventArgs e)
        {
            // XXX Transaction History button handler
            while (true)
            {
                if (BookShopControl.LoggedinCustomer.pastTransactions.Count <= 0)
                {
                    throw new NullReferenceException("There are no past transactions.");
                }
                try
                {
                    // to capture an exception from SelectedIndex/SelectedItem of listTransactionHistoryDialog
                    listTransactionHistoryDialog.ClearDisplayItems();
                    //listTransactionHistoryDialog.AddDisplayItems(null); // null is a dummy argument

                    foreach (Transaction tran in BookShopControl.LoggedinCustomer.pastTransactions)
                    {
                        listTransactionHistoryDialog.AddDisplayItems(tran.ToString());
                    }
                    if (listTransactionHistoryDialog.Display() == DialogReturn.Done) return;
                    // Select is pressed


                    showTransactionDialog.ClearDisplayItems();
                    //showTransactionDialog.AddDisplayItems(null); // null is a dummy argument
                    BookShopControl.showPastCartInformation(ref showTransactionDialog, BookShopControl.LoggedinCustomer.pastTransactions[listTransactionHistoryDialog.SelectedIndex]);
                    showTransactionDialog.ShowDialog();
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                }
                catch (NullReferenceException nex)
                {
                    MessageBox.Show(this, nex.Message);
                    return;
                }
            }
        }

        private void bnLogout_Click(object sender, EventArgs e)
        {
            // XXX Logout  button event handler
            try
            {
                BookShopControl.logoutCustomer();
                updateLabel = "none";
            }
            catch (BookShopException ex)
            {
                MessageBox.Show(this, ex.ErrorMessage);
                throw;
            }
        }

    }
}

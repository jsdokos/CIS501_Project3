using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace edu.ksu.cis.masaaki
{
    public partial class StaffWindow : Form
    {
        // XXX add more fields if necessary

        ListCustomersDialog listCustomersDialog;
        CustomerDialog customerDialog;
        ListBooksDialog listBooksDialog;
        BookDialog bookDialog;
        ListCompleteTransactionsDialog listCompleteTransactionsDialog;
        ShowCompleteTransactionDialog showCompleteTransactionDialog;
        ListPendingTransactionsDialog listPendingTransactionsDialog;
        ShowPendingTransactionDialog showPendingTransactionDialog;
        ControlBookShop BookShopControl;

        public StaffWindow()
        {
            InitializeComponent();
        }

        public StaffWindow(ref ControlBookShop BookShopControl)
        {
            this.BookShopControl = BookShopControl;
            InitializeComponent();
        }

        // XXX You may add overriding constructors (constructors with different set of arguments).
        // If you do so, make sure to call :this()
        // public StaffWindow(XXX xxx): this() { }
        // Without :this(), InitializeComponent() is not called
        private void StaffWindow_Load(object sender, EventArgs e)
        {
            listCustomersDialog = new ListCustomersDialog();
            customerDialog = new CustomerDialog();
            listBooksDialog = new ListBooksDialog();
            bookDialog = new BookDialog();
            listCompleteTransactionsDialog = new ListCompleteTransactionsDialog();
            showCompleteTransactionDialog = new ShowCompleteTransactionDialog();
            listPendingTransactionsDialog = new ListPendingTransactionsDialog();
            showPendingTransactionDialog = new ShowPendingTransactionDialog();
        }

        private void bnListCustomers_Click(object sender, EventArgs e)
        {
            // XXX List Customers button event handler
            
            while (true)
            {

                try
                { // to capture an exception from SelectedIndex/SelectedItem of listCustomersDialog
                    listCustomersDialog.ClearDisplayItems();
                    //listCustomersDialog.AddDisplayItems(null); // null is a dummy argument

                    foreach (Customer cust in BookShopControl.listOfCustomers) //print all the customers
                    {
                        listCustomersDialog.AddDisplayItems(cust.ToString());
                    }
                    if (listCustomersDialog.Display() == DialogReturn.Done) return;
                    // select button is pressed
                    BookShopControl.updateCustomerDialog(customerDialog, listCustomersDialog);

                    if (customerDialog.Display() == DialogReturn.Cancel) continue;
                    // XXX Edit Done button is pressed
                    BookShopControl.editCurrentCustomer(customerDialog.FirstName, customerDialog.LastName,
                                customerDialog.UserName, customerDialog.Password, customerDialog.EMailAddress, customerDialog.Address, customerDialog.TelephoneNumber);
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnAddBook_Click(object sender, EventArgs e)
        {
            // XXX Add Book button event handler
            while (true)
            {
                try
                { // to capture an exception from Price/Stock of bookDialog
                    // also throw an exception if the ISBN is already registered
                    bookDialog.ClearDisplayItems();
                    if (bookDialog.ShowDialog() == DialogResult.Cancel) return;
                    // Edit Done button is pressed
                    BookShopControl.addNewBook(bookDialog.BookTitle, bookDialog.Author, bookDialog.Publisher, bookDialog.ISBN, bookDialog.Price, bookDialog.Date, bookDialog.Stock);
                    return;
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnListBooks_Click(object sender, EventArgs e)
        {
            // XXX List Books button event handler
           
            while (true)
            {            
                try
                {   // to capture an exception from SelectedItem/SelectedIndex of listBooksDialog
                    listBooksDialog.ClearDisplayItems();
                    //listBooksDialog.AddDisplayItems(null); //null is a dummy argument

                    BookShopControl.printBookToBookdialog(listBooksDialog);

                    if (listBooksDialog.Display() == DialogReturn.Done) return;
                    // select is pressed
                    BookShopControl.editBookStaffView(ref bookDialog, ref listBooksDialog);

                    while (true)
                    {
                        try
                        { // to capture an exception from Price/Stock of bookDialog
                            if (bookDialog.Display() == DialogReturn.Cancel) break;
                            // XXX
                            BookShopControl.updateBookInformationStaff(ref bookDialog, ref listBooksDialog);
                            break;
                        }
                        catch (BookShopException bsex)
                        {
                            MessageBox.Show(this, bsex.ErrorMessage);
                            continue;
                        }
                    }
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }
        private void bnPendingTransactions_Click(object sender, EventArgs e)
        {
            // XXX List Pending Transactions button event handl

            while (true)
            {
               
                try
                {  // to capture an exception from SelectedIndex/SelectedItem of listPendingTransactionsDialog
                    listPendingTransactionsDialog.ClearDisplayItems();
                    //listPendingTransactionsDialog.AddDisplayItems(null);  // null is a dummy argument
                    foreach (Transaction tran in BookShopControl.listOfPendingTransactions)
                    {
                        //TODO move into new method
                        listPendingTransactionsDialog.AddDisplayItems(tran.customerName.userName + " : " + tran.ToString());
                    }
                    if (listPendingTransactionsDialog.Display() == DialogReturn.Done) return;
                    // select button is pressed

                    while (true)
                    {
                        try
                        {  // to capture an exception from SelectedItem/SelectedTransaction of showPendingTransactionDialog
                            showPendingTransactionDialog.ClearDisplayItems();
                            //showPendingTransactionDialog.AddDisplayItems(null); // null is a dummy argument

                            BookShopControl.listTransactionDetails((SelectDialog)showPendingTransactionDialog, BookShopControl.listOfPendingTransactions[listPendingTransactionsDialog.SelectedIndex]);

                            switch (showPendingTransactionDialog.Display())
                            {
                                case DialogReturn.Approve:  // Transaction Processed
                                    // XXX
                                    BookShopControl.approveTransaction(BookShopControl.listOfPendingTransactions[listPendingTransactionsDialog.SelectedIndex]);
                                    if (BookShopControl.amountOfPendingTransactions <= 0)
                                        return;
                                    break;
                                case DialogReturn.ReturnBook: // Return Book
                                    // XXX
                                    BookShopControl.listOfPendingTransactions[listPendingTransactionsDialog.SelectedIndex].
                                        removeSubTransaction(BookShopControl.listOfPendingTransactions[listPendingTransactionsDialog.SelectedIndex].itemsPurchased[showPendingTransactionDialog.SelectedIndex].purchaseBook.isbn, 1);
                                    continue;
                                case DialogReturn.Remove: // Remove transaction
                                    // XXX
                                    BookShopControl.removeTransactionFromPending(BookShopControl.listOfPendingTransactions[listPendingTransactionsDialog.SelectedIndex]);
                                    break;
                            }
                            break; //for "transaction processed"
                        }
                        catch (BookShopException bsex)
                        {
                            MessageBox.Show(this, bsex.ErrorMessage);
                            return;
                        }
                    }
                }
                catch (BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnCompleteTransactions_Click(object sender, EventArgs e)
        {
            // XXX List Complete Transactions button event handler
            
            while (true)
            {           
                try
                { // to capture an exception from SelectedItem/SelectedIndex of listCompleteTransactionsDialog
                    listCompleteTransactionsDialog.ClearDisplayItems();
                    //listCompleteTransactionsDialog.AddDisplayItems(null); // XXX null is a dummy argument

                    foreach (Transaction tran in BookShopControl.listOfCompleteTransactions)
                    {
                        listCompleteTransactionsDialog.AddDisplayItems(tran.customerName.userName + " : " + tran.ToString());
                    }
                    if (listCompleteTransactionsDialog.Display() == DialogReturn.Done) return;
                    // select button is pressed
                    
                    showCompleteTransactionDialog.ClearDisplayItems();
                    //showCompleteTransactionDialog.AddDisplayItems(null); // XXX null is a dummy argument

                    //moved it into it's own method but casted it to it's base class so i can use it for pending and complete
                    BookShopControl.listTransactionDetails((SelectDialog) showCompleteTransactionDialog, BookShopControl.listOfCompleteTransactions[listCompleteTransactionsDialog.SelectedIndex]);

                    switch (showCompleteTransactionDialog.Display())
                    {
                        case DialogReturn.Remove: // transaction Remove
                            // XXX
                            BookShopControl.removeTransactionFromComplete(null);
                            continue;
                        case DialogReturn.Done:
                            continue;
                    }
                }
                catch(BookShopException bsex)
                {
                    MessageBox.Show(this, bsex.ErrorMessage);
                    continue;
                }
            }
        }

        private void bnSave_Click(object sender, EventArgs e)
        {
            // XXX Save button handler
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "VRS Files|*.vrs";
                saveFileDialog.AddExtension = true;
                saveFileDialog.InitialDirectory = Application.StartupPath;
                if (saveFileDialog.ShowDialog() != DialogResult.OK) return;
                // XXX
                BookShopControl.serializeData(saveFileDialog.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Serialization Failed");
            }
        }

        private void bnRestore_Click(object sender, EventArgs e)
        {
            // XXX Restore button handler
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "VRS Files|*.vrs";
                openFileDialog.InitialDirectory = Application.StartupPath;
                if (openFileDialog.ShowDialog() != DialogResult.OK) return;
                // XXX

                BookShopControl.deSerializeData(openFileDialog.SafeFileName);
            }
 
            catch (Exception ex)
            {
                MessageBox.Show("DeSerialization Failed");
            }
        }

        private void bnDone_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

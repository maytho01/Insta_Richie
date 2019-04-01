using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Popups;
using StartFinance.Models;
using SQLite.Net;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238
//created by Malcolm Billinghurst
namespace StartFinance.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ContactDetailsPage : Page
    {
        SQLiteConnection conn; // adding an SQLite connection
        string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "Findata.sqlite");
        ContactInfo c = new ContactInfo();

        public ContactDetailsPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Enabled;
            /// Initializing a database
            conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            // Creating table
            Results();

        }

        public void Results()
        {
            // Creating table
            conn.CreateTable<ContactInfo>();
            var query = conn.Table<ContactInfo>();
            ContactList.ItemsSource = query.ToList();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // checks if account name is null
                if (FName.Text.ToString() == "")
                {
                    MessageDialog dialog = new MessageDialog("Name not Entered", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {   // Inserts the data
                    conn.Insert(new ContactInfo()
                    {
                        CustFName = FName.Text,
                        CustLName = LName.Text,
                        CompanyName = ComName.Text,
                        PhoneNum = Convert.ToDouble(CustPhone.Text)
                    });
                    Results();
                }

            }
            catch (Exception ex)
            {   // Exception to display when amount is invalid or not numbers
                if (ex is FormatException)
                {
                    MessageDialog dialog = new MessageDialog("You forgot to enter the contact details or entered an invalid data", "Oops..!");
                    await dialog.ShowAsync();
                }   // Exception handling when SQLite contraints are violated
                else if (ex is SQLiteException)
                {
                    MessageDialog dialog = new MessageDialog("Contact details already exist, Try Different Name", "Oops..!");
                    await dialog.ShowAsync();
                }
                else
                {
                    /// no idea
                }

            }
        }

        // Clears the fields
        private async void ClearFileds_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ClearDialog = new MessageDialog("Cleared", "information");
            await ClearDialog.ShowAsync();
        }

        // Displays the data when navigation between pages
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Results();
            Savebttn.Visibility = Visibility.Collapsed;
            Editbttn.Visibility = Visibility.Collapsed;
        }

        
        private void ContactListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Editbttn.Visibility = Visibility.Visible;
            c = (ContactInfo)ContactList.SelectedItem;
        }

        //Edit the Choosen Contact Details
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

            Savebttn.Visibility = Visibility.Visible;
            Addbttn.Visibility = Visibility.Collapsed;
            FName.Text = c.CustFName;
            LName.Text = c.CustLName;
            ComName.Text = c.CompanyName;
            CustPhone.Text = c.PhoneNum.ToString();
        }

        //save the updated contact details
        private async void Savebttn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string ContactsLabel = ((ContactInfo)ContactList.SelectedItem).CustFName;
                c.CustFName = FName.Text;
                c.CustLName = LName.Text;
                c.CompanyName = ComName.Text;
                c.PhoneNum = Convert.ToDouble(CustPhone.Text);
                conn.CreateTable<ContactInfo>();
                var qr = conn.Update(c);
                var query1 = conn.Table<ContactInfo>();
                ContactList.ItemsSource = query1.ToList();
                Addbttn.Visibility = Visibility.Visible;
                Deletebttn.Visibility = Visibility.Visible;
                Savebttn.Visibility = Visibility.Collapsed;
                Editbttn.Visibility = Visibility.Collapsed;

            }
            catch
            {
                MessageDialog ClearDialog = new MessageDialog("Please select the item to Delete", "Oops..!");
                await ClearDialog.ShowAsync();
            }
        }

        //delete the chosen Contact Details from the database
        private async void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            MessageDialog ShowConf = new MessageDialog("Deleting this Account will delete all transactions of this account", "Important");
            ShowConf.Commands.Add(new UICommand("Yes, Delete")
            {
                Id = 0
            });
            ShowConf.Commands.Add(new UICommand("Cancel")   
            {
                Id = 1
            });
            ShowConf.DefaultCommandIndex = 0;
            ShowConf.CancelCommandIndex = 1;

            var result = await ShowConf.ShowAsync();
            if ((int)result.Id == 0)
            {
                // checks if data is null else inserts
                try
                {
                    string ContactsLabel = ((ContactInfo)ContactList.SelectedItem).CustFName;
                    var querydel = conn.Query<ContactInfo>("DELETE FROM ContactInfo WHERE CustFName = '" + ContactsLabel + "'");
                    Results();
                }
                catch (NullReferenceException)
                {
                    MessageDialog ClearDialog = new MessageDialog("Please select the item to Delete", "Oops..!");
                    await ClearDialog.ShowAsync();
                }
            }
            else
            {
                //
            }
        }
    }
}

using System;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace LittleBB
{
    public partial class MainPage : ContentPage
    {
        private SQLiteAsyncConnection _connection;
        private ObservableCollection<Account> _accounts;

        public MainPage()
        {
            InitializeComponent();

            _connection = DependencyService.Get<ISQLiteDb>().GetConnection();
            _connection.CreateTableAsync<Account>();

        }

        protected override async void OnAppearing()
        {
           
            var accounts = await _connection.Table<Account>().ToListAsync();
            _accounts = new ObservableCollection<Account>(accounts);
        
            Update();

            base.OnAppearing();
        }

        async void OnAddAccount(object sender, EventArgs e)
        {
            
            var page = new AccountDetailPage(new Account());

            page.AccountAdded += (source, account) =>
            {
                account.Status = "created";
                _connection.InsertAsync(account);
                _accounts.Add(account);
            };

            await Navigation.PushAsync(page);

            //Clear search bar text when returning to page
            searchBar.Text = null;

        }

        async void OnAccountSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (accountsListView.SelectedItem == null)
                return;

            var selectedAccount = e.SelectedItem as Account;

            accountsListView.SelectedItem = null;

          
            var page = new AccountDetailPage(selectedAccount);

            page.AccountUpdated += (source, account) =>
            {
                var updatedAccount = new Account();

                updatedAccount.Name = account.Name;
                updatedAccount.A_Note = account.A_Note;
                updatedAccount.B_Note = account.B_Note;
                updatedAccount.C_Note = account.C_Note;
                updatedAccount.D_Note = account.D_Note;
                updatedAccount.E_Note = account.E_Note;
                updatedAccount.Status = account.Status;

                _connection.DeleteAsync(selectedAccount);
                _accounts.Remove(selectedAccount);

                _connection.InsertAsync(updatedAccount);
                _accounts.Add(updatedAccount);

                Update();
            };

            await Navigation.PushAsync(page);

            //Clear search bar text when returning to page
            searchBar.Text = null;

        }

        async void OnDelete(object sender, EventArgs e)
        {
            var account = (sender as MenuItem).CommandParameter as Account;

            if (await DisplayAlert("Warning", $"Are you sure you want to delete {account.Name}?", "Yes", "No"))
            {
                _accounts.Remove(account);
                await _connection.DeleteAsync(account);
                Update();
            }
        }

        private void Update(string searchText = null)
        {
            //Clear list before populating with new order (standard alphabetical or modified by searchbar)
            accountsListView.ItemsSource = null;

            //Keep liste updated based on text in searchbar
            if (String.IsNullOrWhiteSpace(searchText))
            {
                //accountsListView.ItemsSource = null;
                accountsListView.ItemsSource = _accounts.OrderBy(x => x.Name); 
            }
            else
            {
                accountsListView.ItemsSource = _accounts.Where(x => x.Name.ToLower().StartsWith(searchText.ToLower()));
            }

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update(e.NewTextValue);
        }
    }
}

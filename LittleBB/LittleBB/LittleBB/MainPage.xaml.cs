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
            //await _connection.CreateTableAsync<Account>();
            //_accounts.OrderBy(x => x.Name).ToList();

            var accounts = await _connection.Table<Account>().ToListAsync();
            _accounts = new ObservableCollection<Account>(accounts);
            accountsListView.ItemsSource = _accounts.OrderBy(x => x.Name);
            //accountsListView.ItemsSource = _accounts;

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
                //selectedAccount.Name = account.Name;
                //selectedAccount.A_Note = account.A_Note;
                //selectedAccount.B_Note = account.B_Note;
                //selectedAccount.C_Note = account.C_Note;
                //selectedAccount.Status = account.Status;

                //Hack
                var updatedAccount = new Account();

                updatedAccount.Name = account.Name;
                updatedAccount.A_Note = account.A_Note;
                updatedAccount.B_Note = account.B_Note;
                updatedAccount.C_Note = account.C_Note;
                updatedAccount.Status = account.Status;

                _connection.DeleteAsync(selectedAccount);
                _accounts.Remove(selectedAccount);

                _connection.InsertAsync(updatedAccount);
                _accounts.Add(updatedAccount);

                Update();
            };

            await Navigation.PushAsync(page);
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

        private void Update()
        {
            accountsListView.ItemsSource = null;
            accountsListView.ItemsSource = _accounts.OrderBy(x => x.Name);
        }
    }
}

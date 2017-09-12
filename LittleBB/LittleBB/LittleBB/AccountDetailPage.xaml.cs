using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LittleBB
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountDetailPage : ContentPage
    {
        //EventHandler delegates for udating and saving a new account
        public event EventHandler<Account> AccountAdded;
        public event EventHandler<Account> AccountUpdated;

        public AccountDetailPage(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            InitializeComponent();

            //Temp account in case user doesn't save current account info
            BindingContext = new Account
            {
                Name = account.Name,
                A_Note = account.A_Note,
                B_Note = account.B_Note,
                C_Note = account.C_Note,
                D_Note = account.D_Note,
                E_Note = account.E_Note,
                Status = account.Status
            };
        }

        async void OnSave(object sender, EventArgs e)
        {
            var account = BindingContext as Account;

            if (String.IsNullOrWhiteSpace(account.Name))
            {
                await DisplayAlert("Error", "Please enter name for entry", "OK");
                return;
            }

            if (account.Status == null)
            {
                AccountAdded?.Invoke(this, account);
            }
            else
            {
                AccountUpdated?.Invoke(this, account);
            }

            await Navigation.PopAsync();
        }
    }
}
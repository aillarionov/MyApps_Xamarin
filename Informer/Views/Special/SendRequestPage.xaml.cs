using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Informer.Services.RPC;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Informer.Views.Special
{
    public partial class SendRequestPage : ContentPage
    {
        private int groupId;

        public SendRequestPage(int groupId)
        {
            this.groupId = groupId;
            InitializeComponent();
            Title = "Заявка на участие";
        }

        void Item_TextChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            (sender as Entry).BackgroundColor = string.IsNullOrEmpty((sender as Entry).Text) ? Color.LightCoral : Color.Transparent;
        }

        void Send_Clicked(object sender, System.EventArgs e)
        {
            bool complete = true;

            if (string.IsNullOrEmpty(Name.Text)) 
            {
                Name.BackgroundColor = Color.LightCoral;
                complete = false;
            }

            if (string.IsNullOrEmpty(Email.Text))
            {
                Email.BackgroundColor = Color.LightCoral;
                complete = false;
            }

            if (string.IsNullOrEmpty(Phone.Text))
            {
                Phone.BackgroundColor = Color.LightCoral;
                complete = false;
            }

            if (string.IsNullOrEmpty(Company.Text))
            {
                Company.BackgroundColor = Color.LightCoral;
                complete = false;
            }
     
            if (complete)
            {
                bool result = false;

                try
                {
                    result = RemoteFunctions.SendRequest(this.groupId, Name.Text, Company.Text, Phone.Text, Email.Text, Text.Text, default(CancellationToken)).Result;
                } 
                catch (Exception) 
                {
                    
                }

                if (result)
                {
                    DisplayAlert("Успешно", "Заявка успешно отправлена", "OK");
                }
                else
                {
                    DisplayAlert("Ошибка", "Произошла ошибка при попытке отправить заявку. Попробуйте позже или свяжитесь с организаторами по телефону", "OK");
                }
            }

        }
    }
}


using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Informer.Views.Special
{
    public partial class MapPage : ContentPage
    {
        private Map map;

        public MapPage(int groupId)
        {
            InitializeComponent();

            this.Title = "Расположение";

            BindingContext = this;

            map = new Map(MapSpan.FromCenterAndRadius(App.mapPin, Distance.FromMiles(0.3)))
            {
                //IsShowingUser = true,
                //HeightRequest = 100,
                //WidthRequest = 960,
                VerticalOptions = LayoutOptions.FillAndExpand
            };

            map.Pins.Add(new Pin { Position = App.mapPin, Label = App.mapPinLabel });

            var stack = new StackLayout 
            { 
                Spacing = 0,
                Orientation = StackOrientation.Vertical
            };

            stack.Children.Add(map);

            Content = stack;
        }

        // >>>>>> Иначе на андроиде карта остается поверх экрана при открытии меню
        protected override void OnAppearing()
        {
            base.OnAppearing();

            map.IsVisible = true;

            ((App.mainPage) as MasterDetailPage).IsPresentedChanged += Handle_IsPresentedChanged;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            ((App.mainPage) as MasterDetailPage).IsPresentedChanged -= Handle_IsPresentedChanged;
        }

        void Handle_IsPresentedChanged(object sender, System.EventArgs e)
        {
            map.IsVisible = !((App.mainPage) as MasterDetailPage).IsPresented;
        }
        // <<<<<< Иначе на андроиде карта остается поверх экрана при открытии меню
    }


}

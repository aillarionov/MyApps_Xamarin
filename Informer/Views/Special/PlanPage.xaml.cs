using System;
using System.Collections.Generic;
using Informer.Controls;
using Informer.Models;
using Informer.Services;
using Informer.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace Informer.Views
{
    public partial class PlanPage : Xamarin.Forms.TabbedPage
    {
        public PlanPage()
        {
            InitializeComponent();
            this.Title = "План выставки";

            this.On<Xamarin.Forms.PlatformConfiguration.Android>().SetIsSwipePagingEnabled(false);

            GeneratePlans();
        }

 
        private void GeneratePlans() 
        {
            if (App.group.Plan != null) 
            {
                this.Children.Clear();

                foreach (KeyValuePair<String, String> kvp in App.group.Plan)
                {
                    ContentPage cp = new ContentPage
                    {
                        Title = kvp.Key,
                        Padding = new Thickness(0),
                        Icon = new FileImageSource { File = "plan.png" },
                        Content = new ZoomView
                        {
                            VerticalOptions = LayoutOptions.FillAndExpand,
                            IsClippedToBounds = true,
                            Content = new Xamarin.Forms.Image
                            {
                                Aspect = Xamarin.Forms.Aspect.AspectFill,
                                VerticalOptions = LayoutOptions.Start,
                                HorizontalOptions = LayoutOptions.Start,
                                Source = new UriImageSource
                                {
                                    Uri = new Uri(kvp.Value),
                                    CacheValidity = App.imageCacheTime
                                }
                            }
                        }
                    };

                    this.Children.Add(cp);
                }
            }
        }


    }
}

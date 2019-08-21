using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Informer.Controls;
using Informer.Models;
using Informer.Services;
using Informer.Services.RPC;
using Informer.Utils;
using Informer.Views;
using Newtonsoft.Json;
using Plugin.PushNotification;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Maps;

namespace Informer
{
    public partial class App : Application
    {
        private static App app;

        public static Size DisplaySettings { get; private set; }

        public static string BackendUrl = "";
        public static TimeSpan ConnectionTimeout = new TimeSpan(0, 0, 30);
        public static TimeSpan imageCacheTime = new TimeSpan(365, 0, 0, 0, 0);
        public static TimeSpan queryDelay = TimeSpan.FromMilliseconds(1000);



        public static bool IsActive { get; set; }


        // >>>>>>> Group config params
        public static Config config = null;
        public static Group group = null;
        public static Position mapPin;
        public static string mapPinLabel;
        // >>>>>>> Group config params

        public static int _maxWidth = 0;
        public static String token = null;

        public static IAD AD;


        private List<Action> runAction = new List<Action>();
        private List<String> processedMessages = new List<string>();

        private static MainMenu mainMenu;

        public event EventHandler<SchemaEventArgs> OnThemeChange;

        public static Page mainPage { get { return app.MainPage; } set { app.MainPage = value; } }

        public static int maxImageSize 
        {
            get 
            {
                if (_maxWidth == 0) 
                {
                    _maxWidth = (int)DisplaySettings.Width;
                }   

                return _maxWidth > 0 ? _maxWidth : 604;
            }
        }

        public App(IDisplaySettings _DisplaySettings, IAD ad)
        {
            DisplaySettings = new Size(_DisplaySettings.GetWidth(), _DisplaySettings.GetHeight());
            AD = ad;

            app = this;


            InitializeComponent();

            ConfigurePush();

            int groupId = LocalDataStore.LoadCurrentGroup().Result;

            if (groupId == 0) 
            {
                mainPage = new GroupsPage();
            } 
            else 
            {
                //   Попытка обновить группу
                try 
                {
                    LocalDataStore.UpdateGroup(groupId, default(CancellationToken)).Wait();
                }
                catch (Exception)
                {
                    
                }

                group = LocalDataStore.GetGroup(groupId).Result;
                ContinueWithGroup();
            }


        }

        protected override void OnStart()
        {
            App.IsActive = true;

            while (runAction.Count > 0)
            {
                Action action = runAction[0];
                action.Invoke();
                runAction.Remove(action);
            }
        }

        protected override void OnSleep()
        {
            App.IsActive = false;
        }

        protected override void OnResume()
        {
            App.IsActive = true;

            while (runAction.Count > 0)
            {
                Action action = runAction[0];
                action.Invoke();
                runAction.Remove(action);
            }
            if (config != null)
            {
                Task.Run(() =>
                {
                    try
                    {
                        DataLoader.UpdateGroup(config, new ObservableProgress(), default(CancellationToken)).Wait();
                    }
                    catch (Exception e)
                    {
                        #if DEBUG
                        Console.WriteLine(e.Message);
                        #endif
                    }
                });
            }
        }

        public static void ContinueWithGroup()
        {
            token = null;
            LoadGroupConfig();

            Task.Run(async () => await LocalDataStore.SaveCurrentGroup(group.Id).ConfigureAwait(false));

            try
            {
                config = LocalDataStore.GetConfig(group.Id).Result;
            }
            catch (Exception)
            {
                LocalDataStore.Create().Wait();
            }

            if (config == null)
            {
                mainPage = new InitialPage(group.Id);
            }
            else
            {
                try
                {
                    SendUpdatedToken();
                    SendAdId();

                    DataLoader.UpdateGroup(config, new ObservableProgress(), default(CancellationToken)).Wait();
                }
                catch (Exception e)
                {
                    #if DEBUG
                    Console.WriteLine(e.Message);
                    #endif
                }

                mainPage = new MainMenu();
            } 
        }

        public static void SetPage(Page page)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                mainPage = page;
            });
        }


        public static void SetMainPage()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                mainPage = mainMenu == null ? mainMenu = new MainMenu() : mainMenu;
            });

            SendUpdatedToken();
        }

        public static void SendUpdatedToken() 
        {
            if (!String.IsNullOrEmpty(CrossPushNotification.Current.Token))
            {
                UpdateToken(CrossPushNotification.Current.Token);
            }
        }

        private static void SendAdId() 
        {
            Task.Run(async () =>
            {
                try
                {
                    List<Config> configs = await LocalDataStore.GetConfigs().ConfigureAwait(false);
                    await RemoteFunctions.SendAd(configs, AD.GetAdId(), default(CancellationToken)).ConfigureAwait(false);
                }
                catch (Exception e)
                {
                    
                }
            });
        }

        private static void UpdateToken(String _token) 
        {
            #if !DEBUG
            if (token != _token)
            {
                Task.Run(async () =>
                {
                    try
                    {
                        List<Config> configs = await LocalDataStore.GetConfigs().ConfigureAwait(false);
                        await RemoteFunctions.SendPushToken(configs, _token, default(CancellationToken)).ConfigureAwait(false);
                        token = _token;
                    }
                    catch (Exception)
                    {

                    }
                });
            }
            #endif
        }

       

        public void OpenNews(int groupId, int newsId) 
        {
            try
            {
                News news = LocalDataStore.GetNews(groupId, newsId).Result;

                if (news != null && mainPage is MasterDetailPage)
                {
                    var detail = (mainPage as MasterDetailPage).Detail;
                    if (detail is NavigationPage)
                    {
                        Task.Run(async () =>
                        {
                            if (groupId != group.Id)
                            {
                                try
                                {
                                    group = await LocalDataStore.GetGroup(groupId).ConfigureAwait(false);
                                    Device.BeginInvokeOnMainThread(async () =>
                                    {
                                        ContinueWithGroup();
                                        var ndetail = (mainPage as MasterDetailPage).Detail;
                                        await ItemDetailOpener.Open((ndetail as NavigationPage).Navigation, news).ConfigureAwait(false);
                                    });
                                } 
                                catch (Exception)
                                {
                                    
                                }
                            }
                            else 
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await ItemDetailOpener.Open((detail as NavigationPage).Navigation, news).ConfigureAwait(false);
                                });

                            }
                        });
                    }
                }
            }
            catch (Exception e) 
            {
                System.Diagnostics.Debug.WriteLine("Opened news exception: " + e.Message);
            }
        }
       

        #region PUSH

        private void ConfigurePush()
        {
            CrossPushNotification.Current.OnTokenRefresh += (s, p) =>
            {
                UpdateToken(p.Token);
            };


            CrossPushNotification.Current.OnNotificationReceived += (s, p) =>
            {
                if (App.IsActive) 
                {
                    Action action = ParsePush(p.Data);
                    if (action != null)
                    {
                        action.Invoke();
                    }
                } 
            };

            CrossPushNotification.Current.OnNotificationOpened += (s, p) =>
            {
                
                Action action = ParsePush(p.Data);
                if (action != null)
                {
                    if (App.IsActive)
                    {
                        action.Invoke();

                    } else {
                        AddAction(p.Identifier, action);
                    }
                }
            };
        }

        private Action ParsePush(IDictionary<String, Object> data)
        {
            if (data.ContainsKey("type"))
            {
                switch (data["type"])
                {
                    case "open_news":
                        if (data.ContainsKey("id"))
                        {
                            if (int.TryParse(data["id"].ToString(), out int id) && int.TryParse(data["group"].ToString(), out int groupId))
                            {
                                return async () =>
                                {
                                    await LocalDataStore.UpdateNews(groupId, default(CancellationToken)).ConfigureAwait(false);
                                    OpenNews(groupId, id);
                                };
                            }
                        }
                        break;
                }
            }

            return null;
        }


        private void AddAction(String id, Action action)
        {

            if (!processedMessages.Contains(id))
            {
                processedMessages.Add(id);
                runAction.Add(action);
            }
        }

        #endregion PUSH


        private static void LoadGroupConfig()
        {
            try
            {
                if (group.Map != null) 
                {
                    mapPin = new Position(group.Map.Lat, group.Map.Lon);
                    mapPinLabel = group.Map.Text;
                }
                else 
                {
                    mapPin = new Position();
                    mapPinLabel = null;
                }

                if (group.Plan != null) 
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            List<Uri> uris = new List<Uri>();

                            foreach (KeyValuePair<String, String> kvp in group.Plan)
                            {
                                uris.Add(new Uri(kvp.Value));
                            }

                            await ImageStore.StroreImageCache(uris, new ObservableProgress(), default(CancellationToken)).ConfigureAwait(false);
                        }
                        catch (Exception)
                        {
                            
                        }
                    });
                }

                if (group.Schema != null) 
                {
                    Schema schema = group.Schema;

                    app.Resources["TitleColor"] = schema.TitleColor;
                    app.Resources["TitleLineColor"] = schema.TitleLineColor;
                    app.Resources["MainBackgroundColor"] = schema.MainBackgroundColor;
                    app.Resources["MainForegroundColor"] = schema.MainForegroundColor;
                    app.Resources["DividerColor"] = schema.DividerColor;
                    app.Resources["TextColor"] = schema.TextColor;
                    app.Resources["ButtonBorderColor"] = schema.ButtonBorderColor;
                    app.Resources["ButtonBackgroundColor"] = schema.ButtonBackgroundColor;

                    app.OnThemeChange?.Invoke(app, new SchemaEventArgs(group.Schema));
                }
            }
            catch (Exception)
            {
                
            }
        }
    }
}

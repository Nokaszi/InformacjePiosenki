using DevEnvExe_LocalStorage;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unosquare.Swan.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PiosenkiInfo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LikePage : ContentPage
    {
        string id;
        string toke;
        string typ;
        string u;
        IEnumerable<SongEntity> song;
        public LikePage(string user, string token, string type)
        {
            InitializeComponent();
            toke = token;
            typ = type;
            u = user;
            SQLHelper dbHelper = new SQLHelper(user);
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                List<string> traks = new List<string> { };
                song = dbHelper.GetItems();

                SpotifyWebAPI _spotify = new SpotifyWebAPI()
                {
                    AccessToken = token,
                    TokenType = type

                };

                foreach (SongEntity s in song)
                {
                    traks.Add(_spotify.GetTrack(s.Song_id).Name);
                }
                list.ItemsSource = traks;
            }
            else
            {
                DisplayAlert("Alert", "Sprawdź połączenie z internetem", "OK");

            }

        }

        private void list_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var a = e.ItemIndex;
            NextPage(song.ElementAt(a).Song_id, toke, typ);
        }

        async void NextPage(string id, string token, string type)
        {
            await Navigation.PushAsync(new Page1(u,id, token, type));
        }
    }
}
using DevEnvExe_LocalStorage;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.TizenSpecific;
using Xamarin.Forms.Xaml;

namespace PiosenkiInfo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        string id_song;
        SQLHelper sqlHelper;
        public Page1(string user,string id, string token, string type)
        {
            id_song = id;
            InitializeComponent();
            sqlHelper=new SQLHelper(user);
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                FullTrack track;
                SpotifyWebAPI _spotify = new SpotifyWebAPI()
                {
                    AccessToken = token,
                    TokenType = type
                };
                track = _spotify.GetTrack(id);
                name.Text = track.Name;
                artist.Text = track.Album.Name+" -";
                for (int i = 0; i < track.Artists.Count; i++)
                    artist.Text += " " + track.Artists[i].Name;
                rok.Text =track.Album.ReleaseDate;
                images.Source = track.Album.Images[0].Url;

                if (sqlHelper.GetItem(id_song) != null)
                {
                    button.Text = "Usuń z ulubionych";
                }
            }
            else
            {
                DisplayAlert("Alert", "Sprawdź połączenie z internetem", "OK");
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            //SQLHelper sqlHelper = new SQLHelper();
            if (sqlHelper.GetItem(id_song) == null)
            {
                SongEntity song = new SongEntity();
                song.Song_id = id_song;
                sqlHelper.SaveItem(song);
                button.Text = "Usuń z ulubionych";
            }
            else
            {
                sqlHelper.DeleteItem(sqlHelper.GetItem(id_song).ID);
                button.Text = "Dodaj do ulubionych";
            }


        }
    }
}
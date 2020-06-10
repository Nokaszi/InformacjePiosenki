using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Models;
using Unosquare.Swan.Formatters;
using Xamarin.Forms;

namespace PiosenkiInfo
{
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        SpotifyWebAPI api;
        string toke;//= "BQClwSVQ6D5OO3xR2Zh3wORRoWQMI5t-7nW1BECZw2WW0f9yVbpAn-vc05SdGR7xJyciLK0SaZKLzrou-E95ZWMrpjAYMhro8LTMWSBAjFlqu6WPNThuivTsZSfB_ny087RXg519TzjIDL35sn73dD03s7GpM28K-_Fqdw";
        public MainPage()
        {
            InitializeComponent();
            var spotifyClient = "26bfbc49b1c54ab09b2474c4d6b3b318";
             var spotifySecret = "487c0689cd9d4fb1a0935619f78f5fca";

             WebClient webClient = new WebClient();

             NameValueCollection postparams = new NameValueCollection();
             postparams.Add("grant_type", "client_credentials");

             string authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format("{0}:{1}", spotifyClient, spotifySecret)));
            webClient.Headers.Add(HttpRequestHeader.Authorization, "Basic " + authHeader);

             byte[] tokenResponse = webClient.UploadValues("https://accounts.spotify.com/api/token", postparams);

             string textResponse = Encoding.UTF8.GetString(tokenResponse);
            Console.WriteLine(textResponse);

             toke = textResponse.Substring(17, 83);
            Console.WriteLine(toke);

        }
        private void login(object sender, EventArgs e)
        {
            SQLHelperUser sqlUser = new SQLHelperUser();
            if (sqlUser.GetItem(username.Text) != null)
            {

                if (sqlUser.GetItem(username.Text).Password == password.Text)
                    NextPage(username.Text, toke, "Bearer");
                else
                {
                    DisplayAlert("Error", "Błąedne hasło", "OK");
                }
            }
            else
            {
                User user = new User();
                user.Name = username.Text;
                user.Password = password.Text;
                sqlUser.SaveItem(user);
                DisplayAlert("Alert", "Konto zostało utworzone, zaloguj się", "OK");
                username.Text = "";
                password.Text = "";
            }
        }
        async void NextPage(string user,string token, string type)
        {
            await Navigation.PushAsync(new ListPage(user, token, type));
        }
    }
}

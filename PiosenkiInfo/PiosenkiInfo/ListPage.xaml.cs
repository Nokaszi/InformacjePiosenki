using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PiosenkiInfo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListPage : ContentPage
    {
        SearchItem item;
        string AccessToke;//= "BQCTYgZt8Say-9SIkHhut8DdtkpNRtd0ebPAMc0hQAV3u2hZLG7VSqSYKvyxiNwuJ98ALltcMcgHwQoJgsXOQhYYNrkTG-frwQH1-p6fJ85NAM8BTgFqYWAOz_FnTkkozH2zUDVA8Q-8cATUD1Pwp6wDSbm7evDhFgJRywRAX4Ynh-xBcoZyxKvPcDEYcbcZQJHqDwdzLgT8UA";
        string TokenTyp;// = "Bearer";
        string user;

        public ListPage(string u,string token, string type)
        {
            InitializeComponent();
            AccessToke = token;//"BQClwSVQ6D5OO3xR2Zh3wORRoWQMI5t-7nW1BECZw2WW0f9yVbpAn-vc05SdGR7xJyciLK0SaZKLzrou-E95ZWMrpjAYMhro8LTMWSBAjFlqu6WPNThuivTsZSfB_ny087RXg519TzjIDL35sn73dD03s7GpM28K-_Fqdw";
            TokenTyp = type;
            user = u;
        }

        async void NextPage(string u,string id,string token, string type)
        {
            await Navigation.PushAsync(new Page1(u,id,token,type));
        }

        private void list_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var a = e.ItemIndex;
            NextPage(user,item.Tracks.Items[a].Id, AccessToke, TokenTyp);
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            FullTrack track;
            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                List<string> traks = new List<string> { };
                List<string> image = new List<string> { };
                List<string> artist = new List<string> { };
                ObservableCollection<Contacts> MyList = new ObservableCollection<Contacts>();
                var search = search_bar.Text.ToString();
                Console.WriteLine(AccessToke);
                SpotifyWebAPI _spotify = new SpotifyWebAPI()
                {
                    AccessToken = AccessToke,
                    TokenType = TokenTyp
                };
                item = _spotify.SearchItems(search, SpotifyAPI.Web.Enums.SearchType.Track);
                for (int i = 0; i < item.Tracks.Items.Count; i++)
                {
                    track = _spotify.GetTrack(item.Tracks.Items[i].Id);
                    traks.Add(track.Name);
                    artist.Add(track.Artists.ToString());
                    image.Add(track.Album.Images[2].Url);
                    MyList.Add(new Contacts { Title = track.Name, Artist = track.Artists[0].Name, Images = track.Album.Images[2].Url });
                }

                list.ItemsSource = MyList;
            }
            else
            {
                DisplayAlert("Alert", "Sprawdź połączenie z internetem", "OK");

            }


        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            likePage(user, AccessToke, TokenTyp);
        }
        async void likePage(string user, string token, string typ)
        {
            await Navigation.PushAsync(new LikePage(user,token,typ));
        }
    }
}
public class Contacts
{

    public string Title
    {
        get;
        set;
    }
    public string Artist
    {
        get;
        set;
    }
    public string Images
    {
        get;
        set;
    }
}
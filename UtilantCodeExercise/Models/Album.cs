using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UtilantCodeExercise.Helpers;

namespace UtilantCodeExercise.Models
{
    public class Album
    {
        public int userId { get; set; }
        public int id { get; set; }
        public string title { get; set; }

        public List<Photo> photos { get; set; }
        public User user { get; set; }

        public Album()
        {
            photos = new List<Photo>();
        }

        public void GetAlbumData()
        {
            string json = DataRetriever.GetJsonData("photos?albumId=" + id);
            photos = JsonConvert.DeserializeObject<List<Photo>>(json);

            // Since there's only one user per album, but it returns in a list from the source, we can trim it down from a list to a single object on retrieval
            json = DataRetriever.GetJsonData("users?id=" + userId);
            List<User> users = JsonConvert.DeserializeObject<List<User>>(json);
            user = users.FirstOrDefault();
        }
    }
}
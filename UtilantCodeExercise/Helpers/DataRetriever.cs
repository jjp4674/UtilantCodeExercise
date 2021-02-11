using System.Net;

namespace UtilantCodeExercise.Helpers
{
    public static class DataRetriever
    {
        public static string GetJsonData(string query)
        {
            string json;

            using (var wc = new WebClient())
            {
                json = wc.DownloadString(@"https://jsonplaceholder.typicode.com/" + query);
            }

            return json;
        }
    }
}
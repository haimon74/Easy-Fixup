using System;
using System.Net;

namespace AspNetDating.Classes
{
    /// <summary>
    /// This class handles the Google API integration
    /// </summary>
    public static class GoogleMaps
    {
        /// <summary>
        /// Gets the coordinates.
        /// </summary>
        /// <param name="location">The location.</param>
        /// <returns></returns>
        public static double[] GetCoordinates(string location)
        {
            if (Config.ThirdPartyServices.GoogleMapsAPIKey.Length <= 1) return null;

            double latitude, longitude;
            string gApiUrl = String.Format("https://maps.google.com/maps/geo?q={0}&output=csv&key={1}",
                                           location, Config.ThirdPartyServices.GoogleMapsAPIKey);
            var wc = new WebClient();
            string csvData = wc.DownloadString(gApiUrl);
            string[] csvFields = csvData.Split(',');
            if (csvFields[0] != "200") return null;
            try
            {
                latitude = Convert.ToDouble(csvFields[2], System.Globalization.CultureInfo.InvariantCulture);
                longitude = Convert.ToDouble(csvFields[3], System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
            return new[] {latitude, longitude};
        }
    }
}
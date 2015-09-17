/* ASPnetDating 
 * Copyright (C) 2003-2010 eStream 
 * lovehitch.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ASPnetDating license agreement.  
 * It can be found at lovehitch.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Microsoft.ApplicationBlocks.Data;

namespace AspNetDating.Classes
{
    /// <summary>
    /// A Location is represented by a City, State, ZIP Code, County, Latitude, Longitude, and ZIP 
    ///  Class.  This just so happens to correspond to the columns of the ZIP_CODES table.
    /// </summary>
    public class Location
    {
        #region CONSTRUCTORS

        /// <summary>
        /// Default constructor.  Does nothing.
        /// </summary>
        public Location()
        {
        }

        #endregion

        #region PROPERTIES

        public String City
        {
            get { return _strCity; }
            set { _strCity = value; }
        }

        public String State
        {
            get { return _strState; }
            set { _strState = value; }
        }

        public String ZipCode
        {
            get { return _strZipCode; }
            set { _strZipCode = value; }
        }

        public String County
        {
            get { return _strCounty; }
            set { _strCounty = value; }
        }

        public Double Latitude
        {
            get { return _dLatitude; }
            set { _dLatitude = value; }
        }

        public Double Longitude
        {
            get { return _dLongitude; }
            set { _dLongitude = value; }
        }

        public String ZipClass
        {
            get { return _strZipClass; }
            set { _strZipClass = value; }
        }

        #endregion

        #region METHODS

        public Double DistanceFrom(Location inRemoteLocation)
        {
            return Distance.GetDistance(this, inRemoteLocation);
        }


//		public Location[] LocationsWithinRadius (Double inRadius, int maxResults)
//		{
//			return Radius.LocationsWithinRadius (this, inRadius, maxResults);
//		}


        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("Location: {0}, {1} {2} in {3} County\n", City, State, ZipCode, County);
            str.AppendFormat("\tLatitude:\t{0}\n", Latitude);
            str.AppendFormat("\tLongitude:\t{0}\n", Longitude);
            str.AppendFormat("\tZip Class:\t{0}\n", ZipClass);

            return str.ToString();
        }

        #endregion

        #region MEMBER DATA

        private string _strCity;
        private string _strState;
        private string _strZipCode;
        private string _strCounty;
        private double _dLatitude;
        private double _dLongitude;
        private string _strZipClass;

        #endregion
    }

    public class LocationInRadius : Location
    {
        #region CONSTRUCTOR

        public LocationInRadius() : base()
        {
            DistanceToCenter = Double.MinValue;
        }

        #endregion

        #region PROPERTIES

        public Double DistanceToCenter
        {
            get { return _dDistToCenter; }
            set { _dDistToCenter = value; }
        }

        #endregion

        #region METHODS

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();

            str.AppendFormat("Location: {0}, {1} {2} in {3} County\n", City, State, ZipCode, County);
            str.AppendFormat("\tLatitude:\t{0}\n", Latitude);
            str.AppendFormat("\tLongitude:\t{0}\n", Longitude);
            str.AppendFormat("\tZip Class:\t{0}\n", ZipClass);
            str.AppendFormat("\tDistance to original location:\t{0}\n", DistanceToCenter);

            return str.ToString();
        }

        #endregion

        #region MEMBER DATA 

        private double _dDistToCenter;

        #endregion
    }

    /// <summary>
    /// The Distance class takes two <see cref="AspNetDating.Classes.Location" /> objects and
    ///  uses their Latitude and Longitude to determine the distance between them.  Uses the
    ///  Haversine formula.
    /// </summary>
    public class Distance
    {
        #region CLASS METHODS

        /// <summary>
        /// Returns the distance in miles between two locations, calculated using the Haversine
        ///  forumula.
        /// </summary>
        /// <param name="inLoc1"></param>
        /// <param name="inLoc2"></param>
        /// <returns></returns>
        public static Double GetDistance(Location inLoc1, Location inLoc2)
        {
            return GetDistance(inLoc1, inLoc2, 'm');
        }


        /// <summary>
        /// Returns the distance in specified measure units between two locations, calculated using the Haversine
        /// forumula.
        /// </summary>
        /// <param name="inLoc1">The in loc1.</param>
        /// <param name="inLoc2">The in loc2.</param>
        /// <param name="units">The units. It can be 'm' - statute miles, 'n' - nautical miles, 'k' - kilometers.</param>
        /// <returns></returns>
        public static double GetDistance(Location inLoc1, Location inLoc2, char units)
        {
            Debug.Assert(null != inLoc1);
            Debug.Assert(null != inLoc2);

            if (null == inLoc1)
                throw new ArgumentNullException("inLoc1", "Null location passed in.");
            if (null == inLoc2)
                throw new ArgumentNullException("inLoc2", "Null location passed in.");

            Debug.Assert(Double.MinValue != inLoc1.Latitude);
            Debug.Assert(Double.MinValue != inLoc1.Longitude);
            Debug.Assert(Double.MinValue != inLoc2.Latitude);
            Debug.Assert(Double.MinValue != inLoc2.Longitude);

            if (Double.MinValue == inLoc1.Latitude)
                throw new ArgumentException("inLoc1.Latitude",
                                            string.Format(
                                                "The database does not contain latitude information for {0}, {1}.",
                                                inLoc1.City, inLoc1.State));
            if (Double.MinValue == inLoc1.Longitude)
                throw new ArgumentException("inLoc1.Longitude",
                                            string.Format(
                                                "The database does not contain longitude information for {0}, {1}.",
                                                inLoc1.City, inLoc1.State));
            if (Double.MinValue == inLoc2.Latitude)
                throw new ArgumentException("inLoc2.Latitude",
                                            string.Format(
                                                "The database does not contain latitude information for {0}, {1}.",
                                                inLoc2.City, inLoc2.State));
            if (Double.MinValue == inLoc2.Longitude)
                throw new ArgumentException("inLoc2.Longitude",
                                            string.Format(
                                                "The database does not contain longitude information for {0}, {1}.",
                                                inLoc2.City, inLoc2.State));

            //if (Config.Search.UsePreciseMethod)
            //    return Haversine(inLoc1, inLoc2, units);
            //else
            //{
                return TrigonometricMethod(inLoc1, inLoc2, units);
            //}
        }

        private static double TrigonometricMethod(Location inLoc1, Location inLoc2, char units)
        {
            double distanceLat = 69.1*(inLoc2.Latitude - inLoc1.Latitude);
            double distanceLong = 53*(inLoc2.Longitude - inLoc1.Longitude);

            double distanceInMiles = Math.Sqrt(Math.Pow(distanceLat, 2.0) + Math.Pow(distanceLong, 2.0));
            
            switch(units)
            {
                case 'k':
                    return distanceInMiles*1.621371192;
                case 'm':
                    return distanceInMiles;
            }

            throw new Exception("invalid measure unit");
        }

        /// <summary>
        /// Haversines the specified in loc1.
        /// </summary>
        /// <param name="inLoc1">The in loc1.</param>
        /// <param name="inLoc2">The in loc2.</param>
        /// <param name="units">The units.</param>
        /// <returns></returns>
        private static double Haversine(Location inLoc1, Location inLoc2, char units)
        {
            /*
				The Haversine formula according to Dr. Math.
				https://mathforum.org/library/drmath/view/51879.html
				
				dlon = lon2 - lon1
				dlat = lat2 - lat1
				a = (sin(dlat/2))^2 + cos(lat1) * cos(lat2) * (sin(dlon/2))^2
				c = 2 * atan2(sqrt(a), sqrt(1-a)) 
				d = R * c
				
				Where
					* dlon is the change in longitude
					* dlat is the change in latitude
					* c is the great circle distance in Radians.
					* R is the radius of a spherical Earth.
					* The locations of the two points in spherical coordinates (longitude and 
						latitude) are lon1,lat1 and lon2, lat2.
			*/
            double dDistance = Double.MinValue;
            double dLat1InRad = inLoc1.Latitude*(Math.PI/180.0);
            double dLong1InRad = inLoc1.Longitude*(Math.PI/180.0);
            double dLat2InRad = inLoc2.Latitude*(Math.PI/180.0);
            double dLong2InRad = inLoc2.Longitude*(Math.PI/180.0);

            double dLongitude = dLong2InRad - dLong1InRad;
            double dLatitude = dLat2InRad - dLat1InRad;

            // Intermediate result a.
            double a = Math.Pow(Math.Sin(dLatitude/2.0), 2.0) +
                       Math.Cos(dLat1InRad)*Math.Cos(dLat2InRad)*Math.Pow(Math.Sin(dLongitude/2.0), 2.0);

            // Intermediate result c (great circle distance in Radians).
            double c = 2.0*Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            // (Earth Radius) = 3956.0 mi = 3437.7 nm = 6367.0 km

            switch (units)
            {
                case 'm': // STATUTE MILES
                    dDistance = Globals.kEarthRadiusMiles * c;
                    break;
                case 'n': // NAUTICAL 
                    dDistance = Globals.kEarthRadiusNautical * c;
                    break;
                case 'k': // KILOMETERS 
                    dDistance = Globals.kEarthRadiusKilometers * c;
                    break;
            }

            return dDistance;
        }

        #endregion
    }

    /// <summary>
    /// Summary description for Radius.
    /// </summary>
    public class Radius
    {
        #region CLASS METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inLocation">The Location around which to search.</param>
        /// <param name="inRadius">Search radius in miles.</param>
        /// <returns></returns>
        public static LocationInRadius[] UserLocationsWithinRadius(Location inLocation, User.eGender gender, int maxAge,
                                                                   int minAge, bool photoReq, double inRadius,
                                                                   int maxResults)
        {
            Debug.Assert(null != inLocation);
            Debug.Assert(inRadius > 0.0);

            if (null == inLocation)
                throw new ArgumentNullException("inLocation", "Null location passed in.");
            if (inRadius <= 0.0)
                throw new ArgumentOutOfRangeException("inRadius", inRadius, "Invalid value for radius passed in.");

            Debug.Assert(Double.MinValue != inLocation.Latitude);
            Debug.Assert(Double.MinValue != inLocation.Longitude);

            if (Double.MinValue == inLocation.Latitude)
                throw new ArgumentException("inLocation.Latitude",
                                            string.Format(
                                                "The database does not contain latitude information for {0}, {1}.",
                                                inLocation.City, inLocation.State));
            if (Double.MinValue == inLocation.Longitude)
                throw new ArgumentException("inLocation.Longitude",
                                            string.Format(
                                                "The database does not contain longitude information for {0}, {1}.",
                                                inLocation.City, inLocation.State));


            RadiusBox radBox = RadiusBox.Create(inLocation, inRadius);
            LocationInRadius[] locs = null;

            locs = GetUserLocationsWithinRadius(inLocation, gender, maxAge, minAge, photoReq, radBox, maxResults);

            return locs;
        }

        /// <summary>
        /// Finds all <see cref="AspNetDating.Classes.LocationInRadius" />es within X miles
        ///  of inRefLoc.
        /// </summary>
        /// <remarks>
        /// To speed the calculation, this method finds all areas within a square area of dimension
        ///  (2*Radius)x(2*Radius).  Any city with a Lat/Lon pair that falls within this square is
        ///  returned.  However, only those cities whose distance is less than or equal to Radius
        ///  miles from inRefLoc are returned.  This has the unfortunate side effect of selecting
        ///  from ~22% more area than is necessary.
        /// </remarks>
        /// <param name="inRefLoc">The central location from which we are trying to find other locations within the specified radius.</param>
        /// <param name="inBounds">A class containing the "box" that encloses inRefLoc.  Used to approximate a circle of Radius R centered around the point inRefLoc.</param>
        /// <returns>0 or more <see cref="AspNetDating.Classes.LocationInRadius" />es that are
        ///  within Radius miles of inRefLoc.</returns>
        public static LocationInRadius[] GetUserLocationsWithinRadius(Location inRefLoc, User.eGender gender, int maxAge,
                                                                      int minAge, bool photoReq, RadiusBox inBounds,
                                                                      int maxResults)
        {
            if (maxResults < 0)
                throw new Exception("Invalid maxResults value");

            List<LocationInRadius> lLocs = new List<LocationInRadius>();

//			sql.Append ("SELECT * FROM ZIP_CODES WHERE ");
//			sql.Append ("IIf(ISNULL(LATITUDE),999.0,CDbl(LATITUDE)) >= ? AND ");
//			sql.Append ("IIf(ISNULL(LATITUDE),999.0,CDbl(LATITUDE)) <= ? AND ");
//			sql.Append ("IIf(ISNULL(LONGITUDE),999.0,CDbl(LONGITUDE)) >= ? AND ");
//			sql.Append ("IIf(ISNULL(LONGITUDE),999.0,CDbl(LONGITUDE)) <= ? ");
//			sql.Append ("ORDER BY CITY, STATE, ZIP");

//			oleCmd.Parameters.Add (new OleDbParameter ("SouthLat", inBounds.BottomLine));
//			oleCmd.Parameters.Add (new OleDbParameter ("NorthLat", inBounds.TopLine));
//			oleCmd.Parameters.Add (new OleDbParameter ("WestLong", inBounds.LeftLine));
//			oleCmd.Parameters.Add (new OleDbParameter ("EastLong", inBounds.RightLine));


            using (SqlConnection conn = Config.DB.Open())
            {
                SqlDataReader reader =
                    SqlHelper.ExecuteReader(conn,
                                            "FetchUserLocationsInRadius",
                                            (int) gender,
                                            DateTime.Now.Subtract(TimeSpan.FromDays(maxAge*365.25)),
                                            DateTime.Now.Subtract(TimeSpan.FromDays(minAge*365.25)),
                                            photoReq,
                                            inBounds.BottomLine,
                                            inBounds.TopLine,
                                            inBounds.LeftLine,
                                            inBounds.RightLine,
                                            maxResults);


                while (reader.Read())
                {
                    LocationInRadius loc = new LocationInRadius();

                    loc.City = (string) reader["CITY"];
                    loc.State = (string) reader["STATE"];
                    loc.ZipCode = (string) reader["ZIP"];
                    loc.County = (string) reader["COUNTY"];
                    loc.Latitude = Double.Parse(Convert.ToString(reader["LATITUDE"]), CultureInfo.InvariantCulture);
                    loc.Longitude = Double.Parse(Convert.ToString(reader["LONGITUDE"]), CultureInfo.InvariantCulture);
                    loc.ZipClass = (string) reader["ZIP_CLASS"];
                    loc.DistanceToCenter = Distance.GetDistance(inRefLoc, loc);

                    if (loc.DistanceToCenter <= inBounds.Radius)
                        lLocs.Add(loc);
                }
            }

            lLocs.Sort(new LocationInRadiusComparer());


            return lLocs.ToArray();
        }

        #endregion

        /// <summary>
        /// Allows for sorting of an ArrayList of <see cref="AspNetDating.Classes.LocationInRadius" /> 
        ///  objects by DistanceToCenter, ascending.
        /// </summary>
        private class LocationInRadiusComparer : IComparer<LocationInRadius>
        {
            public int Compare(LocationInRadius x, LocationInRadius y)
            {
                if (x.DistanceToCenter < y.DistanceToCenter)
                    return -1;
                else if (x.DistanceToCenter > y.DistanceToCenter)
                    return 1;
                else
                    return 0;
            }
        }
    }

    /// <summary>
    /// A RadiusBox encloses a "box" area around a location, where each side of the square is
    ///  radius miles away from the location.  Doing it this way includes ~22% more area than if we
    ///  used a proper circle, but using a box simplifies the SQL query.
    /// </summary>
    public class RadiusBox
    {
        #region CONSTRUCTORS

        public RadiusBox()
        {
        }

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Represents the Southern latitude line.
        /// </summary>
        public Double BottomLine
        {
            get { return _dBottomLatLine; }
            set { _dBottomLatLine = value; }
        }


        /// <summary>
        /// Represents the Northern latitude line.
        /// </summary>
        public Double TopLine
        {
            get { return _dTopLatLine; }
            set { _dTopLatLine = value; }
        }


        /// <summary>
        /// Represents the Western longitude line.
        /// </summary>
        public Double LeftLine
        {
            get { return _dLeftLongLine; }
            set { _dLeftLongLine = value; }
        }


        /// <summary>
        /// Represents the Eastern longitude line.
        /// </summary>
        public Double RightLine
        {
            get { return _dRightLongLine; }
            set { _dRightLongLine = value; }
        }


        /// <summary>
        /// Represents the radius of the search area.
        /// </summary>
        public Double Radius
        {
            get { return _dRadius; }
            set { _dRadius = value; }
        }

        #endregion

        #region MEMBER DATA

        private double _dBottomLatLine;
        private double _dTopLatLine;
        private double _dLeftLongLine;
        private double _dRightLongLine;
        private double _dRadius;

        #endregion

        #region CLASS METHODS

        /// <summary>
        /// Creates a box that encloses the specified location, where the sides of the square
        ///  are inRadius miles away from the location at the perpendicular.  Note that we do
        ///  not actually generate lat/lon pairs; we only generate the coordinate that 
        ///  represents the side of the box.
        /// </summary>
        /// <remarks>
        /// <para>Formula obtained from Dr. Math at https://www.mathforum.org/library/drmath/view/51816.html.</para>
        /// </remarks>
        /// <param name="inLocation"></param>
        /// <param name="inRadius"></param>
        /// <returns></returns>
        public static RadiusBox Create(Location inLocation, Double inRadius)
        {
            /*
				A point {lat,lon} is a distance d out on the tc radial from point 1 if:
			
				lat = asin (sin (lat1) * cos (d) + cos (lat1) * sin (d) * cos (tc))
				dlon = atan2 (sin (tc) * sin (d) * cos (lat1), cos (d) - sin (lat1) * sin (lat))
				lon = mod (lon1 + dlon + pi, 2 * pi) - pi
			
				Where:
					* d is the distance in radians (an arc), so the desired radius divided by
						the radius of the Earth.
					* tc = 0 is N, tc = pi is S, tc = pi/2 is E, tc = 3*pi/2 is W.
			*/
            double lat;
            double dlon;
            double dLatInRads = inLocation.Latitude*(Math.PI/180.0);
            double dLongInRads = inLocation.Longitude*(Math.PI/180.0);
            double dDistInRad = inRadius/Globals.kEarthRadiusMiles;
            RadiusBox box = new RadiusBox();
            box.Radius = inRadius;

            //	N (tc == 0):
            //		lat = asin (sin(lat1)*cos(d) + cos(lat1)*sin(d))
            //			= asin (sin(lat1 + d))
            //			= lat1 + d
            //	Unused:
            //		lon	= lon1, because north-south lines follow lines of longitude.
            box.TopLine = dLatInRads + dDistInRad;
            box.TopLine *= (180.0/Math.PI);

            //	S (tc == pi):
            //		lat = asin (sin(lat1)*cos(d) - cos(lat1)*sin(d))
            //			= asin (sin(lat1 - d))
            //			= lat1 - d
            //	Unused:
            //		lon	= lon1, because north-south lines follow lines of longitude.
            box.BottomLine = dLatInRads - dDistInRad;
            box.BottomLine *= (180.0/Math.PI);

            //	E (tc == pi/2):
            //		lat	 = asin (sin(lat1)*cos(d))
            //		dlon = atan2 (sin(tc)*sin(d)*cos(lat1), cos(d) - sin(lat1)*sin(lat))
            //		lon	 = mod (lon1 + dlon + pi, 2*pi) - pi
            lat = Math.Asin(Math.Sin(dLatInRads)*Math.Cos(dDistInRad));
            dlon =
                Math.Atan2(Math.Sin(Math.PI/2.0)*Math.Sin(dDistInRad)*Math.Cos(dLatInRads),
                           Math.Cos(dDistInRad) - Math.Sin(dLatInRads)*Math.Sin(lat));
            box.RightLine = ((dLongInRads + dlon + Math.PI)%(2.0*Math.PI)) - Math.PI;
            box.RightLine *= (180.0/Math.PI);

            //	W (tc == 3*pi/2):
            //		lat	 = asin (sin(lat1)*cos(d))
            //		dlon = atan2 (sin(tc)*sin(d)*cos(lat1), cos(d) - sin(lat1)*sin(lat))
            //		lon	 = mod (lon1 + dlon + pi, 2*pi) - pi
            dlon =
                Math.Atan2(Math.Sin(3.0*Math.PI/2.0)*Math.Sin(dDistInRad)*Math.Cos(dLatInRads),
                           Math.Cos(dDistInRad) - Math.Sin(dLatInRads)*Math.Sin(lat));
            box.LeftLine = ((dLongInRads + dlon + Math.PI)%(2.0*Math.PI)) - Math.PI;
            box.LeftLine *= (180.0/Math.PI);

            double temp;
            if (box.LeftLine > box.RightLine)
            {
                temp = box.RightLine;
                box.RightLine = box.LeftLine;
                box.LeftLine = temp;
            }

            if (box.BottomLine > box.TopLine)
            {
                temp = box.TopLine;
                box.TopLine = box.BottomLine;
                box.BottomLine = temp;
            }

            return box;
        }

        #endregion
    }

    /// <summary>
    /// Summary description for Globals.
    /// </summary>
    public class Globals
    {
        #region CONSTANTS

        /// <summary>
        /// The radius of the Earth in miles, assuming it is a sphere.
        /// </summary>
        public const Double kEarthRadiusMiles = 3956.0;
        public const Double kEarthRadiusKilometers = 6367.0;
        public const Double kEarthRadiusNautical = 3437.7;

        #endregion
    }
}
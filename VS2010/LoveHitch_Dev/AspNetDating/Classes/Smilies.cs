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
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

namespace AspNetDating.Classes
{
    /// <summary>
    /// This class handles the Smilies
    /// </summary>
    public static class Smilies
    {
        private static readonly Dictionary<string, Smiley> htSmilies = new Dictionary<string, Smiley>();
        private static readonly List<string> lSmiliesKeysSorted = new List<string>();
        private static bool smiliesLoaded;
        private static bool smiliesLoadingInProgress;

        /// <summary>
        /// Gets the smileys.
        /// </summary>
        /// <value>The smileys.</value>
        public static Dictionary<string, Smiley> dSmileys
        {
            get
            {
                if (!smiliesLoaded) LoadSmilies();
                return htSmilies;
            }
        }

        private static void LoadSmilies()
        {
            if (!File.Exists(Config.Directories.Smilies + @"\smilies.pak")) return;

            smiliesLoadingInProgress = true;

            lock (htSmilies)
            {
                TextReader textReader;
                try
                {
                    textReader = File.OpenText(Config.Directories.Smilies + @"\smilies.pak");
                }
                catch (Exception err)
                {
                    Global.Logger.LogError("LoadSmilies", err);
                    return;
                }

                htSmilies.Clear();

                string textLine;
                while ((textLine = textReader.ReadLine()) != null)
                {
                    string[] splitLine = Regex.Split(textLine, Regex.Escape("=+:"));
                    var smiley = new Smiley
                                     {
                                         Key = splitLine[2],
                                         Description = splitLine[1],
                                         Image = splitLine[0],
                                         Secondary = false
                                     };
                    htSmilies.Add(smiley.Key, smiley);
                }
                textReader.Close();

                if (File.Exists(Config.Directories.Smilies + @"\smilies2.pak"))
                {
                    try
                    {
                        textReader = File.OpenText(Config.Directories.Smilies + @"\smilies2.pak");
                    }
                    catch (Exception err)
                    {
                        Global.Logger.LogError("LoadSmilies", err);
                        return;
                    }

                    while ((textLine = textReader.ReadLine()) != null)
                    {
                        string[] splitLine = Regex.Split(textLine, Regex.Escape("=+:"));
                        var smiley = new Smiley
                                         {
                                             Key = splitLine[2],
                                             Description = splitLine[1],
                                             Image = splitLine[0],
                                             Secondary = true
                                         };
                        if (htSmilies.ContainsKey(smiley.Key)) continue;
                        bool duplicate = false;
                        foreach (var value in htSmilies.Values)
                        {
                            if (!value.Secondary && value.Image == smiley.Image)
                            {
                                duplicate = true;
                                break;
                            }
                        }
                        if (duplicate) continue;

                        htSmilies.Add(smiley.Key, smiley);
                    }
                    textReader.Close();
                }

                lock (lSmiliesKeysSorted)
                {
                    lSmiliesKeysSorted.AddRange(htSmilies.Keys);
                    lSmiliesKeysSorted.Sort(new LengthComparer());
                    lSmiliesKeysSorted.Reverse();
                }
            }

            smiliesLoaded = true;
            smiliesLoadingInProgress = false;
        }

        /// <summary>
        /// Processes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        public static void Process(ref string text)
        {
            if (smiliesLoadingInProgress) Thread.Sleep(1000);
            if (smiliesLoadingInProgress) return;
            if (!smiliesLoaded) LoadSmilies();
            foreach (string key in lSmiliesKeysSorted)
            {
                Smiley smiley = htSmilies[key];
                text = Regex.Replace(text, Regex.Escape(key), String.Format("<img src=\"{0}\" alt=\"{1}\" />",
                                                                            Config.Urls.Home + "/Smilies/" +
                                                                            smiley.Image,
                                                                            smiley.Description), RegexOptions.IgnoreCase);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class LengthComparer : IComparer<string>
    {
        #region IComparer Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>
        /// Value Condition Less than zero<paramref name="x"/> is less than <paramref name="y"/>.Zero<paramref name="x"/> equals <paramref name="y"/>.Greater than zero<paramref name="x"/> is greater than <paramref name="y"/>.
        /// </returns>
        public int Compare(string x, string y)
        {
            if (x.Length == y.Length)
            {
                return 0;
            }
            if (x.Length > y.Length)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        #endregion
    }


    /// <summary>
    /// Defines a single smiley
    /// </summary>
    public struct Smiley
    {
        /// <summary>
        /// The key
        /// </summary>
        public string Key;
        /// <summary>
        /// The description
        /// </summary>
        public string Description;
        /// <summary>
        /// The smiley image file
        /// </summary>
        public string Image;

        public bool Secondary;
    }
}
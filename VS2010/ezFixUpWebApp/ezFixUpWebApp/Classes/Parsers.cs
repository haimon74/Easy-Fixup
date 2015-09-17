/* ezFixUp 
 * Copyright (C) 2003-2010 eStream 
 * ezFixUp.com

 *  
 * IMPORTANT: This is a commercial software product. By using this product  
 * you agree to be bound by the terms of the ezFixUp license agreement.  
 * It can be found at ezFixUp.com/agreement.htm

 *  
 * This notice may not be removed from the source code. 
 */
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ezFixUp.Classes
{
    /// <summary>
    /// All sorts of string parsers
    /// </summary>
    public static class Parsers
    {
        /// <summary>
        /// Shortens the string.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="maxLength">Length of the max.</param>
        /// <param name="maxLines">The max lines.</param>
        /// <returns></returns>
        public static string ShortenString(string str, int maxLength, int maxLines)
        {
            string ret = str.Length > maxLength ? str.Substring(0, maxLength) : str;

            while (Regex.Matches(ret, @"\n").Count >= maxLines)
            {
                ret = ret.Remove(ret.LastIndexOf('\n'));
            }

            return ret;
        }

        public static string TrimLongWords(string str, int maxWordLength)
        {
            if (str == null) return null;
            return Regex.Replace(str, @"(\S{" + (maxWordLength + 1) + @"})", m => m.Value + " ", 
                RegexOptions.Compiled);
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="str">The string.</param>
        public static void ProcessMessage(ref string str)
        {
            str = HttpUtility.HtmlEncode(str);
            str = str.Replace("\n", "<br>");
            Smilies.Process(ref str);
        }

        /// <summary>
        /// Processes the name of the affiliate.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ProcessAffiliateName(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// Processes the name of the group.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ProcessGroupName(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// Processes the group description.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ProcessGroupDescription(string str)
        {
            return ProcessGroupDescription(str, false);
        }

        public static string ProcessAdSubject(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        public static string ProcessAdDescription(string str)
        {
            return ProcessGroupDescription(str, false);
        }

        /// <summary>
        /// Processes the group description.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="stripTags">if set to <c>true</c> [strip tags].</param>
        /// <returns></returns>
        public static string ProcessGroupDescription(string str, bool stripTags)
        {
            string ret = HttpUtility.HtmlEncode(str);
            if (stripTags)
            {
                StripTags(ref ret);
            }
            else
            {
                ParseBBCode(ref ret);
                Smilies.Process(ref ret);
            }

            ret = ret.Replace("\n", "<br>");
            return ret;
        }

        /// <summary>
        /// Processes the name of the group topic.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ProcessGroupTopicName(string str)
        {
            return HttpUtility.HtmlEncode(str);
        }

        /// <summary>
        /// Processes the group post.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ProcessGroupPost(string str)
        {
            return ProcessGroupPost(str, false);
        }

        /// <summary>
        /// Processes the group post.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="stripTags">if set to <c>true</c> [strip tags].</param>
        /// <returns></returns>
        public static string ProcessGroupPost(string str, bool stripTags)
        {
            string ret = HttpUtility.HtmlEncode(str);
            if (stripTags)
            {
                StripTags(ref ret);
            }
            else
            {
                ret = ret.Replace("&amp;", "&");
                ret = ret.Replace("&quot;", "\"");
                ParseBBCode(ref ret);
                Smilies.Process(ref ret);
                ParseVideoLinks(ref ret);
            }

            ret = ret.Replace("\n", "<br>");
            return ret;
        }

        private static void StripTags(ref string str)
        {
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
            str = Regex.Replace(str, @"\[/?(?:[\w\s=]*)\]", String.Empty, options);
        }

        private static void ParseBBCode(ref string str)
        {
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
            str = Regex.Replace(str, @"\[b(?:\s*)\]((.|\n)*?)\[/b(?:\s*)\]", "<b>$1</b>", options);
            str = Regex.Replace(str, @"\[i(?:\s*)\]((.|\n)*?)\[/i(?:\s*)\]", "<i>$1</i>", options);
            str = Regex.Replace(str, @"\[u(?:\s*)\]((.|\n)*?)\[/u(?:\s*)\]", "<u>$1</u>", options);
            str = Regex.Replace(str, @"\[left(?:\s*)\]((.|\n)*?)\[/left(?:\s*)]",
                              "<div style=\"text-align:left\">$1</div>", options);
            str = Regex.Replace(str, @"\[center(?:\s*)\]((.|\n)*?)\[/center(?:\s*)]",
                              "<div style=\"text-align:center\">$1</div>", options);
            str = Regex.Replace(str, @"\[right(?:\s*)\]((.|\n)*?)\[/right(?:\s*)]",
                              "<div style=\"text-align:right\">$1</div>", options);
            str = Regex.Replace(str, @"\[quote(?:\s*)(?:user)?=(?:"")?([a-z\d_-]*?)(?:"")?\]",
                              "<div class=\"quotelabel\">$1 " + Lang.Trans("wrote") +
                              ":</div><div class=\"quotetext\">", options);
            str = Regex.Replace(str, @"\[/quote(\s*)\]", "</div>", options);
            str = Regex.Replace(str, @"\[quote(?:\s*)\]", "<div class=\"quotetext\">", options);
            str = Regex.Replace(str, @"\[/quote(?:\s*)\]", "</div>", options);

            str = Regex.Replace(str, @"\[url(?:\s*)\]www\.(.*?)\[/url(?:\s*)\]",
                              "<a href=\"https://www.$1\" target=\"_blank\" title=\"$1\" rel=\"nofollow\">$1</a>",
                              options);
            str = Regex.Replace(str, @"\[url(?:\s*)\]((.|\n)*?)\[/url(?:\s*)\]",
                              "<a href=\"$1\" target=\"_blank\" title=\"$1\" rel=\"nofollow\">$1</a>", options);
            str = Regex.Replace(str, @"\[url=(?:"")?((.|\n)*?)(?:\s*)(?:"")?\]((.|\n)*?)\[/url(?:\s*)\]",
                              "<a href=\"$1\" target=\"_blank\" title=\"$1\" rel=\"nofollow\">$3</a>", options);
            str = Regex.Replace(str, @"\[link(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]",
                              "<a href=\"$1\" target=\"_blank\" title=\"$1\" rel=\"nofollow\">$1</a>", options);
            str = Regex.Replace(str, @"\[link=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/link(?:\s*)\]",
                              "<a href=\"$1\" target=\"_blank\" title=\"$1\" rel=\"nofollow\">$3</a>", options);
            str = Regex.Replace(str, @"\[img(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]", "<img src=\"$1\" border=\"0\" />",
                              options);
            str = Regex.Replace(str, @"\[img=((.|\n)*?)x((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/img(?:\s*)\]",
                              "<img width=\"$1\" height=\"$3\" src=\"$5\" border=\"0\" />", options);
            str = Regex.Replace(str, @"\[color=(?:"")?((.|\n)*?)(?:"")?(?:\s*)\]((.|\n)*?)\[/color(?:\s*)\]",
                              "<span style=\"color: $1;\">$3</span>", options);
            str = Regex.Replace(str, @"\[hr(?:\s*)\]", "<hr />", options);
            str = Regex.Replace(str, @"\[email(?:\s*)\]((.|\n)*?)\[/email(?:\s*)\]", "<a href=\"mailto:$1\">$1</a>",
                              options);
            str = Regex.Replace(str, @"\[size=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/size(?:\s*)\]",
                              "<span style=\"font-size:$1\">$3</span>", options);
            str = Regex.Replace(str, @"\[font=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/font(?:\s*)\]",
                              "<span style=\"font-family:$1;\">$3</span>", options);
            str = Regex.Replace(str, @"\[align=((.|\n)*?)(?:\s*)\]((.|\n)*?)\[/align(?:\s*)\]",
                              "<div style=\"text-align:$1;\">$3</div>", options);
        }

        private static void ParseVideoLinks(ref string str)
        {
            const RegexOptions options = RegexOptions.IgnoreCase | RegexOptions.Compiled;
            str = Regex.Replace(str, @"https://(?:www\.)?youtube\.com/watch\?v=([\w-]+)",
                "<br><object width=\"425\" height=\"350\"><param name=\"movie\" value=\"https://www.youtube.com/v/$1\"></param><param name=\"wmode\" value=\"transparent\"></param><embed src=\"https://www.youtube.com/v/$1\" type=\"application/x-shockwave-flash\" wmode=\"transparent\" width=\"425\" height=\"350\"></embed></object><br>", options);
            str = Regex.Replace(str, @"https://(?:www\.)?vbox7\.com/play:(\w+)",
                "<br><object classid=\"clsid:D27CDB6E-AE6D-11cf-96B8-444553540000\" codebase=\"https://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,0,0\" width=\"450\" height=\"403\"><param name=\"movie\" value=\"https://i.vbox7.com/player/ext.swf?vid=$1\"><param name=\"quality\" value=\"high\"><embed src=\"https://i.vbox7.com/player/ext.swf?vid=$1\" quality=\"high\" pluginspage=\"https://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\" width=\"450\" height=\"403\"></embed></object><br>", options);
            str = Regex.Replace(str, @"https://(?:www\.)?metacafe.com/watch/(\w+)/(\w+)/",
                "<br><embed src=\"https://www.metacafe.com/fplayer/$1/$2.swf\" width=\"400\" height=\"345\" wmode=\"transparent\" pluginspage=\"https://www.macromedia.com/go/getflashplayer\" type=\"application/x-shockwave-flash\"></embed><br>", options);
        }

        /// <summary>
        /// Processes the bad words.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string ProcessBadWords(string str)
        {
            foreach (string badword in Config.Misc.BadWords.Split('\n'))
            {
                if (badword != "")
                {
                    try
                    {
                        string pattern = Config.Misc.EnableBadWordsRegularExpressions 
                            ? badword : @"(^|\W)" + Regex.Escape(badword) + @"($|\W)";
                        str = Regex.Replace(str, pattern, Config.Misc.BadWordsReplacement,
                            RegexOptions.IgnoreCase | RegexOptions.Compiled);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            return str;
        }

        /// <summary>
        /// Parses the CSV.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <returns></returns>
        public static string ParseCSV(DataTable dataTable)
        {
            if (dataTable == null) return String.Empty;

            StringBuilder csvFile = new StringBuilder(String.Empty);
            List<string> header = new List<string>(dataTable.Columns.Count);

            foreach (DataColumn column in dataTable.Columns)
            {
                header.Add(parseCSVCell(column.ColumnName));
            }

            csvFile.AppendLine(String.Join(",", header.ToArray()));

            foreach (DataRow row in dataTable.Rows)
            {
                List<string> cells = new List<string>(row.ItemArray.Length);

                foreach (object cell in row.ItemArray)
                {
//                    if (cell == null || cell == Convert.DBNull) continue;
                    string csvCell = parseCSVCell(cell as string);
                    cells.Add(csvCell);
                }
                csvFile.AppendLine(String.Join(",", cells.ToArray()));
            }

            return csvFile.ToString();
        }

        private static string parseCSVCell(string cell)
        {
            if (cell == null) return String.Empty;

            if (cell.Contains("\n") || cell.Contains("\"") || cell.Contains(","))
            {
                cell = cell.Replace("\"", "\"\"");
                cell = '"' + cell + '"';
            }

            return cell;
        }

        public static string StripHTML(this string text)
        {
            return text == null ? null : Regex.Replace(text, @"<(.|\n)*?>", string.Empty);
        }

        public static string StripScriptTags(this string text)
        {
            return Regex.Replace(text, @"<script(.|\n)*?>(.|\n)*?</script(.|\n)*?>", string.Empty,
                RegexOptions.IgnoreCase);
        }

        public static string Shorten(this string text, int length)
        {
            if (text == null) return null;
            if (text.Length <= length) return text;
            return text.Remove(length) + "...";
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Regex_comments
{
    public static class ParseAndClearService
    {
        public static string ProcessParseAndClear(string text, ChainAndShoppingCenterLogItem item)
        {
            bool flag = false;
            using (StringReader sr = new StringReader(text))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Regex regex1 = new Regex(@"^(.+)\s-\sold:\s(.*),\snew:\s?(.*)", RegexOptions.Singleline);
                    MatchCollection matches = regex1.Matches(line);

                    var match = matches.FirstOrDefault();

                    if (match != null)
                    {
                        (string field, (string old, string new_) values) row =
                        (field: match.Groups[1].Value,
                          (old: match.Groups[2].Value.Trim(),
                          new_: match.Groups[3].Value.Trim()));

                        if (row.values.old == row.values.new_) // && row.field != "Description" && row.field != "DescriptionLong"
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            stringBuilder.Append(text);
                            int index = text.IndexOf(line);
                            stringBuilder.Replace(line + "\r\n", String.Empty, index, line.Length + 2);

                            text = stringBuilder.ToString();
                            flag = true;
                        }
                    }
                }
            }
                
            if (flag)
                Helper.WriteProcessLog("One or more single lines are updated: ", item);

            // Case for multiline fields: DescriptionLong and Description

            if (text.Contains("Description"))
            {
                // CASE: Description - old: , new: 
                Regex regex3 = new Regex(@"^(DescriptionLong|Description|CommonDescriptionShort|DescriptionShort|ShortDescription)\s-\sold:\s{1},\snew:\s{1}\r\n$", RegexOptions.Singleline | RegexOptions.Multiline);
                MatchCollection matches3 = regex3.Matches(text);
                var match3 = matches3.FirstOrDefault();

                if (match3 != null)
                {
                    text = text.Replace(match3.Groups[0].Value, String.Empty);
                    Helper.WriteProcessLog("Description singleline is removed: ", item);
                }
                else
                {
                    //text = text.Replace("\r\n", "¤");

                    //text = text.Replace("¤¤", "⋆⋆");
                    //text = text.Replace("¤", "\r\n");
                    //text = text.Replace("\r\n", String.Empty);

                    //Regex regex2 = new Regex(@"(DescriptionLong|Description)\s-\sold:\s(.*),\snew:\s?(.*)", RegexOptions.Multiline);
                    Regex regex2 = new Regex(@"^(DescriptionLong|Description|CommonDescriptionShort|DescriptionShort|ShortDescription)\s-\sold:\s(.*),\snew:\s?(.*)(\r\n){3}", RegexOptions.Multiline | RegexOptions.Singleline);
                    MatchCollection matches2 = regex2.Matches(text);
                    var match2 = matches2.FirstOrDefault();

                    if (match2 != null)
                    {
                        (string field, (string old, string new_) values) row =
                            (field: match2.Groups[1].Value, (old: match2.Groups[2].Value, match2.Groups[3].Value));

                        var old = match2.Groups[2].Value.Replace("⋆", "\r\n").Trim();
                        var new_ = match2.Groups[3].Value.Replace("⋆", "\r\n").Trim();
                        if (old == new_)
                        {
                            text = text.Replace("⋆", "\r\n");

                            var str = match2.Groups[0].Value.Replace("⋆", "\r\n");
                            text = text.Replace(str, "\r\n\r\n\r\n");

                            Helper.WriteProcessLog("Multiline is updated before 3 EmptyStrings: ", item);
                        }
                        else
                        {
                            //text = text.Replace("⋆", "\r\n");
                            //Helper.WriteProcessLog("Alert - Multiline description is NOT UPDATED: ", item);
                        }
                    }
                    else
                    {
                        Regex regex4 = new Regex(@"^(DescriptionLong|CommonDescription|Description|CommonDescriptionShort|DescriptionShort|ShortDescription)\s-\sold:\s(.*),\snew:\s?(.*)(\r\n)", RegexOptions.Multiline | RegexOptions.Singleline);
                        MatchCollection matches4 = regex4.Matches(text);
                        var match4 = matches4.FirstOrDefault();

                        if (match4 != null)
                        {
                            (string field, (string old, string new_) values) row =
                                (field: match4.Groups[1].Value, (old: match4.Groups[2].Value, match4.Groups[3].Value));

                            var old = match4.Groups[2].Value.Replace("⋆", "\r\n").Trim();
                            var new_ = match4.Groups[3].Value.Replace("⋆", "\r\n").Trim();
                            if (old == new_)
                            {

                                text = text.Replace(match4.Groups[0].Value, String.Empty);

                                Helper.WriteProcessLog("Multiline is updated 1 before END: ", item);
                            }
                            else
                            {
                                Regex regex5 = new Regex(@"(^(DescriptionLong|CommonDescription|Description|CommonDescriptionShort|DescriptionShort|ShortDescription)\s-\sold:\s(.*),\snew:\s?(.*))ShortDescription\s{1}-\s{1}old:", RegexOptions.Multiline | RegexOptions.Singleline);
                                MatchCollection matches5 = regex5.Matches(text);
                                var match5 = matches5.FirstOrDefault();

                                if (match5 != null)
                                {
                                    (string field, (string old, string new_) values) row5 =
                                        (field: match5.Groups[1].Value, (old: match5.Groups[2].Value, match5.Groups[3].Value));

                                    var old5 = match5.Groups[3].Value.Replace("⋆", "\r\n").Trim();
                                    var new_5 = match5.Groups[4].Value.Replace("⋆", "\r\n").Trim();

                                    if(old5 == new_5)
                                    {

                                        text = text.Replace(match5.Groups[1].Value, String.Empty);

                                        Helper.WriteProcessLog("Multiline is updated 2 - before ShortDescription: ", item);
                                    }
                                }
                                else
                                {
                                    Helper.WriteProcessLog("Alert Multiline is not matched 0: ", item);
                                }
                            }

                        } 
                        else
                        {
                            Helper.WriteProcessLog("Alert - Multiline is not matched: 1", item);
                        }

                        //Helper.WriteProcessLog("Alert - Multiline is not matched: ", item);
                    }
                }
            }

            text = text.Replace("⋆", "\r\n");
            return text;

        }
    }
}

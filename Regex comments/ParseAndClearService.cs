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
                          (old: match.Groups[2].Value,
                          new_: match.Groups[3].Value));

                        if (row.values.old == row.values.new_ && row.field != "Description" && row.field != "DescriptionLong")
                        {

                            text = text.Replace(line + "\r\n", String.Empty);
                            text = text.Replace(line, String.Empty);

                        }
                    }
                }
            }


            // Case for multiline fields: DescriptionLong and Description

            text = text.Replace("\r\n", "¤");

            text = text.Replace("¤¤", "⋆⋆");
            text = text.Replace("¤", "\r\n");
            //text = text.Replace("\r\n", String.Empty);

            Regex regex2 = new Regex(@"(DescriptionLong|Description)\s-\sold:\s(.*),\snew:\s?(.*)", RegexOptions.Multiline);
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
                    text = text.Replace(str, String.Empty);

                    Helper.WriteProcessLog(item);
                }
                else
                {
                    text = text.Replace("⋆", "\r\n");
                }
            }

            text = text.Replace("⋆", "\r\n");
            return text;
        }
    }
}

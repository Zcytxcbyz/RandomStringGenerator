using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RandomStringGenerator
{
    /// <summary>
    /// RandomStringGenerator
    /// </summary>
    public class RandomStringGenerator
    {
        /// <summary>
        /// 说明：
        ///[自定义字符集](长度){模式}如：CHAR:[a-z,A-Z,0-9,\[,\]](10,20){0};END。
        ///长度：(最小长度，最大长度)或(固定长度)。
        ///模式(可省)：{0}为默认模式，{1}为生成完全相同字符，{2}为生成完全不重复字符。
        /// </summary>
        /// <param name="Expression">表达式</param>
        /// <returns>随机字符串</returns>
        public string Generator(string Expression)
        {
            MatchCollection matcon = Regex.Matches(Expression, @"(?<=\[.*)[0-9A-Za-z]-[0-9A-Za-z](?=.*\]\([^\(\)]+\))");
            foreach (Match match in matcon)
            {
                string fullchar = null;
                char firstchar = Regex.Match(match.Value.Trim(), @".(?=-)").Value.Trim().ToCharArray()[0];
                char secondchar = Regex.Match(match.Value.Trim(), @"(?<=-).").Value.Trim().ToCharArray()[0];       
                if (firstchar < secondchar)
                {
                    for(char i = firstchar; 
                        i <= secondchar; 
                        i++)
                    {
                        fullchar += i.ToString() + ",";
                    }
                    fullchar = fullchar.TrimEnd(',');
                    Expression = Expression.Replace(match.Value, fullchar);
                }
            }     
            Expression = Regex.Replace(Expression, @"(?<=\[.*)((?<!\\)\\,)(?=.*\]\([^\(\)]+\))", "44");
            Expression = Regex.Replace(Expression, @"(?<=\[.*)((?<!\\)\\\()(?=.*\]\([^\(\)]+\))", "40");
            Expression = Regex.Replace(Expression, @"(?<=\[.*)((?<!\\)\\\))(?=.*\]\([^\(\)]+\))", "41");
            Expression = Regex.Replace(Expression, @"(?<=\[.*)((?<!\\)\\\[)(?=.*\]\([^\(\)]+\))", "91");
            Expression = Regex.Replace(Expression, @"(?<=\[.*)((?<!\\)\\\])(?=.*\]\([^\(\)]+\))", "93");
            Expression = Regex.Replace(Expression, @"(?<=\[.*)((?<!\\)\\\\)(?=.*\]\([^\(\)]+\))", "92");
            Expression = Regex.Replace(Expression, @"(?<=\[[^\[\]]+?]\([^\(\)]+?)\)(?!{(0|1|2)})", "){0}");
            string result = null;
            string pattern = @"^(.*\[(([^,]|44|40|41|91|93|92|123|125),)+([^,]|44|40|41|91|93|92|123|125)\]\([0-9]+(,[0-9]+)?\){(0|1|2)}.*)+$";
            if (Regex.IsMatch(Expression, pattern))
            {
                result = Expression;
                MatchCollection mc = Regex.Matches(Expression, @"\[[^\[\]]+]\([^\(\)]+\){(0|1|2)}");
                foreach (Match m in mc)
                {
                    int mode = Convert.ToInt32(Regex.Match(m.Value, @"(?<=\[.+\]\(.+\){).(?=})").Value);
                    //string prefix = Regex.Match(m.Value, @".*(?=\[.+\]\(.+\){.})").Value;
                    //string suffix = Regex.Match(m.Value, @"(?<=\[.+\]\(.+\){.}).*").Value;
                    string[] Chars = Regex.Match(m.Value, @"(?<=\[)[^\[\]]+(?=\]\(.+\){.})").Value.Trim().Split(',');
                    char[] CustomChars = new char[Chars.Length];
                    for (int i = 0; i < Chars.Length; i++)
                    {
                        if (Chars[i].Length == 1)
                        {
                            CustomChars[i] = Chars[i].ToCharArray()[0];
                        }
                        else if (Chars[i].Length >= 2)
                        {
                            CustomChars[i] = (char)Convert.ToInt32(Chars[i]);
                        }
                    }
                    string[] Lengths = Regex.Match(m.Value, @"(?<=\[.+\]\()[^\(\)]+(?=\){.})").Value.Trim().Split(',');
                    if (Lengths.Length == 1)
                    {
                        int Length = Convert.ToInt32(Lengths[0]);
                        result = result.Replace(m.Value, RanStrGen(CustomChars, Length, mode));
                        //result += prefix + RanStrGen(CustomChars, Length, mode) + suffix;
                    }
                    else if (Lengths.Length == 2)
                    {
                        int MinLength = Convert.ToInt32(Lengths[0]);
                        int MaxLength = Convert.ToInt32(Lengths[1]);
                        result = result.Replace(m.Value, RanStrGen(CustomChars, MinLength, MaxLength, mode));
                        //result += prefix + RanStrGen(CustomChars, MinLength, MaxLength, mode) + suffix;
                    }
                }
            }
            return result;

        }
        private string RanStrGen(char[] CustomChars,int MinLength,int MaxLength,int Mode=0)
        {
            int seed = Guid.NewGuid().GetHashCode();
            Random random = new Random(seed);
            int length = random.Next(MinLength, MaxLength + 1);
            string RandomsString = null;
            if (Mode == 0)
            {
                for (int i = 0; i < length; i++)
                {
                    RandomsString += CustomChars[random.Next(0, CustomChars.Length)];
                }
            }
            else if (Mode == 1)
            {
                char Chars = CustomChars[random.Next(0, CustomChars.Length)];
                for (int i = 0; i < length; i++)
                {
                    RandomsString += Chars;
                }
            }
            else if (Mode == 2)
            {
                bool HaveDuplicates = CustomChars.ToList().Exists(x => CustomChars.Count(y => y.Equals(x)) > 1);
                if (MaxLength <= CustomChars.Length && !HaveDuplicates)
                {
                    List<char> CharsList = new List<char> { };
                    while (CharsList.Count < length)
                    {
                        CharsList.Add(CustomChars[random.Next(0, CustomChars.Length)]);
                        CharsList = CharsList.Distinct().ToList();
                    }
                    foreach (char chars in CharsList)
                    {
                        RandomsString += chars;
                    }
                }
                else
                {
                    for (int i = 0; i < length; i++)
                    {
                        RandomsString += CustomChars[random.Next(0, CustomChars.Length)];
                    }
                }
            }
            else
            {
                throw new Exception();
            }
            return RandomsString;
        }
        private string RanStrGen(char[] CustomChars,int Length,int Mode=0)
        {
            return RanStrGen(CustomChars, Length, Length, Mode);
        }
    }
}

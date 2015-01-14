using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace PrestageCommonMethodCleanerHelpers
{
    public class Helpers
    {
        public static double GetStringPercentDifference(string sA, string sB)
        {
            Dictionary<char, int> s1CharCount = new Dictionary<char, int>();
            Dictionary<char, int> s2CharCount = new Dictionary<char, int>();


            CollectCharacterOccurances(sA, sB, s1CharCount, s2CharCount);

            return CalculateStringPercentDifference(s1CharCount, s2CharCount);
        }

        private static double CalculateStringPercentDifference(Dictionary<char, int> s1CharCount, Dictionary<char, int> s2CharCount)
        {
            char[] usedChars = s1CharCount.Keys.ToArray();
            double sumOfPercents = 0;
            for (int i = 0; i < usedChars.Length; i++)
            {
                int characterMaxOccurance = 0;
                int characterMinOccurance = 0;
                if (s1CharCount[usedChars[i]] > s2CharCount[usedChars[i]])
                {
                    characterMaxOccurance = s1CharCount[usedChars[i]];
                    characterMinOccurance = s2CharCount[usedChars[i]];
                }
                else
                {
                    characterMinOccurance = s1CharCount[usedChars[i]];
                    characterMaxOccurance = s2CharCount[usedChars[i]];
                }
                int difference = characterMaxOccurance - characterMinOccurance;
                double characterPercentDifference = (double)difference / (double)characterMaxOccurance;
                sumOfPercents += characterPercentDifference;
            }

            return sumOfPercents / usedChars.Length;
        }

        private static void CollectCharacterOccurances(string sA, string sB, Dictionary<char, int> s1CharCount, Dictionary<char, int> s2CharCount)
        {
            string longString = "";
            string shortString = "";
            if (sA.Length > sB.Length)
            {
                longString = sA;
                shortString = sB;
            }
            else
            {
                longString = sB;
                shortString = sA;
            }

            for (int i = 0; i < longString.Length; i++)
            {
                if (i < shortString.Length)
                {
                    char currentCharShort = shortString[i];
                    if (s1CharCount.ContainsKey(currentCharShort))
                    {
                        s1CharCount[currentCharShort]++;
                    }
                    else
                    {
                        s1CharCount.Add(currentCharShort, 1);
                        if (!s2CharCount.ContainsKey(currentCharShort))
                        {
                            s2CharCount.Add(currentCharShort, 0);
                        }
                    }

                }

                char currentCharLong = longString[i];
                if (s2CharCount.ContainsKey(currentCharLong))
                {
                    s2CharCount[currentCharLong]++;
                }
                else
                {
                    s2CharCount.Add(currentCharLong, 1);
                    if (!s1CharCount.ContainsKey(currentCharLong))
                    {
                        s1CharCount.Add(currentCharLong, 0);
                    }
                }
            }
        }


    }
}

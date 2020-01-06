using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace FasTest.Validation
{
    public class InputValidation
    {
        
        Regex alphaNum = new Regex(@"[^\w\s]+$");
        Regex NumSpace = new Regex(@"[^0-9# ]+$");
        Regex Num = new Regex(@"[^0-9]+$");
        Regex alpha = new Regex(@"[^a-zA-Z]+$");
        Regex alphaSpace = new Regex(@"[^a-zA-Z\w]+$");
        Regex alphaNumSpace = new Regex(@"[^a-zA-Z0-9\w]+$");


        public InputValidation()
        {
            
        }
        
        public bool VAlphaNum(string input)
        {
            bool isTrue = true;
            
            foreach (char c in input)
            {
                if (!char.IsLetterOrDigit(c))
                {
                    isTrue = false;
                }
            }
            return isTrue;
        }

        public bool VAlphaNumSpace(string input)
        {
            bool isTrue;

            if (alphaNumSpace.IsMatch(input))
                isTrue = true;
            else
                isTrue = false;

            return isTrue;
        }

        public bool VNumSpace(string input)
        {
            bool isTrue;

            if (NumSpace.IsMatch(input))
                isTrue = true;
            else
                isTrue = false;

            return isTrue;
        }

        public bool VAlphaSpace(string input)
        {
            bool isTrue;

            if (alphaSpace.IsMatch(input))
                isTrue = true;
            else
                isTrue = false;

            return isTrue;

        }

        public bool VAlpha(string input)
        {
            bool isTrue;

            if (alpha.IsMatch(input))
                isTrue = true;
            else
                isTrue = false;

            return isTrue;

        }

        public bool VNum(string input)
        {
            bool isTrue;

            if (Num.IsMatch(input))
                isTrue = true;
            else
                isTrue = false;

            return isTrue;

        }
    }
}
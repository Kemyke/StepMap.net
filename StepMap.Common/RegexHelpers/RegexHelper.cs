using StepMap.Logger.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StepMap.Common.RegexHelpers
{
    public class RegexHelper : IRegexHelper
    {
        private readonly ILogger logger;
        public RegexHelper(ILogger logger)
        {
            this.logger = logger;
        }

        public bool IsValidEmail(string strIn)
        {
            if (String.IsNullOrEmpty(strIn))
            {
                throw new ArgumentNullException("strIn");
            }
               
            try
            {
                strIn = DomainMapper(strIn);
            }
            catch (ArgumentException ex)
            {
                logger.Error(ex.ToString());
                return false;
            }
            catch (RegexMatchTimeoutException ex)
            {
                logger.Error(ex.ToString());
                return false;
            }

            try
            {
                return Regex.IsMatch(strIn, @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" + @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException ex)
            {
                logger.Error(ex.ToString());
                return false;
            }
        }

        private string DomainMapper(string strIn)
        {
            string ret = Regex.Replace(strIn, @"(@)(.+)$", (match) =>
            {
                IdnMapping idn = new IdnMapping();

                string domainName = match.Groups[2].Value;
                    domainName = idn.GetAscii(domainName);
                return match.Groups[1].Value + domainName;
            }, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            return ret;
        }
    }
}

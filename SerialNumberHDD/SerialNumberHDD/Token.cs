using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anh.License
{
    public class TokenUtil
    {
        /// <summary>
        /// 'A' = 65, 'z'=90,
        /// 'a' = 97, 'z'=122,
        /// </summary>
        /// <returns></returns>
        public static string CreateToken()
        {
            StringBuilder sbRES = new StringBuilder();
            Random r = new Random(65);
            Random r1 = new Random(0);
            for (int i = 0; i < 1024; i++)
            {
                int k = r.Next(65, 91);
                bool lower = r1.Next(0, 2) > 0 ? true : false;
                if (lower)
                {
                    k += 32;
                }
                char ck = (char)k;
                sbRES.Append(ck.ToString());
            }

            return sbRES.ToString();
        }

        public const string DefaultToken = "rxflOjaBaGEFiHmQehlMluzTbivznLEOLqipWRMwkuIQeaLMMVZawSFxxmjfoBopBpUsyXtNvocRmjOaWImRmEssklrKigIIfSOOgtnpBlFhqNKOxiIFpwDxXBhMGieoGtLtuZUDvwQaZlBwbcnzXDtJprREmajsgQaXbquxofKTGCXLAKLDVFqDaCechPrUnxtJKdukIPlStadmTfEbPGXbZXXzMrpyOPFijobWPAVfTyKAZriClqFhARJMgVyqzMedUudFEhLokrHbnxaUJnnYkYirfpqUceFHuGfEODyDeqwJKIZivegMyGsVMLaoQQLGLaggrVfgpJtTcZHWPizxxTiNNkScdjnNEPXJbsqZyaWOmzsneDZhrOirRTSGdDCGwfoMBXuwrAZWWuaoSEYZJFwQhFSIakVuYQsHkFlgIcqRoLvxsDozfDwRjXFvFmWheEyGctEnsRqhpFcWKuQWvwpUuBYMzqbwFimJmpvYZBgoMpbdvJkkVZLIpEmbvrncpubgsSjvzRWuqBIfzURqFRaGwEuIDUDINdyRnvKdApkedRSnVgbAZWxyUVJOXASfoogmCqReJoQPgwCFLHMwSDIrGZxSlVfZWfQPkFkFpxQlgHSxSecQnwnkFpgdSzsNiRtbfNuGwHgUmXKzHAZHSPLARFjvzhMPFCdPOQSEJwtgPSyIEhVSZlutfsjADygoUqPjtvkQhUXWvfLzgbrsMuqTHKjvHraLPFLYCooIWEKEOYXaHKtcHoCAAvcJMojqTlGQtMWsNIwHjLFjmioWXYDBcLyGMEWhvUlcSgGuoBpZRuqpqSxDaMsVifoIOpFIlpmPACQGCtIWjEJiuIqThtgIbpUNSYgywgNCIAPNcRBgEWMDvfnFZLBRIeOkIDNEMjZCQgaUtCRQszeFdqrMKBzKFvUDMeFyDxuuWJNOFTLwTLzIzjgsMaTLkPXUMWIPXtMVFKIuDcGQqDQtWLdCxNMCdijHLScjlWZaYecPaMpjudUhyDYaSFRjHJsw";
    }

    public class KeyUtil
    {
        /// <summary>
        /// 'A' = 65, 'z'=90,
        /// 'a' = 97, 'z'=122,
        /// </summary>
        /// <returns></returns>
        public static string CreateProductKey()
        {
            StringBuilder sbRES = new StringBuilder();
            Random r = new Random();
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int k = r.Next(0, 10);
                    sbRES.Append(k.ToString());
                }
                if (i < 5)
                {
                    sbRES.Append("-");
                }
            }

            return sbRES.ToString();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    public interface ITermsService
    {
        List<string> GetPossibleDailyTerms();
        List<string> GetTermsWithinTwoHours();
        List<string> GetTermsAfterTwoHours(List<string> allPossibleTerms);
        DateTime CreateTime(string term);
        List<string> GetAvailableTerms(int doctorID, DateTime when);

    }
}

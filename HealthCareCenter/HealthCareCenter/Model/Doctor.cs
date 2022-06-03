using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class Doctor : User
    {
        public string Type { get; set; }
        public List<int> VacationRequestIDs { get; set; }
        public List<int> MedicineCreationRequestIDs { get; set; }
        public List<double> Ratings { get; set; }

        public double GetAverageRating()
        {
            double average = 0.0;
            foreach (double rating in Ratings)
            {
                average += rating;
            }

            return average / Ratings.Count;
        }
    }
}
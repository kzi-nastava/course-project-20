using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public class HospitalRoomNotFound : Exception
    {
        /// <summary>
        ///
        /// </summary>
        public HospitalRoomNotFound()
        {
            Console.WriteLine("Error, hospital room not found!");
        }
    }
}
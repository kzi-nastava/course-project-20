using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Model
{
    public static class Constants
    {
        public static string DateTimeFormat = "dd/MM/yyyy HH:mm";
        public static string DateFormat = "dd/MM/yyyy";
        public static string TimeFormat = "HH:mm";
        public static int StartWorkTime = 8;
        public static int EndWorkTime = 21;

        public static List<string> DynamicEquipment = new List<string>() { "Gauze", "Buckle", "Band aid", "Syringe", "Paper", "Pen"};

        public static int Second = 1000;
        public static int Minute = 60 * Second;
    }
}

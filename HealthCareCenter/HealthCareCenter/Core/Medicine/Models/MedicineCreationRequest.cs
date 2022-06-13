using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Core.Medicine.Repositories;

namespace HealthCareCenter.Core.Medicine.Models
{
    public class MedicineCreationRequest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public List<string> Ingredients { get; set; }
        public string Manufacturer { get; set; }
        public RequestState State { get; set; }
        public string DenyComment { get; set; }

        public MedicineCreationRequest()
        {
        }

        public MedicineCreationRequest(int id, string name, List<string> ingredients, string manufacturer, RequestState state, string denyComment = "")
        {
            ID = id;
            Name = name;
            Ingredients = ingredients;
            Manufacturer = manufacturer;
            State = state;
            DenyComment = denyComment;
        }

        public MedicineCreationRequest(string name, List<string> ingredients, string manufacturer, RequestState state, string denyComment = "")
        {
            ID = MedicineCreationRequestRepository.GetLargestId() + 1;
            Name = name;
            Ingredients = ingredients;
            Manufacturer = manufacturer;
            State = state;
            DenyComment = denyComment;
        }
    }
}
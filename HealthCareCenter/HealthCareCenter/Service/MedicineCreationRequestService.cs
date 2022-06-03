using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    internal class MedicineCreationRequestService
    {
        public static MedicineCreationRequest getMedicineCreationRequest(int id)
        {
            foreach(MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
            {
                if (id == request.ID)
                    return request;
            }
            return null;
        }
        public static string GetIngredients(MedicineCreationRequest request)
        {
            string ingredients = "";
            foreach (string ingredient in request.Ingredients)
            {
                ingredients += ingredient + ",";
            }
            if(ingredients.Length > 0)
                return ingredients.Substring(0, ingredients.Length - 1);
            return ingredients;
        }
    }
}

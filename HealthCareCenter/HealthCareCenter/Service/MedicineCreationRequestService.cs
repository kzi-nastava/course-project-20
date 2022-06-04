using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    internal class MedicineCreationRequestService
    {
        public static MedicineCreationRequest GetMedicineCreationRequest(int id)
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
        public static MedicineCreationRequest Get(int id)
        {
            try
            {
                foreach (MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
                {
                    if (request.ID == id)
                    {
                        return request;
                    }
                }

                throw new MedicineNotFound();
            }
            catch (MedicineNotFound ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<MedicineCreationRequest> GetMedicines()
        {
            return MedicineCreationRequestRepository.Requests;
        }

        public static void Add(MedicineCreationRequest newMedicineCreationRequest)
        {
            MedicineCreationRequestRepository.Requests.Add(newMedicineCreationRequest);
            MedicineCreationRequestRepository.Save();
        }

        public static bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < MedicineCreationRequestRepository.Requests.Count; i++)
                {
                    if (id == MedicineCreationRequestRepository.Requests[i].ID)
                    {
                        MedicineCreationRequestRepository.Requests.RemoveAt(i);
                        MedicineCreationRequestRepository.Save();
                        return true;
                    }
                }
                throw new MedicineNotFound();
            }
            catch (MedicineNotFound ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(MedicineCreationRequest medicineCreationRequest)
        {
            try
            {
                for (int i = 0; i < MedicineCreationRequestRepository.Requests.Count; i++)
                {
                    if (medicineCreationRequest.ID == MedicineCreationRequestRepository.Requests[i].ID)
                    {
                        MedicineCreationRequestRepository.Requests.RemoveAt(i);
                        MedicineCreationRequestRepository.Save();
                        return true;
                    }
                }
                throw new MedicineNotFound();
            }
            catch (MedicineNotFound ex)
            {
                Console.WriteLine(ex);
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


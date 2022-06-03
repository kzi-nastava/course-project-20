using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Model;

namespace HealthCareCenter.Service
{
    internal class MedicineCreationRequestService
    {
        public static Medicine Get(int id)
        public static MedicineCreationRequest getMedicineCreationRequest(int id)
        {
            try
            foreach(MedicineCreationRequest request in MedicineCreationRequestRepository.Requests)
            {
                foreach (Medicine medicine in MedicineCreationRequestRepository.Medicines)
                {
                    if (medicine.ID == id)
                    {
                        return medicine;
                if (id == request.ID)
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
        public static string GetIngredients(MedicineCreationRequest request)
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Medicine> GetMedicines()
            string ingredients = "";
            foreach (string ingredient in request.Ingredients)
        {
                ingredients += ingredient + ",";
            return MedicineCreationRequestRepository.Medicines;
        }
            if(ingredients.Length > 0)
                return ingredients.Substring(0, ingredients.Length - 1);
            return ingredients;

        public static void Add(Medicine newMedicine)
        {
            MedicineCreationRequestRepository.Medicines.Add(newMedicine);
            MedicineCreationRequestRepository.Save();
        }

        public static int GetLargestId()
        {
            try
            {
                List<Medicine> medicines = MedicineCreationRequestRepository.Medicines;
                medicines.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (medicines.Count == 0)
                {
                    return -1;
                }

                return medicines[medicines.Count - 1].ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < MedicineCreationRequestRepository.Medicines.Count; i++)
                {
                    if (id == MedicineCreationRequestRepository.Medicines[i].ID)
                    {
                        MedicineCreationRequestRepository.Medicines.RemoveAt(i);
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

        public static bool Delete(Medicine medicine)
        {
            try
            {
                for (int i = 0; i < MedicineCreationRequestRepository.Medicines.Count; i++)
                {
                    if (medicine.ID == MedicineCreationRequestRepository.Medicines[i].ID)
                    {
                        MedicineCreationRequestRepository.Medicines.RemoveAt(i);
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
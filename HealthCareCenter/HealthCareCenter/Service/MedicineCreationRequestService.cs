using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    internal class MedicineCreationRequestService
    {
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

        public static int GetLargestId()
        {
            try
            {
                List<MedicineCreationRequest> requests = MedicineCreationRequestRepository.Requests;
                requests.Sort((x, y) => x.ID.CompareTo(y.ID));
                if (requests.Count == 0)
                {
                    return -1;
                }

                return requests[requests.Count - 1].ID;
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
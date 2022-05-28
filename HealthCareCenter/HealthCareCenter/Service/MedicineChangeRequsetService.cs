using HealthCareCenter.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Service
{
    internal class MedicineChangeRequsetService
    {
        public static MedicineChangeRequest Get(int id)
        {
            try
            {
                foreach (MedicineChangeRequest request in MedicineChangeRequestRepository.Requests)
                {
                    if (request.Medicine.ID == id)
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

        public static List<MedicineChangeRequest> GetMedicines()
        {
            return MedicineChangeRequestRepository.Requests;
        }

        public static void Add(MedicineChangeRequest newChangeRequest)
        {
            MedicineChangeRequestRepository.Requests.Add(newChangeRequest);
            MedicineChangeRequestRepository.Save();
        }

        public static int GetLargestId()
        {
            try
            {
                List<MedicineChangeRequest> requests = MedicineChangeRequestRepository.Requests;
                requests.Sort((x, y) => x.Medicine.ID.CompareTo(y.Medicine.ID));
                if (requests.Count == 0)
                {
                    return -1;
                }

                return requests[requests.Count - 1].Medicine.ID;
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
                for (int i = 0; i < MedicineChangeRequestRepository.Requests.Count; i++)
                {
                    if (id == MedicineChangeRequestRepository.Requests[i].Medicine.ID)
                    {
                        MedicineChangeRequestRepository.Requests.RemoveAt(i);
                        MedicineChangeRequestRepository.Save();
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

        public static bool Delete(MedicineChangeRequest request)
        {
            try
            {
                for (int i = 0; i < MedicineChangeRequestRepository.Requests.Count; i++)
                {
                    if (request.Medicine.ID == MedicineChangeRequestRepository.Requests[i].Medicine.ID)
                    {
                        MedicineChangeRequestRepository.Requests.RemoveAt(i);
                        MedicineChangeRequestRepository.Save();
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
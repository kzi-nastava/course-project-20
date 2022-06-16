using System;
using System.Collections.Generic;
using System.Text;
using HealthCareCenter.Core.Exceptions;
using HealthCareCenter.Core.Medicine.Models;
using HealthCareCenter.Core.Medicine.Repositories;

namespace HealthCareCenter.Core.Medicine.Services
{
    public class MedicineCreationRequestService : IMedicineCreationRequestService
    {
        private readonly BaseMedicineCreationRequestRepository _medicineCreationRequestRepository;

        public MedicineCreationRequestService(BaseMedicineCreationRequestRepository medicineCreationRequestRepository)
        {
            _medicineCreationRequestRepository = medicineCreationRequestRepository;
        }

        public MedicineCreationRequest GetMedicineCreationRequest(int id)
        {
            foreach (MedicineCreationRequest request in _medicineCreationRequestRepository.Requests)
            {
                if (id == request.ID)
                    return request;
            }
            return null;
        }

        public string GetIngredients(MedicineCreationRequest request)
        {
            string ingredients = "";
            foreach (string ingredient in request.Ingredients)
            {
                ingredients += ingredient + ",";
            }
            if (ingredients.Length > 0)
                return ingredients.Substring(0, ingredients.Length - 1);
            return ingredients;
        }

        public MedicineCreationRequest Get(int id)
        {
            try
            {
                foreach (MedicineCreationRequest request in _medicineCreationRequestRepository.Requests)
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

        public List<MedicineCreationRequest> GetMedicines()
        {
            return _medicineCreationRequestRepository.Requests;
        }

        public void Add(MedicineCreationRequest newMedicineCreationRequest)
        {
            _medicineCreationRequestRepository.Requests.Add(newMedicineCreationRequest);
            _medicineCreationRequestRepository.Save();
        }

        public bool Delete(int id)
        {
            try
            {
                for (int i = 0; i < _medicineCreationRequestRepository.Requests.Count; i++)
                {
                    if (id == _medicineCreationRequestRepository.Requests[i].ID)
                    {
                        _medicineCreationRequestRepository.Requests.RemoveAt(i);
                        _medicineCreationRequestRepository.Save();
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

        public bool Delete(MedicineCreationRequest medicineCreationRequest)
        {
            try
            {
                for (int i = 0; i < _medicineCreationRequestRepository.Requests.Count; i++)
                {
                    if (medicineCreationRequest.ID == _medicineCreationRequestRepository.Requests[i].ID)
                    {
                        _medicineCreationRequestRepository.Requests.RemoveAt(i);
                        _medicineCreationRequestRepository.Save();
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
using HealthCareCenter.Exceptions;
using HealthCareCenter.Model;
using HealthCareCenter.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Controller
{
    public abstract class BaseMedicineCreationRequestController
    {
        public List<string> AddedIngrediens { get; set; }

        public BaseMedicineCreationRequestController()
        {
            AddedIngrediens = new List<string>();
        }

        public virtual void AddIngredient(string ingredient)
        {
            IsValideIngredientForAdding(ingredient);
            AddedIngrediens.Add(ingredient);
        }

        public virtual void RemoveIngredient(string ingredient)
        {
            IsValideIngredientForRemoving(ingredient);
            AddedIngrediens.Remove(ingredient);
        }

        public abstract void Send(string medicineName, string medicineManufacturer);

        protected virtual bool IsValideIngredientName(string ingredient)
        {
            return ingredient.Trim() != "";
        }

        protected virtual bool IsIngridientAlreadyAdded(string ingredient)
        {
            return AddedIngrediens.Contains(ingredient);
        }

        protected void IsValideIngredientForAdding(string ingredient)
        {
            if (!IsValideIngredientName(ingredient)) { throw new InvalideIngredientNameException(ingredient); }

            if (IsIngridientAlreadyAdded(ingredient)) { throw new IngredientAlreadyAddedException(ingredient); }
        }

        protected void IsValideIngredientForRemoving(string ingredient)
        {
            if (!IsValideIngredientName(ingredient)) { throw new InvalideIngredientNameException(ingredient); }

            if (!IsIngridientAlreadyAdded(ingredient)) { throw new IngredientNotFoundException(ingredient); }
        }

        protected bool IsValideMedicineName(string name)
        {
            return name.Trim() != "";
        }

        protected bool IsValideMedicineManufacturer(string manufacturer)
        {
            return manufacturer.Trim() != "";
        }

        protected bool DrugHasIngredient()
        {
            return AddedIngrediens.Count != 0;
        }

        protected void IsPossibleToCreateMedicineCreationRequest(string name, string manfacturer)
        {
            if (!IsValideMedicineName(name))
            {
                throw new InvalideMedicineNameException(name);
            }

            if (!IsValideMedicineManufacturer(manfacturer))
            {
                throw new InvalideMedicineManugacturerException(manfacturer);
            }

            if (!DrugHasIngredient())
            {
                throw new IngredientsNotFoundException();
            }
        }
    }
}
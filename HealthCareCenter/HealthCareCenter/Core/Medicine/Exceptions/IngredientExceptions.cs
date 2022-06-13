using System;
using System.Collections.Generic;
using System.Text;

namespace HealthCareCenter.Core.Medicine.Exceptions
{
    public class InvalideIngredientNameException : Exception
    {
        public InvalideIngredientNameException(string ingredient) : base($"Invalide ingredient name={ingredient}!")
        {
        }
    }

    public class IngredientAlreadyAddedException : Exception
    {
        public IngredientAlreadyAddedException(string ingredient) : base($"Ingredient {ingredient} is already added!")
        {
        }
    }

    public class IngredientNotFoundException : Exception
    {
        public IngredientNotFoundException(string ingredient) : base($"Ingredient {ingredient} not found!")
        {
        }
    }
}
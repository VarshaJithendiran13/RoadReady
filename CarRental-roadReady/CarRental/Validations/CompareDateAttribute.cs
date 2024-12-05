using System;
using System.ComponentModel.DataAnnotations;
using CarRental.Models;

namespace CarRental.Validations
{
    public class CompareDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            // Ensure value is a valid DateOnly
            if (value is DateOnly dropoffDate)
            {
                // Access the PickupDate from the object being validated
                var objectInstance = validationContext.ObjectInstance;

                if (objectInstance == null)
                {
                    // If the object instance is null, validation cannot proceed
                    return new ValidationResult("The object being validated is null.");
                }

                // Use reflection to access the PickupDate property
                var pickupDateProperty = objectInstance.GetType().GetProperty("PickupDate");

                // If PickupDate property exists, get its value
                if (pickupDateProperty != null && pickupDateProperty.GetValue(objectInstance) is DateOnly pickupDate)
                {
                    // Compare dropoffDate with pickupDate
                    if (dropoffDate > pickupDate)
                    {
                        return ValidationResult.Success;
                    }
                    else
                    {
                        return new ValidationResult("Drop-off date must be after the pickup date.");
                    }
                }
                else
                {
                    return new ValidationResult("PickupDate property is not found or is not a DateOnly.");
                }
            }

            return new ValidationResult("Invalid date format for drop-off date.");
        }
    }
}
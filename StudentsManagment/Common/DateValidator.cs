using System;
using System.ComponentModel.DataAnnotations;

namespace StudentsManagment.Common
{
    public class DateValidator : ValidationAttribute
    {
        /*
            Function that check the date the user insert relation to the current date
            if the difference (calculated by years) is less than 5 return false 
            otherwise return true.
        */
        public override bool IsValid(object value)
        {
            DateTime dateTime = Convert.ToDateTime(value);
            int diff = DateTime.Now.Year - dateTime.Year;
            if (diff >= 5)
            {
                return true;
            }else
            {
                return false;
            }
        }
    }
}
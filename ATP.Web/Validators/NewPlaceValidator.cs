using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ATP.Web.Resources;

namespace ATP.Web.Validators
{
    public class NewPlaceValidator : BaseValidator<Place>
    {
        public override List<Error> Validate(Resource resource)
        {
            var place = resource as Place;
            if (place == null) throw new ArgumentException();

            if (string.IsNullOrEmpty(place.Name))
            {
                Errors.Add(
                    new Error
                    {
                        Field = "Name",
                        Message = "Name required",
                        Code = ErrorCode.Missing,
                        Resource = place.Uri
                    });
            }
            return Errors;
        }
    }
}
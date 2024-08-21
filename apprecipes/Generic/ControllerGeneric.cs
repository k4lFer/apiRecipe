using apprecipes.DataTransferObject.OtherObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace apprecipes.Generic
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class ControllerGeneric<So> : ControllerBase
    {
        public So _so;

        public ControllerGeneric()
        {
            _so = (So)Activator.CreateInstance(typeof(So), new object[] {});
        }

        protected DtoMessage ValidateDto(object dto, List<string> listField)
        {
            DtoMessage dtoMessage = new DtoMessage();
            List<string> errors = new List<string>();
    
            ModelState.Clear();
            TryValidateModel(dto);

            foreach (string fieldName in listField)
            {
                var propertyInfo = dto.GetType().GetProperty(fieldName);
                if (propertyInfo != null)
                {
                    var value = propertyInfo.GetValue(dto);

                    if (propertyInfo.PropertyType == typeof(Guid))
                    {
                        if ((Guid)value == Guid.Empty)
                        {
                            errors.Add($"El campo {fieldName} es obligatorio.");
                        }
                    }
                }

                ModelState.TryGetValue(fieldName, out ModelStateEntry modelState);

                if (modelState is not null && modelState.Errors.Count > 0)
                {
                    errors.AddRange(modelState.Errors.Select(er => $"El campo {fieldName} es obligatorio.").ToList());
                }
            }

            if (errors.Count > 0)
            {
                dtoMessage.listMessage = errors;
                dtoMessage.Error();
            }

            return dtoMessage;
        }
    }
}

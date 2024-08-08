using apprecipes.DataTransferObject.OtherObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace apprecipes.Generic
{
    //[Authorize]
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
            ModelState.Clear();
            TryValidateModel(dto);
            foreach (string item in listField)
            {
                ModelStateEntry modelStateEntry;
                ModelState.TryGetValue(item, out modelStateEntry);
                if (modelStateEntry != null && modelStateEntry.Errors.Count > 0)
                {
                    dtoMessage.listMessage.AddRange(modelStateEntry.Errors.Select(S => S.ErrorMessage).ToList());
                }
            }
            if (dtoMessage.ExistsMessage())
            {
                dtoMessage.Error();
            }
            return dtoMessage;
        }
    }
}

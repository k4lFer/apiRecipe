using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoLike
    {
        public Guid idRecipe { get; set; }       
        public Guid idUser { get; set; }   
    }
}

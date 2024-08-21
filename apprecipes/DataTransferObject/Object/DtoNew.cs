using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using apprecipes.DataAccess.Generic;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoNew : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }   
        public Guid idRecipe { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string content { get; set; }
        public bool status { get; set; }
    }
}

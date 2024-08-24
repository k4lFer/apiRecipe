using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using apprecipes.DataTransferObject.ObjectGeneric;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoVideo : DtoDateGeneric
    {
        public Guid id { get; set; }
        public Guid idRecipe { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string description { get; set; }
        
        #region parents
        public DtoRecipe recipe { get; set; }
        #endregion
    }
}

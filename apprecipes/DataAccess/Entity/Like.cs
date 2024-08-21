using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apprecipes.DataAccess.Entity
{
    public class Like
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid idRecipe { get; set; }       
        //[Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid idUser { get; set; }       
    }
}


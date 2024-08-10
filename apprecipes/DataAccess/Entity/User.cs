using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using apprecipes.DataAccess.Generic;

namespace apprecipes.DataAccess.Entity
{
    public class User : DateGeneric
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid id { get; set; }
        public Guid idAthentication { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        
        public Authentication ChildAthentication { get; set; }
    }
}

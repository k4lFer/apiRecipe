using apprecipes.DataTransferObject.ObjectGeneric;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoUser : DtoDateGeneric
    {
        public Guid id { get; set; }
        public Guid idAthentication { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }

        public DtoAuthentication ChildDtoAthentication { get; set; } = null!;
    }
}

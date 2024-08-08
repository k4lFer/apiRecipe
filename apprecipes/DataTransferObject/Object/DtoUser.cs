namespace apprecipes.DataTransferObject.Object
{
    public class DtoUser
    {
        public Guid id { get; set; }
        public Guid idAthentication { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public DateTime createdAt { get; set; }
        public DateTime updatedAt { get; set; }

        public DtoAuthentication ChildDtoAthentication { get; set; } = null!;
    }
}

using apprecipes.DataTransferObject.EnumObject;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoAuthentication
    {
        public Guid id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public Role role { get; set; }
        public bool status { get; set; }
    }
}

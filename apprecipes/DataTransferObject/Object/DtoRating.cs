using apprecipes.DataTransferObject.ObjectGeneric;

namespace apprecipes.DataTransferObject.Object
{
    public class DtoRating : DtoDateGeneric
    {
        public Guid id { get; set; }
        public Guid idRecipe { get; set; }
        public string comment { get; set; }
        public long numberLike { get; set; }
    }
}

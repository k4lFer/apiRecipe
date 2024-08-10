using apprecipes.DataTransferObject.OtherObject;

namespace apprecipes.Generic
{
    public class SoGeneric<Dto>
    {
        public SoGeneric()
        {
            mo = new DtoMessage();
        }

        public DtoMessage mo { get; set; }
        public Dto dto { get; set; }
        public ICollection<Dto> listDto { get; set; }
    }
}

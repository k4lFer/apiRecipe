using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.OtherObject;

namespace apprecipes.ServerObjet
{
    public class SoAuthentication
    {
        public SoAuthentication()
        {
            mo = new DtoMessage();
            tokens = new Tokens();
        }

        public DtoMessage mo { get; set; }
        public Tokens tokens { get; set; }
        public DtoAuthentication authetication { get; set; }
    }
}

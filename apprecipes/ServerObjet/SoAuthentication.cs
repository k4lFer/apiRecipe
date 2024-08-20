using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.OtherObject;
using apprecipes.Generic;

namespace apprecipes.ServerObjet
{
    public class SoAuthentication : SoGeneric<DtoAuthentication>
    {
        public SoAuthentication()
        {
            data.additional = new Tokens();
        }
    }
}

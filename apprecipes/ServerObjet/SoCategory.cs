using apprecipes.DataTransferObject.Object;
using apprecipes.DataTransferObject.OtherObject;
using apprecipes.Generic;

namespace apprecipes.ServerObjet
{
    public class SoCategory : SoGeneric<DtoCategory>
    {
        public SoCategory()
        {
            data.additional = new Pagination();
        }
    }
}

namespace apprecipes.DataTransferObject.OtherObject
{
    public class DtoMessage
    {
        public List<string> listMessage { get; set; }
        public string type { get; set; }

        public DtoMessage() 
        {
            listMessage = new List<string>();
        }

        public bool ExistsMessage()
        {
            return listMessage.Count > 0;
        }

        public void Success()
        {
            type = "success";
        }

        public void Warning()
        {
            type = "warnig";
        }

        public void Error()
        {
            type = "error";
        }   
    }
}

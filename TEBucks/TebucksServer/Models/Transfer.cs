namespace TEbucksServer.Models
{
    public class Transfer
    {
        public int Transfer_Id { get; set; }
        public int Transfer_Type_Id { get; set; }
        public int Transfer_Status_Id { get; set; }
        public int Account_From { get; set; }
        public int Account_To { get; set; }
        public decimal Amount { get; set; }
        public bool Approved
        {
            get
            {
                return Transfer_Status_Id == 2;
            }
        }
        public bool Rejected
        {
            get
            {
                return Transfer_Status_Id == 3;
            }
        }
        public bool Pending
        {
            get
            {
                return Transfer_Status_Id == 1;
            }
        }
        public bool RequestType
        {
            get
            {
                return Transfer_Type_Id == 1;
            }
        }
        public bool SendType
        {
            get
            {
                return Transfer_Type_Id == 2;
            }
        }
    }
}

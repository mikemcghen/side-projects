namespace TEbucksServer.DTO
{
    public class NewTransferDto
    {
        public int Transfer_Type_Id { get; set; }
        public int Transfer_Status_Id { get; set; }
        public int Account_From { get; set; }
        public int Account_To { get; set; }
        public decimal Amount { get; set; }
    }
}

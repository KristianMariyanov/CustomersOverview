namespace CustomerOverview.Web.Mvc.Models.Orders
{
    public class OrderViewModel
    {
        public decimal TotalPrice{ get; set; }

        public int TotalUnits { get; set; }

        public bool PossibleProblemWithExecution { get; set; }
    }
}

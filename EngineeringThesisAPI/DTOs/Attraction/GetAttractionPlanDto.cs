namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class GetAttractionPlanDto
    {
        public List<GetAttractionsDto> attractions { get; set; }
        public List<LocationSummaryDto> locationSummary { get; set; }
        public TimeSpan totalTime { get; set; }
        public float totalPrice { get; set; }
    }
}

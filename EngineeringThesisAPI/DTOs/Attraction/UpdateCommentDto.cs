﻿namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class UpdateCommentDto
    {
        public int AttractionId { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
    }
}

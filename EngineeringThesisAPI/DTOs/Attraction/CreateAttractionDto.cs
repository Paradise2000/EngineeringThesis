namespace EngineeringThesisAPI.DTOs.Attraction
{
    public class CreateAttractionDto
    {
        public string City { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }
        public double CoordinateZ { get; set; }
        public List<string> ImagesPaths { get; set; }
        public string MainImagePath { get; set; }
    }
}


/*
            bool isMainPhotoPicked = false;

            var attraction = _mapper.Map<Attraction>(dto);

            attraction.UserId = _userIdProvider.GetUserId();

            _context.Attractions.Add(attraction);

            _context.SaveChanges(); //rozwiązanie tymczasowe

            foreach (var file in dto.ImagesPaths)
            {
                var record = _context.FilePaths.FirstOrDefault(r => r.FileName == file && r.UserId == _userIdProvider.GetUserId());

                if (record == null)
                {
                    _context.Remove(attraction); //rozwiązanie tymczasowe
                    return BadRequest("File name error");
                }

                if(file == dto.MainImagePath)
                {
                    attraction.MainPhotoId = record.Id;
                    isMainPhotoPicked = true;
                }

                record.AttractionId = attraction.Id;
            }

            if(isMainPhotoPicked == false)
            {
                _context.Remove(attraction); //rozwiązanie tymczasowe
                return BadRequest("File name error");
            }

            _context.SaveChanges();

            return Ok();
*/
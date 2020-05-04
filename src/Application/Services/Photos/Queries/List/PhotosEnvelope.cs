using System.Collections.Generic;

namespace Application.Services.Photos.Queries.List
{
    public class PhotosEnvelope
    {
        public List<PhotoDto> Photos { get; set; }
        public int PhotosCount { get; set; }
    }
}
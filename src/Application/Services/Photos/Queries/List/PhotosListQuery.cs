using MediatR;

namespace Application.Services.Photos.Queries.List
{
    public class PhotosListQuery : IRequest<PhotosEnvelope>
    {
        public string Username { get; set; }
        public int? Skip { get; set; }
        public int? Limit { get; set; }
    }
}
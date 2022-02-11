using ModelX.Domain.Entities;
using ModelX.Logic.Common.Mappings;

namespace ModelX.Logic.CQRS.Models.Queries.GetModels;

public class GetModelsModelDto : IMapFrom<Model>
{
    public int AttachmentId { get; set; }

    public int Latitude { get; set; }

    public int Longitude { get; set; }
}
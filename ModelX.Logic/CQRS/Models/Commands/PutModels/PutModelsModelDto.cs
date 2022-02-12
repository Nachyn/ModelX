using ModelX.Domain.Entities;
using ModelX.Logic.Common.Mappings;

namespace ModelX.Logic.CQRS.Models.Commands.PutModels;

public record PutModelsModelDto : IMapFrom<Model>
{
    public int AttachmentId { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }
}
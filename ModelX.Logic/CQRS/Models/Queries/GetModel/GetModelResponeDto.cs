using AutoMapper;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.Mappings;

namespace ModelX.Logic.CQRS.Models.Queries.GetModel;

public class GetModelResponeDto : IMapFrom<Model>
{
    public int Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public string Name { get; set; }

    public string GlbModelUrl { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Model, GetModelResponeDto>()
            .ForMember(d => d.Name, o => o.MapFrom(d => d.Attachment.Name))
            .ForMember(d => d.Id, o => o.MapFrom(d => d.AttachmentId))
            .ForMember(d => d.GlbModelUrl, o => o.Ignore());
    }
}
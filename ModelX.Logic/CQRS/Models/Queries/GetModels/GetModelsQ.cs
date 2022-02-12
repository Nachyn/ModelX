using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ModelX.Logic.Common.AppConfigs.Main;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.UserAccessor;

namespace ModelX.Logic.CQRS.Models.Queries.GetModels;

public record GetModelsQ : IRequest<List<GetModelsModelDto>>
{
}

public class GetModelsQHandler : IRequestHandler<GetModelsQ, List<GetModelsModelDto>>
{
    private readonly IAppDbContext _context;

    private readonly EndpointsConfig _endpointsConfig;

    private readonly IMapper _mapper;

    private readonly IUserAccessor _userAccessor;

    public GetModelsQHandler(IAppDbContext context
        , IMapper mapper
        , IUserAccessor userAccessor
        , IOptions<EndpointsConfig> endpointsConfig)
    {
        _context = context;
        _mapper = mapper;
        _userAccessor = userAccessor;
        _endpointsConfig = endpointsConfig.Value;
    }

    public async Task<List<GetModelsModelDto>> Handle(GetModelsQ request
        , CancellationToken cancellationToken)
    {
        var models = await _context.Models.Where(m => m.Attachment.OwnerId == _userAccessor.UserId)
            .ProjectTo<GetModelsModelDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        models.ForEach(m =>
            m.GlbModelUrl = $"{_userAccessor.Host}/{_endpointsConfig.DownloadAttachment}/{m.Id}");

        return models;
    }
}
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using ModelX.Logic.Common.AppConfigs.Main;
using ModelX.Logic.Common.Exceptions.Api;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.UserAccessor;

namespace ModelX.Logic.CQRS.Models.Queries.GetModel;

public class GetModelQ : IRequest<GetModelResponeDto>
{
    public int Id { get; set; }
}

public class GetModelQHandler : IRequestHandler<GetModelQ, GetModelResponeDto>
{
    private readonly IAppDbContext _context;

    private readonly EndpointsConfig _endpointsConfig;

    private readonly IMapper _mapper;

    private readonly IStringLocalizer<ModelsResource> _modelLocalizer;

    private readonly IUserAccessor _userAccessor;

    public GetModelQHandler(IMapper mapper
        , IAppDbContext context
        , IStringLocalizer<ModelsResource> modelLocalizer
        , IOptions<EndpointsConfig> endpointsConfig
        , IUserAccessor userAccessor)
    {
        _mapper = mapper;
        _context = context;
        _modelLocalizer = modelLocalizer;
        _userAccessor = userAccessor;
        _endpointsConfig = endpointsConfig.Value;
    }

    public async Task<GetModelResponeDto> Handle(GetModelQ request
        , CancellationToken cancellationToken)
    {
        var model = await _context.Models
            .Where(m => m.AttachmentId == request.Id
                        && _userAccessor.UserId == m.Attachment.OwnerId)
            .ProjectTo<GetModelResponeDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (model == null)
        {
            throw new NotFoundException(_modelLocalizer["ModelNotFound"]);
        }

        model.GlbModelUrl =
            $"{_userAccessor.Host}/{_endpointsConfig.DownloadAttachment}/{model.Id}";

        return model;
    }
}
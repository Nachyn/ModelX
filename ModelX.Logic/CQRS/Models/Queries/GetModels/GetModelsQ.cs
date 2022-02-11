using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.UserAccessor;

namespace ModelX.Logic.CQRS.Models.Queries.GetModels;

public record GetModelsQ : IRequest<List<GetModelsModelDto>>
{
}

public class GetModelsQHandler : IRequestHandler<GetModelsQ, List<GetModelsModelDto>>
{
    private readonly IAppDbContext _context;

    private readonly IMapper _mapper;

    private readonly IUserAccessor _userAccessor;

    public GetModelsQHandler(IAppDbContext context
        , IMapper mapper
        , IUserAccessor userAccessor)
    {
        _context = context;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }

    public async Task<List<GetModelsModelDto>> Handle(GetModelsQ request
        , CancellationToken cancellationToken)
    {
        return await _context.Models.Where(m => m.Attachment.OwnerId == _userAccessor.UserId)
            .ProjectTo<GetModelsModelDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
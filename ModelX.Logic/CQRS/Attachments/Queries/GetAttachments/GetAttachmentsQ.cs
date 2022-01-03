using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.UserAccessor;

namespace ModelX.Logic.CQRS.Attachments.Queries.GetAttachments;

public record GetAttachmentsQ : IRequest<GetAttachmentsResponseDto>
{
}

public class GetAttachmentsQHandler
    : IRequestHandler<GetAttachmentsQ, GetAttachmentsResponseDto>
{
    private readonly IAppDbContext _context;

    private readonly IMapper _mapper;

    private readonly IUserAccessor _userAccessor;

    public GetAttachmentsQHandler(IAppDbContext context
        , IMapper mapper
        , IUserAccessor userAccessor)
    {
        _context = context;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }

    public async Task<GetAttachmentsResponseDto> Handle(GetAttachmentsQ request
        , CancellationToken cancellationToken)
    {
        return new GetAttachmentsResponseDto
        {
            Attachments = await _context.Attachments
                .Where(a => a.OwnerId == _userAccessor.UserId)
                .ProjectTo<GetAttachmentsAttachmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)
        };
    }
}
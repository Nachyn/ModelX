using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ModelX.Domain.Entities;
using ModelX.Logic.Common.ExternalServices.Database;
using ModelX.Logic.Common.UserAccessor;

namespace ModelX.Logic.CQRS.Models.Commands.PutModels;

public record PutModelsCmd : IRequest<List<PutModelsModelDto>>
{
    public List<PutModelsModelDto> Models { get; set; }
}

public class PutModelsCmdHandler : IRequestHandler<PutModelsCmd, List<PutModelsModelDto>>
{
    private readonly IAppDbContext _context;

    private readonly IMapper _mapper;

    private readonly IUserAccessor _userAccessor;

    public PutModelsCmdHandler(IAppDbContext context
        , IMapper mapper
        , IUserAccessor userAccessor)
    {
        _context = context;
        _mapper = mapper;
        _userAccessor = userAccessor;
    }

    public async Task<List<PutModelsModelDto>> Handle(PutModelsCmd request
        , CancellationToken cancellationToken)
    {
        var updatedModels = new List<PutModelsModelDto>();
        if (request.Models.IsNullOrEmpty())
        {
            return updatedModels;
        }

        var requestModels = request.Models.ToDictionary(m => m.AttachmentId);
        var attachmentIds = await _context.Attachments
            .Where(a => _userAccessor.UserId == a.OwnerId && requestModels.Keys.Contains(a.Id))
            .Select(a => a.Id)
            .ToListAsync(cancellationToken);


        var updateModels = await _context.Models.Where(m => attachmentIds.Contains(m.AttachmentId))
            .ToListAsync(cancellationToken);
        updateModels.ForEach(m =>
        {
            var requestModel = requestModels[m.AttachmentId];
            m.Latitude = requestModel.Latitude;
            m.Longitude = requestModel.Longitude;
        });
        await _context.SaveChangesAsync(CancellationToken.None);


        var newModels = attachmentIds
            .Where(attachmentId => updateModels.All(m => m.AttachmentId != attachmentId))
            .Select(id =>
            {
                var requestModel = requestModels[id];
                return new Model
                {
                    AttachmentId = id,
                    Latitude = requestModel.Latitude,
                    Longitude = requestModel.Longitude
                };
            })
            .ToList();

        _context.Models.AddRange(newModels);
        await _context.SaveChangesAsync(CancellationToken.None);

        updatedModels.AddRange(_mapper.Map<List<PutModelsModelDto>>(updateModels));
        updatedModels.AddRange(_mapper.Map<List<PutModelsModelDto>>(newModels));

        return updatedModels;
    }
}
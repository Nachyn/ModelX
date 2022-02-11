namespace ModelX.Logic.CQRS.Models.Commands.DeleteModels;

public record DeleteModelsResponseDto
{
    public List<int> Ids { get; set; }
}
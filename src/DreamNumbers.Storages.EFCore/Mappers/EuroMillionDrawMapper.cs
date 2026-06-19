using DreamNumbers.Models;

namespace DreamNumbers.Storages.EFCore.Mappers;

internal static class EuroMillionDrawMapper
{
    public static Entities.EuroMillionDraw ToEntity(EuroMillionDraw model)
    {
        return new Entities.EuroMillionDraw
        {
            Id = model.Id,
            Date = model.Date,
            DrawNumber = model.DrawNumber,
            Numbers = model.Numbers,
            Stars = model.Stars,
            ContestNumber = model.ContestNumber
        };
    }

    public static EuroMillionDraw ToModel(Entities.EuroMillionDraw entity)
    {
        return new EuroMillionDraw
        {
            Id = entity.Id,
            Date = entity.Date,
            DrawNumber = entity.DrawNumber,
            Numbers = entity.Numbers,
            Stars = entity.Stars,
            ContestNumber = entity.ContestNumber
        };
    }
}

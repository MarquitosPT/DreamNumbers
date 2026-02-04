using DreamNumbers.Models;

namespace DreamNumbers.Storages.EFCore.Mappers;

internal static class DrawMapper
{
    public static Entities.Draw ToEntity(Draw model)
    {
        return new Entities.Draw
        {
            Id = model.Id,
            Date = model.Date,
            DrawNumber = model.DrawNumber,
            Numbers = model.Numbers, // string.Join(",", model.Numbers),
            DreamNumber = model.DreamNumber
        };
    }

    public static Draw ToModel(Entities.Draw entity)
    {
        return new Draw
        {
            Id = entity.Id,
            Date = entity.Date,
            DrawNumber = entity.DrawNumber,
            Numbers = entity.Numbers,
            //Numbers = entity.Numbers
            //    .Split(',', StringSplitOptions.RemoveEmptyEntries)
            //    .Select(int.Parse)
            //    .ToList(),
            DreamNumber = entity.DreamNumber
        };
    }
}

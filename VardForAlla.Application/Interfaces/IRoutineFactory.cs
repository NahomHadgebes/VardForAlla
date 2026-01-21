using System.Collections.Generic;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Interfaces
{
    public interface IRoutineFactory
    {
        Routine CreateRoutine(
            string title,
            string category,
            string? simpleDescription,
            string? originalDescription,
            IEnumerable<(int order, string simpleText, string? originalText, string? iconKey, string? imageUrl)> steps
        );
    }
}


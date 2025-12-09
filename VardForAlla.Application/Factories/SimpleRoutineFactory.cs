using VardForAlla.Application.Interfaces;
using VardForAlla.Domain.Entities;

namespace VardForAlla.Application.Factories
{
    public class SimpleRoutineFactory : IRoutineFactory
    {
        public Routine CreateRoutine(
            string title,
            string category,
            string? simpleDescription,
            string? originalDescription,
            IEnumerable<(int order, string simpleText, string? originalText, string? iconKey)> steps)
        {
            return new Routine
            {
                Title = title,
                Category = category,
                SimpleDescription = simpleDescription,
                OriginalDescription = originalDescription,
                IsActive = true,
                Steps = steps.Select(s => new RoutineStep
                {
                    Order = s.order,
                    SimpleText = s.simpleText,
                    OriginalText = s.originalText,
                    IconKey = s.iconKey
                }).ToList()
            };
        }
    }
}



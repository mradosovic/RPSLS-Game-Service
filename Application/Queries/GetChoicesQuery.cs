using Domain.Entities;
using MediatR;

namespace Application.Queries
{
    public class GetChoicesQuery : IRequest<IEnumerable<ChoiceType>> { }
}

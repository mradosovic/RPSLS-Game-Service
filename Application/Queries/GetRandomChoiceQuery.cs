using Domain.Entities;
using MediatR;

namespace Application.Queries
{
    public class GetRandomChoiceQuery : IRequest<Choice> { }
}

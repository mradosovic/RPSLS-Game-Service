using MediatR;
using Domain.Entities;
using Domain.Repositories;
using Application.Queries;

namespace Application.Handlers.QueryHandlers
{
    public class GetRandomChoiceHandler : IRequestHandler<GetRandomChoiceQuery, Choice>
    {
        private readonly IChoiceRepository _choiceRepository;

        public GetRandomChoiceHandler(IChoiceRepository choiceRepository)
        {
            _choiceRepository = choiceRepository;
        }

        public async Task<Choice> Handle(GetRandomChoiceQuery request, CancellationToken cancellationToken)
        {
            return await _choiceRepository.GetRandomChoiceAsync();
        }
    }
}


using Application.Queries;
using Domain.Entities;
using Domain.Repositories;
using MediatR;

namespace Application.Handlers.QueryHandlers
{
    public class GetChoicesHandler : IRequestHandler<GetChoicesQuery, IEnumerable<ChoiceType>>
    {
        private readonly IChoiceRepository _choiceRepository;

        public GetChoicesHandler(IChoiceRepository choiceRepository)
        {
            _choiceRepository = choiceRepository;
        }

        public async Task<IEnumerable<ChoiceType>> Handle(GetChoicesQuery request, CancellationToken cancellationToken)
        {
            var choices = await _choiceRepository.GetChoicesAsync();
            return choices.Select(c => (ChoiceType)c.Id);
        }

    }
}


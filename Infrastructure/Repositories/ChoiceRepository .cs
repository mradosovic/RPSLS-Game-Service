using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
using Domain.Repositories;
using Domain.Entities;
using Infrastructure.Settings;

namespace Infrastructure.Repositories
{
    public class ChoiceRepository : IChoiceRepository
    {
        private readonly ApiSettings _apiSettings;
        private readonly ILogger _logger;

        public ChoiceRepository(IOptions<ApiSettings> apiSettings, ILogger<ChoiceRepository> logger)
        {
            _apiSettings = apiSettings.Value;
            _logger = logger;
        }
        private static readonly List<Choice> Choices = new()
        {
            new Choice(1, "Rock"),
            new Choice(2, "Paper"),
            new Choice(3, "Scissors"),
            new Choice(4, "Lizard"),
            new Choice(5, "Spock")
        };

        public Task<IEnumerable<Choice>> GetChoicesAsync()
        {
            return Task.FromResult(Choices.AsEnumerable());
        }

        public async Task<Choice> GetRandomChoiceAsync()
        {
            int index;

            try
            {
                using var httpClient = new HttpClient();

                var response = await httpClient.GetFromJsonAsync<RandomNumberResponse>(_apiSettings.RandomChoiceApiUrl);

                if (response == null)
                {
                    throw new Exception("Failed to retrieve random number.");
                }

                var randomNumber = response.random_number;
                index = randomNumber % Choices.Count; //This effectively "wraps" the random number to ensure it stays within the bounds of the list.
                                                      //For example, if randomNumber is 7 and there are 5 choices, 7 % 5 would yield 2, allowing access to the third choice in the list.
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred in {MethodName}", nameof(GetRandomChoiceAsync));
                throw new Exception("An unexpected error occurred.");
            }

            return Choices[index];
        }
    }

    internal class RandomNumberResponse
    {
        public int random_number { get; set; }
    }
}

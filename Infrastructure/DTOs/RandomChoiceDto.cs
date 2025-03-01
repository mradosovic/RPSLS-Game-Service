﻿namespace Infrastructure.DTOs
{
    public record RandomChoiceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public RandomChoiceDto(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

}

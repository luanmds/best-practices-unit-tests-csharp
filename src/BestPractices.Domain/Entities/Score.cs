using BestPractices.Domain.ValueObjects;

namespace BestPractices.Domain.Entities;

public class Score
{
    public string Id { get; set; }
    public int ScoreValue { get; set; }
    public DateTime? ScoreDueDate { get; set; }
    public Customer Customer { get; set; }

    public Score()
    {
        Id = new Guid().ToString();
    }

    public Score(int scoreValue, DateTime? scoreDueDate, Customer customer)
    {
        Id = new Guid().ToString();
        ScoreValue = scoreValue;
        ScoreDueDate = scoreDueDate;
        Customer = customer;
    }
}

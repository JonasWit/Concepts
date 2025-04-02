namespace DemoApi.Models.Editions;

public class OrdinalEdition(int number) : IEdition
{
    public int Number { get; private set; } = number;

    public IEdition AdvanceToNext() =>
        new OrdinalEdition(Number + 1);
}

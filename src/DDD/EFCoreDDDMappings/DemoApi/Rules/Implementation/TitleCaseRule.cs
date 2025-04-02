using DemoApi.Models;

namespace DemoApi.Rules.Implementation;

public class TitleCaseRule : ITitleValidity
{
    public bool IsSatisfiedBy(BookTitle title) => true;         // TBD

    public BookTitle ApplyTo(BookTitle title) => title;         // TBD
}

public enum TitleCaseStyle
{
    AmericanPsychologicalAssociation,
    AmericanMedicalAssociation,
    AssociatedPress,
    ChicagoManualOfStyle,
    ModernLanguageAssociation,
    NewYorkTimes,
}
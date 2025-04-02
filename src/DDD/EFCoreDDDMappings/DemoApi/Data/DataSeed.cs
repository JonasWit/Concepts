using System.Globalization;
using DemoApi.Models;
using DemoApi.Models.Editions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DemoApi.Data;

public class DataSeed(ILogger<DataSeed> logger, BookstoreDbContext dbContext)
{
    private readonly ILogger<DataSeed> _logger = logger;
    private readonly BookstoreDbContext _dbContext = dbContext;

    public async Task SeedAuthors()
    {
        RobertMartin = await SeedAuthor(RobertMartin);
        SteveMcConnell = await SeedAuthor(SteveMcConnell);
        CharlesPetzold = await SeedAuthor(CharlesPetzold);
        GayleLaakmannMcDowell = await SeedAuthor(GayleLaakmannMcDowell);
        ErichGamma = await SeedAuthor(ErichGamma);
        RichardHelm = await SeedAuthor(RichardHelm);
        RalphJohnson = await SeedAuthor(RalphJohnson);
        JohnVlissides = await SeedAuthor(JohnVlissides);
        EricEvans = await SeedAuthor(EricEvans);
        JoshuaBloch = await SeedAuthor(JoshuaBloch);
        BertrandMeyer = await SeedAuthor(BertrandMeyer);
        MartinFowler = await SeedAuthor(MartinFowler);
        JonBentley = await SeedAuthor(JonBentley);
        KentBeck = await SeedAuthor(KentBeck);
        DonaldKnuth = await SeedAuthor(DonaldKnuth);
        BrianKernighan = await SeedAuthor(BrianKernighan);
        DennisRitchie = await SeedAuthor(DennisRitchie);
        FrederickBrooks = await SeedAuthor(FrederickBrooks);
        AndrewHunt = await SeedAuthor(AndrewHunt);
        DavidThomas = await SeedAuthor(DavidThomas);
        MichaelFeathers = await SeedAuthor(MichaelFeathers);
        // EnricoBuonanno = await SeedAuthor(EnricoBuonanno);

        await _dbContext.SaveChangesAsync();
    }

    private async Task<Author> SeedAuthor(Author author)
    {
        if (await _dbContext.Authors.FirstOrDefaultAsync(a => a.FullName == author.FullName) is Author existing) return existing;
        _dbContext.Authors.Add(author);
        return author;
    }

    public async Task SeedBooks()
    {
        await SeedAuthors();

        await SeedBook(Book.CreateNew(new("Clean Code: A Handbook of Agile Software Craftsmanship", English), English, new("9780132350884"), [RobertMartin], new Release(PrenticeHall, new OrdinalEdition(1), new Planned(new FullDate(new DateOnly(2008, 08, 11)))), "clean-code"));
        await SeedBook(Book.CreateNew(new("Code Complete: A Practical Handbook of Software Construction", English), English, new("9780735619678"), [SteveMcConnell], new Release(MicrosoftPress, new SeasonalEdition(YearSeason.Autumn, 2004), new Planned(new FullDate(new DateOnly(2004, 06, 09)))), "code-complete"));
        await SeedBook(Book.CreateNew(new("Code: The Hidden Language of Computer Hardware and Software", English), English, new("9780735611313"), [CharlesPetzold], new Release(MicrosoftPress, new OrdinalEdition(1), new Published(new FullDate(new DateOnly(2000, 10, 21)))), "code"));
        await SeedBook(Book.CreateNew(new("Cracking the Coding Interview: 189 Programming Questions and Solutions", English), English, new("9780984782857"), [GayleLaakmannMcDowell], new Release(CareerCup, new OrdinalEdition(1), new Planned(new FullDate(new DateOnly(2015, 07, 01)))), "cracking-the-coding-interview"));
        await SeedBook(Book.CreateNew(new("Design Patterns: Elements of Reusable Object-Oriented Software", English), English, new("9780201633610"), [ErichGamma, RichardHelm, RalphJohnson, JohnVlissides], new Release(AddisonWesleyProfessional, new OrdinalEdition(1), new Published(new FullDate(new DateOnly(1994, 10, 21)))), "design-patterns"));
        await SeedBook(Book.CreateNew(new("Domain-Driven Design: Tackling Complexity in the Heart of Software", English), English, new("9780321125217"), [EricEvans], new Release(AddisonWesleyProfessional, new OrdinalEdition(1), new Planned(new FullDate(new DateOnly(2003, 08, 30)))), "domain-driven-design"));
        await SeedBook(Book.CreateNew(new("Effective Java", English), English, new("9780134685991"), [JoshuaBloch], new Release(AddisonWesleyProfessional, new OrdinalEdition(1), new Planned(new FullDate(new DateOnly(2008, 05, 08)))), "effective-java"));
        await SeedBook(Book.CreateNew(new("Object-Oriented Software Construction", English), English, new("9780136291558"), [BertrandMeyer], new Release(PrenticeHall, new OrdinalEdition(1), new Published(new Year(1988))), "object-oriented-software-construction"));
        await SeedBook(Book.CreateNew(new("Patterns of Enterprise Application Architecture", English), English, new("9780321127426"), [MartinFowler], new Release(AddisonWesleyProfessional, new OrdinalEdition(1), new Planned(new FullDate(new DateOnly(2002, 11, 15)))), "patterns-of-enterprise-application-architecture"));
        await SeedBook(Book.CreateNew(new("Programming Pearls", English), English, new("9780201657883"), [JonBentley], new Release(AddisonWesleyProfessional, new OrdinalEdition(1), new Published(new YearMonth(1999, 9))), "programming-pearls"));
        await SeedBook(Book.CreateNew(new("Refactoring: Improving the Design of Existing Code", English), English, new("9780201485677"), [MartinFowler], new Release(AddisonWesleyProfessional, new OrdinalEdition(1), new Published(new YearMonth(1999, 07))), "refactoring"));
        await SeedBook(Book.CreateNew(new("Test-Driven Development: By Example", English), English, new("9780321146533"), [KentBeck], new Release(AddisonWesleyProfessional, new OrdinalEdition(1), new Planned(new FullDate(new DateOnly(2002, 11, 18)))), "test-driven-development"));
        await SeedBook(Book.CreateNew(new("The Clean Architecture: A Craftsman's Guide to Software Structure and Design", English), English, new("9780134494166"), [RobertMartin], new Release(PrenticeHall, new OrdinalEdition(1), new NotPlannedYet()), "the-clean-architecture"));
        await SeedBook(Book.CreateNew(new("The Art of Computer Programming, Volumes 1-4A Boxed Set", English), English, new("9780321751041"), [DonaldKnuth], new Release(AddisonWesleyProfessional, new OrdinalEdition(1), new Planned(new FullDate(new DateOnly(2011, 02, 14)))), "the-art-of-computer-programming"));
        await SeedBook(Book.CreateNew(new("The C Programming Language", English), English, new("9780131103627"), [BrianKernighan, DennisRitchie], new Release(PrenticeHall, new OrdinalEdition(1), new Published(new Year(1978))), "the-c-programming-language"));
        await SeedBook(Book.CreateNew(new("The Mythical Man-Month: Essays on Software Engineering", English), English, new("9780201835953"), [FrederickBrooks], new Release(AddisonWesleyProfessional, new OrdinalEdition(1), new Published(new Year(1975))), "the-mythical-man-month"));
        await SeedBook(Book.CreateNew(new("The Pragmatic Programmer: From Journeyman to Master", English), English, new("9780201616224"), [AndrewHunt, DavidThomas], new Release(AddisonWesleyProfessional, new SeasonalEdition(YearSeason.Summer, 1999), new Published(new YearMonth(1999, 10))), "the-pragmatic-programmer"));
        await SeedBook(Book.CreateNew(new("Working Effectively with Legacy Code", English), English, new("9780131177055"), [MichaelFeathers], new Release(PrenticeHall, new OrdinalEdition(1), new Planned(new FullDate(new DateOnly(2004, 09, 10)))), "working-effectively-with-legacy-code"));
        await SeedBook(Book.CreateNew(new("Build a Large Language Model", English), English, new("9781617296864"), [SebastianRaschka], new Release(Manning, new OrdinalEdition(1), new Published(new YearMonth(2024, 9))), "build-large-language-model"));

        await _dbContext.SaveChangesAsync();
    }

    private async Task<Book> SeedBook(Book book)
    {
        if (await _dbContext.Books.FirstOrDefaultAsync(b => b.Title == book.Title) is Book existing) return existing;
        _logger.LogInformation($"Adding {book}", book);
        _dbContext.Books.Add(book);
        return book;
    }

    private Publisher PrenticeHall { get; set; } = Publisher.CreateNew("Prentice Hall", "prentice-hall");
    private Publisher MicrosoftPress { get; set; } = Publisher.CreateNew("Microsoft Press", "microsoft-press");
    private Publisher AddisonWesleyProfessional { get; set; } = Publisher.CreateNew("Addison-Wesley Professional", "addison-wesley");
    private Publisher CareerCup { get; set; } = Publisher.CreateNew("CareerCup", "career-cup");
    private Publisher Manning { get; set; } = Publisher.CreateNew("Manning", "manning");

    private static CultureInfo English => new("en-US");
    private static CultureInfo French => new("fr-FR");
    private static CultureInfo Italian => new("it-IT");
    private static CultureInfo German = new("de-DE");

    private Author RobertMartin { get; set; } = Author.CreateNew(English, new PersonalName("Robert", "Cecil", "Martin"), "Robert C. Martin", "robert-martin");
    private Author SteveMcConnell { get; set; } = Author.CreateNew(English, new PersonalName("Steve", string.Empty, "McConnell"), "Steve McConnell", "steve-mcconnell");
    private Author CharlesPetzold { get; set; } = Author.CreateNew(English, new PersonalName("Charles", string.Empty, "Petzold"), "Charles Petzold", "charles-petzold");
    private Author GayleLaakmannMcDowell { get; set; } = Author.CreateNew(English, new PersonalName("Gayle", "Laakmann", "McDowell"), "Gayle Laakmann McDowell", "gayle-mcdowell");
    private Author ErichGamma { get; set; } = Author.CreateNew(English, new PersonalName("Erich", string.Empty, "Gamma"), "Erich Gamma", "erich-gamma");
    private Author RichardHelm { get; set; } = Author.CreateNew(English, new PersonalName("Richard", string.Empty, "Helm"), "Richard Helm", "richard-helm");
    private Author RalphJohnson { get; set; } = Author.CreateNew(English, new PersonalName("Ralph", string.Empty, "Johnson"), "Ralph Johnson", "ralph-johnson");
    private Author JohnVlissides { get; set; } = Author.CreateNew(English, new PersonalName("John", string.Empty, "Vlissides"), "John Vlissides", "john-vlissides");
    private Author EricEvans { get; set; } = Author.CreateNew(English, new PersonalName("Eric", string.Empty, "Evans"), "Eric Evans", "eric-evans");
    private Author JoshuaBloch { get; set; } = Author.CreateNew(English, new PersonalName("Joshua", string.Empty, "Bloch"), "Joshua Bloch", "joshua-bloch");
    private Author BertrandMeyer { get; set; } = Author.CreateNew(French, new PersonalName("Bertrand", string.Empty, "Meyer"), "Bertrand Meyer", "bertrand-meyer");
    private Author MartinFowler { get; set; } = Author.CreateNew(English, new PersonalName("Martin", string.Empty, "Fowler"), "Martin Fowler", "martin-fowler");
    private Author JonBentley { get; set; } = Author.CreateNew(English, new PersonalName("Jon", string.Empty, "Bentley"), "Jon Bentley", "jon-bentley");
    private Author KentBeck { get; set; } = Author.CreateNew(English, new PersonalName("Kent", string.Empty, "Beck"), "Kent Beck", "kent-beck");
    private Author DonaldKnuth { get; set; } = Author.CreateNew(English, new PersonalName("Donald", string.Empty, "Knuth"), "Donald Knuth", "donald-knuth");
    private Author BrianKernighan { get; set; } = Author.CreateNew(English, new PersonalName("Brian", string.Empty, "Kernighan"), "Brian Kernighan", "brian-kernighan");
    private Author DennisRitchie { get; set; } = Author.CreateNew(English, new PersonalName("Dennis", string.Empty, "Ritchie"), "Dennis Ritchie", "dennis-ritchie");
    private Author FrederickBrooks { get; set; } = Author.CreateNew(English, new PersonalName("Frederick", string.Empty, "Brooks"), "Frederick Brooks", "frederick-brooks");
    private Author AndrewHunt { get; set; } = Author.CreateNew(English, new PersonalName("Andrew", string.Empty, "Hunt"), "Andrew Hunt", "andrew-hunt");
    private Author DavidThomas { get; set; } = Author.CreateNew(English, new PersonalName("David", string.Empty, "Thomas"), "David Thomas", "david-thomas");
    private Author MichaelFeathers { get; set; } = Author.CreateNew(English, new PersonalName("Michael", string.Empty, "Feathers"), "Michael Feathers", "michael-feathers");
    private Author SebastianRaschka { get; set; } = Author.CreateNew(German, new PersonalName("Sebastian", string.Empty, "Raschka"), "Sebastian Raschka", "sebastian-raschka");
    // private Author EnricoBuonanno { get; set; } = Author.CreateNew(Italian.Name, new PersonalName("Enrico", string.Empty, "Buonanno"), "Enrico Buonanno", "enrico-buonanno");
}


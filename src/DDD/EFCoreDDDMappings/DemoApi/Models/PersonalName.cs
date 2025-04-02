using System.Globalization;
using DemoApi.Common;

namespace DemoApi.Models;

public record PersonalName(string First, string Middle, string Last);

public delegate Slug PersonalNameToSlug(CultureInfo culture, PersonalName name);
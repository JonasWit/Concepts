using System.Globalization;
using DemoApi.Common;

namespace DemoApi.Models;

public delegate Slug BookTitleToSlug(CultureInfo culture, string title);
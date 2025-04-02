using System.Text.Json;
using DekatMe.Core.Entities;
using Microsoft.Extensions.Logging;

namespace DekatMe.Console
{
    public class DataExportService
    {
        private readonly ILogger<DataExportService> _logger;

        public DataExportService(ILogger<DataExportService> logger)
        {
            _logger = logger;
        }

        public async Task<ExportResult> ExportBusinessesToJsonAsync(IEnumerable<Business> businesses, string filePath)
        {
            try
            {
                _logger.LogInformation("Starting business export to file: {FilePath}", filePath);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(businesses, options);
                await File.WriteAllTextAsync(filePath, json);

                var count = businesses.Count();
                _logger.LogInformation("Successfully exported {Count} businesses to JSON file", count);

                return new ExportResult
                {
                    Success = true,
                    Message = $"Successfully exported {count} businesses to {filePath}",
                    RecordsExported = count,
                    FilePath = filePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting businesses to JSON");
                return new ExportResult { Success = false, Message = $"Export failed: {ex.Message}" };
            }
        }

        public async Task<ExportResult> ExportCategoriesToJsonAsync(IEnumerable<Category> categories, string filePath)
        {
            try
            {
                _logger.LogInformation("Starting category export to file: {FilePath}", filePath);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(categories, options);
                await File.WriteAllTextAsync(filePath, json);

                var count = categories.Count();
                _logger.LogInformation("Successfully exported {Count} categories to JSON file", count);

                return new ExportResult
                {
                    Success = true,
                    Message = $"Successfully exported {count} categories to {filePath}",
                    RecordsExported = count,
                    FilePath = filePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting categories to JSON");
                return new ExportResult { Success = false, Message = $"Export failed: {ex.Message}" };
            }
        }

        public async Task<ExportResult> ExportReviewsToJsonAsync(IEnumerable<Review> reviews, string filePath)
        {
            try
            {
                _logger.LogInformation("Starting review export to file: {FilePath}", filePath);

                var options = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                var json = JsonSerializer.Serialize(reviews, options);
                await File.WriteAllTextAsync(filePath, json);

                var count = reviews.Count();
                _logger.LogInformation("Successfully exported {Count} reviews to JSON file", count);

                return new ExportResult
                {
                    Success = true,
                    Message = $"Successfully exported {count} reviews to {filePath}",
                    RecordsExported = count,
                    FilePath = filePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting reviews to JSON");
                return new ExportResult { Success = false, Message = $"Export failed: {ex.Message}" };
            }
        }

        public async Task<ExportResult> ExportToCsvAsync<T>(IEnumerable<T> items, string filePath, string[] propertyNames)
        {
            try
            {
                _logger.LogInformation("Starting CSV export to file: {FilePath}", filePath);

                // Get property getters for each property
                var type = typeof(T);
                var propertyGetters = propertyNames
                    .Select(name => type.GetProperty(name))
                    .Where(prop => prop != null)
                    .ToList();

                if (!propertyGetters.Any())
                {
                    _logger.LogWarning("No valid properties found for CSV export");
                    return new ExportResult { Success = false, Message = "No valid properties found for CSV export" };
                }

                // Build CSV content
                var lines = new List<string>();

                // Add header
                lines.Add(string.Join(",", propertyNames));

                // Add data rows
                foreach (var item in items)
                {
                    var values = propertyGetters
                        .Select(prop => {
                            var value = prop?.GetValue(item);
                            return FormatCsvValue(value);
                        });
                    lines.Add(string.Join(",", values));
                }

                // Write to file
                await File.WriteAllLinesAsync(filePath, lines);

                var count = items.Count();
                _logger.LogInformation("Successfully exported {Count} items to CSV file", count);

                return new ExportResult
                {
                    Success = true,
                    Message = $"Successfully exported {count} items to {filePath}",
                    RecordsExported = count,
                    FilePath = filePath
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error exporting to CSV");
                return new ExportResult { Success = false, Message = $"Export failed: {ex.Message}" };
            }
        }

        private string FormatCsvValue(object? value)
        {
            if (value == null)
                return string.Empty;

            var stringValue = value.ToString() ?? string.Empty;

            // Escape quotes and wrap in quotes if needed
            if (stringValue.Contains(',') || stringValue.Contains('"') || stringValue.Contains('\n'))
            {
                stringValue = stringValue.Replace("\"", "\"\"");
                stringValue = $"\"{stringValue}\"";
            }

            return stringValue;
        }
    }

    public class ExportResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public int RecordsExported { get; set; }
        public string FilePath { get; set; } = string.Empty;
    }
}

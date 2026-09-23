using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GameEngine
{
    internal static class JsonContentLoader
    {
        private static readonly JsonSerializerOptions Options =
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

        static JsonContentLoader()
        {
            Options.Converters.Add(
                new JsonStringEnumConverter());
        }

        internal static List<T> Load<T>(
            string resourceName)
        {
            Assembly assembly =
                typeof(JsonContentLoader).Assembly;

            using Stream stream =
                assembly.GetManifestResourceStream(
                    resourceName);

            if (stream == null)
            {
                throw new FileNotFoundException(
                    $"Embedded content resource was not found: {resourceName}");
            }

            List<T> definitions =
                JsonSerializer.Deserialize<List<T>>(
                    stream,
                    Options);

            if (definitions == null)
            {
                throw new InvalidOperationException(
                    $"Could not deserialize content resource: {resourceName}");
            }

            return definitions;
        }


        internal static List<T> LoadFile<T>(
            string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(
                    "Content file path cannot be empty.",
                    nameof(filePath));

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException(
                    $"Content file was not found: {filePath}",
                    filePath);
            }

            using FileStream stream =
                File.OpenRead(filePath);

            List<T> definitions =
                JsonSerializer.Deserialize<List<T>>(
                    stream,
                    Options);

            if (definitions == null)
            {
                throw new InvalidOperationException(
                    $"Could not deserialize content file: {filePath}");
            }

            return definitions;
        }
    }
}
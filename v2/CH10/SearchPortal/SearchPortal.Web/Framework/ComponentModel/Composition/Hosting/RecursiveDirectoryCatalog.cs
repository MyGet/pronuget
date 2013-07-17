﻿namespace MefContrib.Hosting
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.Composition.Hosting;
    using System.ComponentModel.Composition.Primitives;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Extends <see cref="DirectoryCatalog"/> to support discovery of parts in sub-directories.
    /// </summary>
    public class RecursiveDirectoryCatalog : ComposablePartCatalog, INotifyComposablePartCatalogChanged, ICompositionElement
    {
        private AggregateCatalog _aggregateCatalog;
        private readonly string _path;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursiveDirectoryCatalog"/> class with <see cref="ComposablePartDefinition"/> objects based on all the DLL files in the specified directory path and its sub-directories.
        /// </summary>
        /// <param name="path">Path to the directory to scan for assemblies to add to the catalog.</param>
        public RecursiveDirectoryCatalog(string path)
            : this(path, "*.dll")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecursiveDirectoryCatalog"/> class with <see cref="ComposablePartDefinition"/> objects based on the specified search pattern in the specified directory path path and its sub-directories.
        /// </summary>
        /// <param name="path">Path to the directory to scan for assemblies to add to the catalog.</param>
        /// <param name="searchPattern">The pattern to search with. The format of the pattern should be the same as specified for GetFiles.</param>
        /// <exception cref="ArgumentNullException">The value of the <paramref name="path"/> parameter was <see langword="null"/>.</exception>
        public RecursiveDirectoryCatalog(string path, string searchPattern)
        {
            if (path == null) throw new ArgumentNullException("path");

            _path = path;

            Initialize(path, searchPattern);
        }

        private static IEnumerable<string> GetFoldersRecursive(string path)
        {
            var result = new List<string> { path };
            foreach (var child in Directory.GetDirectories(path))
            {
                result.AddRange(GetFoldersRecursive(child));
            }
            return result;
        }

        private void Initialize(string path, string searchPattern)
        {
            var directoryCatalogs = GetFoldersRecursive(path).Select(dir => new DirectoryCatalog(dir, searchPattern));
            _aggregateCatalog = new AggregateCatalog();
            _aggregateCatalog.Changed += (o, e) =>
            {
                if (Changed != null)
                {
                    Changed(o, e);
                }
            };
            _aggregateCatalog.Changing += (o, e) =>
            {
                if (Changing != null)
                {
                    Changing(o, e);
                }
            };
            foreach (var catalog in directoryCatalogs)
            {
                _aggregateCatalog.Catalogs.Add(catalog);
            }
        }

        /// <summary>
        /// Gets the part definitions that are contained in the recursive directory catalog. (Overrides ComposablePartCatalog.Parts.)
        /// </summary>
        public override IQueryable<ComposablePartDefinition> Parts
        {
            get { return _aggregateCatalog.Parts; }
        }

        /// <summary>
        /// Occurs when the contents of the catalog has changed.
        /// </summary>
        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changed;

        /// <summary>
        /// Occurs when the catalog is changing.
        /// </summary>
        public event EventHandler<ComposablePartCatalogChangeEventArgs> Changing;

        private string GetDisplayName()
        {
            return string.Format(
                CultureInfo.CurrentCulture,
                "{0} (RecusrivePath=\"{1}\")", new[] { GetType().Name, _path });
        }

        public override string ToString()
        {
            return GetDisplayName();
        }

        /// <summary>
        /// Gets the display name of the directory catalog.
        /// </summary>
        string ICompositionElement.DisplayName
        {
            get { return GetDisplayName(); }
        }

        /// <summary>
        /// Gets the composition element from which the directory catalog originated.
        /// </summary>
        ICompositionElement ICompositionElement.Origin
        {
            get { return null; }
        }
    }
}


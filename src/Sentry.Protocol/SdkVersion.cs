using System;
#if !LACKS_CONCURRENT_COLLECTIONS
using System.Collections.Concurrent;
#endif
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Threading;

namespace Sentry.Protocol
{
    /// <summary>
    /// Information about the SDK to be sent with the SentryEvent
    /// </summary>
    /// <remarks>Requires Sentry version 8.4 or higher</remarks>
    [DataContract]
    public class SdkVersion
    {
#if !LACKS_CONCURRENT_COLLECTIONS
        private readonly Lazy<ConcurrentBag<Package>> _lazyPackages =
            new Lazy<ConcurrentBag<Package>>(LazyThreadSafetyMode.PublicationOnly);

        [DataMember(Name = "packages", EmitDefaultValue = false)]
        internal ConcurrentBag<Package> InternalPackages
            => _lazyPackages.IsValueCreated
                ? _lazyPackages.Value
                : null;
#else
        [DataMember(Name = "packages", EmitDefaultValue = false)]
        internal List<Package> InternalPackages
        {
            get => InternalPackages ?? (InternalPackages = new List<Package>());
            set => InternalPackages = value;
        }
#endif

        /// <summary>
        /// SDK packages
        /// </summary>
        /// <remarks>This property is not required</remarks>
        public IEnumerable<Package> Packages =>
#if LACKS_CONCURRENT_COLLECTIONS
            InternalPackages;
#else
           _lazyPackages.Value;
#endif

        /// <summary>
        /// SDK name
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name
        {
            get;
            // For integrations to set their name
            [EditorBrowsable(EditorBrowsableState.Never)]
            set;
        }
        /// <summary>
        /// SDK Version
        /// </summary>
        [DataMember(Name = "version", EmitDefaultValue = false)]
        public string Version
        {
            get;
            // For integrations to set their version
            [EditorBrowsable(EditorBrowsableState.Never)]
            set;
        }

        /// <summary>
        /// Add a package used to compose the SDK
        /// </summary>
        /// <param name="name">The package name.</param>
        /// <param name="version">The package version.</param>
        public void AddPackage(string name, string version)
            => AddPackage(new Package(name, version));

        internal void AddPackage(Package package)
#if !LACKS_CONCURRENT_COLLECTIONS
            => _lazyPackages.Value.Add(package);
#else
            => InternalPackages.Add(package);
        #endif
    }
}

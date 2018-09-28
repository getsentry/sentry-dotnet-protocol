using System.Runtime.Serialization;

namespace Sentry.Protocol
{
    /// <summary>
    /// A repository registered within Sentry
    /// </summary>
    /// <remarks>
    /// An interface which describes the local repository configuration for an application.
    /// This is used to map up source code information to stored repository configuration.
    /// Added to Sentry version 8.11.0
    /// </remarks>
    /// <example>
    /// It is used to say the file Sentry/SentrySdk.cs lives in the getsentry/sentry-dotnet repo, and is located at src/Sentry/SentrySdk.cs.
    /// </example>
    /// <seealso href="https://docs.sentry.io/clientdev/interfaces/repos/"/>
    [DataContract]
    public class Repo
    {
        /// <summary>
        /// The name of the repository as it is registered in Sentry.
        /// </summary>
        [DataMember(Name = "name", EmitDefaultValue = false)]
        public string Name { get; set; }

        /// <summary>
        /// The optional prefix path to apply to source code when pairing it up with files in the repository.
        /// </summary>
        [DataMember(Name = "prefix", EmitDefaultValue = false)]
        public string Prefix { get; set; }

        /// <summary>
        /// The optional current revision of the local repository.
        /// </summary>
        [DataMember(Name = "revision", EmitDefaultValue = false)]
        public string Revision { get; set; }
    }
}

using System.Collections.Generic;

namespace EduroamApp
{
	/// <summary>
	/// Stores information found in an EAP-config file.
	/// </summary>
	public class EapConfig
	{
		// Properties
		public List<AuthenticationMethod> AuthenticationMethods { get; set; }
		public ProviderInfo InstitutionInfo { get; set; }

		/// <summary>
		/// AuthenticationMethod contains information about client certificates and CAs.
		/// </summary>
		public class AuthenticationMethod
		{
			// Properties
			public uint EapType { get; set; }
			public List<string> CertificateAuthorities { get; set; }
			public List<string> ServerName { get; set; }
			public string ClientCertificate { get; set; }
			public string ClientPassphrase { get; set; }

			// Constructor
			public AuthenticationMethod(uint eapType, List<string> certificateAuthorities, List<string> serverName, string clientCertificate = null, string clientPassphrase = null)
			{
				EapType = eapType;
				CertificateAuthorities = certificateAuthorities;
				ServerName = serverName;
				ClientCertificate = clientCertificate;
				ClientPassphrase = clientPassphrase;
			}
		}

		/// <summary>
		/// ProviderInfo contains information about the config file's provider.
		/// </summary>
		public class ProviderInfo
		{
			// Properties
			public string DisplayName { get; set; }
			public string Logo { get; set; }
			public string LogoFormat { get; set; }
			public string EmailAddress { get; set; }
			public string WebAddress { get; set; }
			public string Phone { get; set; }
			public string InstId { get; set; }
			public string TermsOfUse { get; set; }

			// Constructor
			public ProviderInfo(string displayName, string logo, string logoFormat, string emailAddress, string webAddress, string phone, string instId, string termsOfUse)
			{
				DisplayName = displayName ?? string.Empty; // if value is null, make it ""
				Logo = logo;
				LogoFormat = logoFormat;
				EmailAddress = emailAddress ?? string.Empty; // if value is null, make it ""
				WebAddress = webAddress ?? string.Empty; // if value is null, make it ""
				Phone = phone ?? string.Empty; // if value is null, make it ""
				InstId = instId;
				TermsOfUse = termsOfUse;
			}
		}

	}
}

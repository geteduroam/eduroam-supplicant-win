//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WpfApp.Properties {
	using System;


	/// <summary>
	///   A strongly-typed resource class, for looking up localized strings, etc.
	/// </summary>
	// This class was auto-generated by the StronglyTypedResourceBuilder
	// class via a tool like ResGen or Visual Studio.
	// To add or remove a member, edit your .ResX file then rerun ResGen
	// with the /str option, or rebuild your VS project.
	[global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
	[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
	[global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
	internal class Resources {

		private static global::System.Resources.ResourceManager resourceMan;

		private static global::System.Globalization.CultureInfo resourceCulture;

		[global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
		internal Resources() {
		}

		/// <summary>
		///   Returns the cached ResourceManager instance used by this class.
		/// </summary>
		[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		internal static global::System.Resources.ResourceManager ResourceManager {
			get {
				if (object.ReferenceEquals(resourceMan, null)) {
					global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WpfApp.Properties.Resources", typeof(Resources).Assembly);
					resourceMan = temp;
				}
				return resourceMan;
			}
		}

		/// <summary>
		///   Overrides the current thread's CurrentUICulture property for all
		///   resource lookups using this strongly typed resource class.
		/// </summary>
		[global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
		internal static global::System.Globalization.CultureInfo Culture {
			get {
				return resourceCulture;
			}
			set {
				resourceCulture = value;
			}
		}

		/// <summary>
		///   Looks up a localized resource of type System.Byte[].
		/// </summary>
		internal static byte[] eduroam_logo {
			get {
				object obj = ResourceManager.GetObject("eduroam_logo", resourceCulture);
				return ((byte[])(obj));
			}
		}

		/// <summary>
		///   Looks up a localized string similar to &lt;!DOCTYPE html&gt;
		///&lt;html lang=&quot;en&quot; class=&quot;dialog&quot;&gt;
		///&lt;!--TODO: embed the css--&gt;
		///&lt;link rel=&quot;stylesheet&quot; href=&quot;https://geteduroam.no/assets/geteduroam.css&quot;&gt;
		///&lt;meta name=&quot;viewport&quot; content=&quot;width=device-width, initial-scale=1&quot;&gt;
		///&lt;title&gt;geteduroam - Authorization&lt;/title&gt;
		///&lt;div&gt;
		///&lt;main&gt;
		///	&lt;p class=&quot;text-center&quot;&gt;
		///		You have been authorized!&lt;br&gt;
		///		&lt;br&gt;
		///		You may now close this tab and&lt;br&gt;
		///		return to the app.
		///	&lt;/p&gt;
		///&lt;/main&gt;
		///&lt;/div&gt;
		///.
		/// </summary>
		internal static string oauth_accepted {
			get {
				return ResourceManager.GetString("oauth_accepted", resourceCulture);
			}
		}

		/// <summary>
		///   Looks up a localized string similar to &lt;!DOCTYPE html&gt;
		///&lt;html lang=&quot;en&quot; class=&quot;dialog&quot;&gt;
		///&lt;!--TODO: embed the css--&gt;
		///&lt;link rel=&quot;stylesheet&quot; href=&quot;https://geteduroam.no/assets/geteduroam.css&quot;&gt;
		///&lt;meta name=&quot;viewport&quot; content=&quot;width=device-width, initial-scale=1&quot;&gt;
		///&lt;title&gt;geteduroam - Authorization&lt;/title&gt;
		///&lt;div&gt;
		///&lt;main&gt;
		///	&lt;p class=&quot;text-center&quot;&gt;
		///		Authorization has been rejected.&lt;br&gt;
		///		&lt;br&gt;
		///		You may now close this tab and&lt;br&gt;
		///		return to the app.
		///	&lt;/p&gt;
		///&lt;/main&gt;
		///&lt;/div&gt;
		///.
		/// </summary>
		internal static string oauth_rejected {
			get {
				return ResourceManager.GetString("oauth_rejected", resourceCulture);
			}
		}
	}
}

using System;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using EduroamConfigure;
using System.Collections.Generic;

namespace EduroamApp
{
	/// <summary>
	/// Lets user select a local EAP-config file, or a local client certificate if required by EAP setup.
	/// </summary>
	[Obsolete]
	public partial class frmLocal : Form
	{
		// makes parent form accessible from this class
		private readonly frmParent frmParent;

		[Obsolete]
		public frmLocal(frmParent parentInstance)
		{
			// gets parent form instance
			frmParent = parentInstance;
			InitializeComponent();
		}

		// lets user browse their PC for a file
		[Obsolete]
		private void btnBrowse_Click(object sender, EventArgs e)
		{
			var dialogTitle = "";
			var dialogFilter = "";
			// expected filetype depends on label in parent form
			switch (frmParent.LocalFileType)
			{
				case "EAPCONFIG":
					dialogTitle = "Select EAP Config file";
					dialogFilter = "EAP-CONFIG files (*.eap-config)|*.eap-config|All files (*.*)|*.*";
					break;
				case "CERT":
					dialogTitle = "Select client certificate";
					dialogFilter = "Certificate files (*.PFX, *.P12)|*.pfx;*.p12|All files (*.*)|*.*";
					break;
			}
			// opens dialog to select file
			string selectedFilePath = FileDialog.getFileFromDialog(dialogTitle, dialogFilter);
			// prints out filepath
			txtFilepath.Text = selectedFilePath;
		}

		/// <summary>
		/// Gets EAP file and creates an EapConfig object from it.
		/// </summary>
		/// <returns>EapConfig object.</returns>
		[Obsolete]
		public EapConfig LocalEapConfig()
		{
			// validates the selected config file
			if (!FileDialog.validateFileSelection(txtFilepath.Text, new List<string>{".eap-config"})) return null;

			try
			{
				// gets content of config file
				string eapString = File.ReadAllText(txtFilepath.Text);
				// creates and returns EapConfig object
				return EapConfig.FromXmlData(eapString);
			}
			catch (XmlException xmlEx)
			{
				MessageBox.Show("The selected EAP config file is corrupted. Please choose another file.\n"
								+ "Exception: " + xmlEx.Message, "eduroam - Exception",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
			catch (ArgumentException argEx)
			{
				MessageBox.Show("Could not read from file. Make sure to select a valid EAP config file.\n"
								+ "Exception: " + argEx.Message, "eduroam - Exception",
								MessageBoxButtons.OK, MessageBoxIcon.Error);
				return null;
			}
		}

		/// <summary>
		/// Installs a client certificate from file.
		/// </summary>
		/// <returns>True if cert installation success, false if not.</returns>
		[Obsolete]
		public bool InstallCertFile()
		{
			// validates file selection
			if (!FileDialog.validateFileSelection(txtFilepath.Text, new List<string>{".pfx", ".p12"})) return false;

			try
			{
				var certificate = new X509Certificate2(txtFilepath.Text, txtCertPassword.Text);
				// TODO!!! this cert is never added to eapConfig ! it won't be installed
				return true;
			}
			// checks if correct password by trying to install certificate
			catch (CryptographicException ex)
			{
				if ((ex.HResult & 0xFFFF) == 0x56)
				{
					MessageBox.Show("The password you entered is incorrect.", "Certificate install",
									MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				else
				{
					MessageBox.Show("Could not install certificate.\nException: " + ex.Message, "Certificate install",
									 MessageBoxButtons.OK, MessageBoxIcon.Error);
				}

				return false;
			}
		}

		[Obsolete]
		private void txtFilepath_TextChanged(object sender, EventArgs e)
		{
			// stops checking if expected file type is not client certificate
			if (frmParent.LocalFileType == "EAPCONFIG") return;

			// checks if password required by trying to install certificate
			var passwordRequired = false;
			try
			{
				var testCertificate = new X509Certificate2(txtFilepath.Text, "");
			}
			catch (CryptographicException ex)
			{
				if ((ex.HResult & 0xFFFF) == 0x56)
				{
					passwordRequired = true;
				}
			}
			catch (Exception)
			{
				// ignored
			}

			// shows/hides password related controls
			if (passwordRequired)
			{
				lblCertPassword.Visible = true;
				txtCertPassword.Visible = true;
				chkShowPassword.Visible = true;
			}
			else
			{
				lblCertPassword.Visible = false;
				txtCertPassword.Visible = false;
				txtCertPassword.Text = "";
				chkShowPassword.Visible = false;
				chkShowPassword.Checked = false;
				txtCertPassword.UseSystemPasswordChar = true;
			}
		}

		// unmasks password characters on screen
		[Obsolete]
		private void cboShowPassword_CheckedChanged(object sender, EventArgs e)
		{
			txtCertPassword.UseSystemPasswordChar = !chkShowPassword.Checked;
		}
	}
}

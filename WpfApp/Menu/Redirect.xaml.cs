using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace WpfApp.Menu
{
	/// <summary>
	/// Interaction logic for Redirect.xaml
	/// </summary>
	public partial class Redirect : Page
	{
		private readonly MainWindow mainWindow;
		private readonly Uri redirect;
		public Redirect(MainWindow mainWindow, Uri redirect)
		{
			this.mainWindow = mainWindow ?? throw new ArgumentNullException(paramName: nameof(mainWindow));
			this.redirect = redirect;
			InitializeComponent();
			Load();
		}

		private void Load()
		{
			hlink.NavigateUri = redirect;
		}

		// TODO: fix hyperlink and open redirect + closing application
		private void Hyperlink_redirect(object sender, RequestNavigateEventArgs e)
		{
			Hyperlink hl = (Hyperlink)sender;
			string navigateUri = hl.NavigateUri.ToString();
			Process.Start(new ProcessStartInfo(navigateUri));
			e.Handled = true;
			mainWindow.Shutdown();
		}
	}
}

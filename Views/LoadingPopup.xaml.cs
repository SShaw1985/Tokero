using CommunityToolkit.Maui.Views;

namespace Tokero.Views
{
    public partial class LoadingPopup : Popup
    {
        public LoadingPopup ()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public void SetText (string text)
        {
            LoadingLabel.Text = text;
        }
    }
}
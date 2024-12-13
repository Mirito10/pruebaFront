using Newtonsoft.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace prueba
{
    public partial class MainPage : ContentPage
    {
        string laURL = "https://gameshubbackend1.azurewebsites.net/";

        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnProfilePictureClicked(object sender, EventArgs e)
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    FileTypes = FilePickerFileType.Images,
                    PickerTitle = "Selecciona una imagen de perfil"
                });

                if (result != null)
                {
                    var stream = await result.OpenReadAsync();
                    ((ImageButton)sender).Source = ImageSource.FromStream(() => stream);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "No se pudo seleccionar la imagen: " + ex.Message, "OK");
            }
        }

        private async void btnRegistrarse_Clicked(object sender, EventArgs e)
        {
            try
            {
                //VALIDAR

                //Conecto Api
                ReqCrearUsuario req = new ReqCrearUsuario();
                req.CrearUsuario = new Entidades.CrearUsuario();

                req.CrearUsuario.NombreUsuario = txtUsuarioname.Text;
                req.CrearUsuario.Email = txtcorreoelectronico.Text;
                req.CrearUsuario.Password = txtContra.Text;

                //Serializo
                var jsonContent = new StringContent(JsonConvert.SerializeObject(req),
                    System.Text.Encoding.UTF8, "application/json");

                HttpClient httpClient = new HttpClient();

                var response = await httpClient.PostAsync(laURL + "api/Usuario/CrearUsuario", jsonContent);

                //Conecto?
                if (response.IsSuccessStatusCode)
                {
                    //Si

                    var responseContent = await response.Content.ReadAsStringAsync();
                    ResCrearUsuario Res = new ResCrearUsuario();
                    Res = JsonConvert.DeserializeObject<ResCrearUsuario>(responseContent);

                    if (Res.Errores.Any())
                    {
                        await DisplayAlert("Éxito", "El usuario "+txtUsuarioname.Text+" se registro exitosamente", "Aceptar");
                    }
                    else
                    {
                        await DisplayAlert("Error de backend", "mensajeError", "Aceptar");
                    }
                }
                else
                {
                    //No
                    await DisplayAlert("Error de Conexion", "Error de conexion con el servidor", "Ni modo");
                }
            }
            catch (Exception Ex)
            {
                await DisplayAlert("Error de la aplicacion", "Error", "Aceptar");
            }
        }

    }

}

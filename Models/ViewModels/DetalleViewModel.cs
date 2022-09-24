namespace Rocastone.Models.ViewModels
{
    public class DetalleViewModel
    {

        //Lo inicializo aca para no tener que hacerlo en el controlador
        public DetalleViewModel()
        {
            Producto = new Producto();
        }
        public Producto Producto { get; set; }
        public bool ExisteEnCarro { get; set; }

    }
}

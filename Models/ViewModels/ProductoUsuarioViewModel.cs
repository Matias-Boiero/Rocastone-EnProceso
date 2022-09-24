namespace Rocastone.Models.ViewModels
{
    public class ProductoUsuarioViewModel //Siempre que deba trabajar con mas de un modelo para crear propiedades hago un ViewModel
    {
        public ProductoUsuarioViewModel() //Instancio la lista en el ctor para no tener que instanciarla en los controladores
        {
            ProductoLista = new List<Producto>();
        }
        public ApplicationUser ApplicationUser { get; set; }
        public IEnumerable<Producto> ProductoLista { get; set; }
    }
}

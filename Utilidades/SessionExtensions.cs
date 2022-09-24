using Newtonsoft.Json;

namespace Rocastone.Utilidades
{
    public static class SessionExtensions
    {
        //voy a necesitar dos clases para configurar las sesiones
        //un metodo set para configurar la sesion 
        //un metodo get para obtenr la sesion
        // Hago esto para que cuando un usuario presione el carro se abra la variante de sesion

        public static void SetObject(this ISession session, string key, object value)
        {
            string data = JsonConvert.SerializeObject(value);
            session.SetString(key, data);
        }

        public static T GetObject<T>(this ISession session, string key)
        {
            string data = session.GetString(key);
            if (data == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(data);
        }

    }
}
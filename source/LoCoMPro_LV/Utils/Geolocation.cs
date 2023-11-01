namespace LoCoMPro_LV.Utils
{
    /// <summary>
    /// Contiene una colección de métodos que permiten a determinar el radio de coincidencias en la geolocalización
    /// </summary>
    public class Geolocation
    {
        /// <summary>
        /// Calcula la distancia encontrada entre dos puntos.
        /// </summary>
        /// <param name="latitude1"> Latitud correspondiente a la primera coordenada</param>
        /// <param name="longitude1"> Longitud correspondiente a la primera coordenada</param>
        /// <param name="latitude2"> Latitud correspondiente a la segunda coordenada</param>
        /// <param name="longitude2"> Longitud correspondiente a la segunda coordenada</param>
        /// <returns> Regresa la distancia calculada</returns>
        public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double earthRadius = 6371000;

            double dLat = DegreesToRadians(latitude2 - latitude1);
            double dLon = DegreesToRadians(longitude2 - longitude1);

            latitude1 = DegreesToRadians(latitude1);
            latitude2 = DegreesToRadians(latitude2);

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(latitude1) * Math.Cos(latitude2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            double distance = earthRadius * c;

            return distance;
        }

        /// <summary>
        /// Convierte las unidades de grados a radianes
        /// </summary>
        /// <param name="degrees"> Unidad de grados a convertir</param>
        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}

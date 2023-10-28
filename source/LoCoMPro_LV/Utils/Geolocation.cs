namespace LoCoMPro_LV.Utils
{
    public class Geolocation
    {
        public static double CalculateDistance(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            double earthRadius = 6371000;

            double dLat = DegreesToRadians(latitude2 - latitude1);
            double dLon = DegreesToRadians(longitude2 - longitude1);

            latitude1 = DegreesToRadians(latitude1);
            latitude2 = DegreesToRadians(latitude2);

            // Fórmula haversine
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                        Math.Sin(dLon / 2) * Math.Sin(dLon / 2) * Math.Cos(latitude1) * Math.Cos(latitude2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            // Distancia en metros
            double distance = earthRadius * c;

            return distance;
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }
}

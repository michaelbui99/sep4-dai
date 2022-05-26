namespace WebAPI.DTO
{
    public class SetSettingsDTO
    {
        public int Co2Threshold { get; set; }
    
        public int HumidityThreshold { get; set; }

        public float TargetTemperature { get; set; }

        public int TemperatureMargin { get; set; }
    }
}
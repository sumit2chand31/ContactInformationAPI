using Newtonsoft.Json;

namespace ContactInformationAPI.Modal
{
    public class ReadContactInfo
    {
        private readonly string _sampleJsonFilePath;
        public ReadContactInfo(string sampleJsonFilePath)
        {
            _sampleJsonFilePath = sampleJsonFilePath;
        }

        public List<ConatctModal> GetConatcatData()
        {
            using StreamReader reader = new(_sampleJsonFilePath);
            var json = reader.ReadToEnd();
            List<ConatctModal> conatcts = JsonConvert.DeserializeObject<List<ConatctModal>>(json);
            return conatcts;
        }

        public void  WriteData(List<ConatctModal> conatctModals)
        {
            if (conatctModals != null)
            {
                string json = JsonConvert.SerializeObject(conatctModals);
                File.WriteAllText(_sampleJsonFilePath, json);
            }
        }
    }
}

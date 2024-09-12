using ContactInformationAPI.Modal;

namespace ContactInformationAPI.Repository
{
    public interface IContactDB
    {
        public Task<List<ConatctModal>> GetConatctAsync(string path);

        public Task<ConatctModal> GetContactByIdAsync(int Id, string path);

        public Task<int> AddConatctAsync(ConatctModalVM modal, string path);

        public Task<string> UpdateConatctAsync(ConatctModalVM modal, int Id, string path);

        public Task<bool> DeleteConatctAsync(int Id, string path);
    }
}

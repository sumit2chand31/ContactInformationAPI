using ContactInformationAPI.Modal;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Reflection;

namespace ContactInformationAPI.Repository
{
    public class ContactRespository : IContactDB
    {
        ReadContactInfo? readContactInfo;
 
        /// <summary>
        /// It is add new record in json file
        /// </summary>
        /// <param name="modal">contact modal</param>
        /// <param name="path">file path</param>
        /// <returns>it is return new id</returns>
        public Task<int> AddConatctAsync(ConatctModalVM modal, string path)
        {
            var data = new ReadContactInfo(path).GetConatcatData();
            var conatct = data.FirstOrDefault(x=>x.Email == modal.Email);
            if(conatct == null)
            {
                var _modal = new ConatctModal()
                {
                    FirstName = modal.FirstName,
                    LastName = modal.LastName,
                    Email = modal.Email,
                    Id = data.Any() ? data.Max(x=> x.Id) +1  : 1,
                };
                data?.Add(_modal);

                new ReadContactInfo(path)
                    .WriteData(data.OrderBy(x=>x.Id).ToList());

                return  Task.FromResult(_modal.Id);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// it function to delete record
        /// </summary>
        /// <param name="Id">Conatct Id</param>
        /// <param name="path">File Path</param>
        /// <returns>It is return true  and false base of record is delete or not</returns>
        public Task<bool> DeleteConatctAsync(int Id, string path)
        {
            var data = new ReadContactInfo(path).GetConatcatData();
            var conatct = data.FirstOrDefault(x => x.Id == Id);

            if(conatct != null)
            {
                data.Remove(conatct);
                new ReadContactInfo(path)
                   .WriteData(data.OrderBy(x => x.Id).ToList());

                return Task.FromResult(true);
            }
            else
            {
                return Task.FromResult(false);
            }
        }

        /// <summary>
        /// It's function retun all record
        /// </summary>
        /// <param name="path">file path</param>
        /// <returns>It's function retun all record</returns>
        public Task<List<ConatctModal>> GetConatctAsync(string path)
        {
            var data= new ReadContactInfo(path).GetConatcatData().OrderByDescending(m=>m.Id).ToList();
            return Task.FromResult(data);
        }

        /// <summary>
        /// It is return record by id
        /// </summary>
        /// <param name="Id">conatct id</param>
        /// <param name="path">file path</param>
        /// <returns>It is return record by id</returns>
        public Task<ConatctModal> GetContactByIdAsync(int Id, string path)
        {
            var data = new ReadContactInfo(path).GetConatcatData();
            var conatct = data.FirstOrDefault(x => x.Id == Id);
            if (conatct != null)
            {
                return Task.FromResult(conatct);
            }
            else
            {
                return Task.FromResult(new ConatctModal());
            }
        }

        /// <summary>
        /// it's function update record 
        /// </summary>
        /// <param name="modal">contact modal</param>
        /// <param name="Id">record id</param>
        /// <param name="path">file path</param>
        /// <returns>it is retrn stats code base of opertion</returns>
        public Task<string> UpdateConatctAsync(ConatctModalVM modal, int Id, string path)
        {
            var data = new ReadContactInfo(path).GetConatcatData();
            var conatct = data.FirstOrDefault(x => x.Id == Id);
            if (conatct != null)
            {
                data.Remove(conatct);
                var _modal = new ConatctModal()
                {
                    FirstName = modal.FirstName,
                    LastName = modal.LastName,
                    Email = modal.Email,
                    Id = Id,
                };

                data.Add(_modal);

                new ReadContactInfo(path)
                   .WriteData(data.OrderBy(x => x.Id).ToList());

                return Task.FromResult("200");
            }
            else
            {
                return Task.FromResult("404");
            }
        }
    }
}

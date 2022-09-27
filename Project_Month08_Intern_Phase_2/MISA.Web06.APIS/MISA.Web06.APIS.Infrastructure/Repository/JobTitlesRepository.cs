using Microsoft.Extensions.Configuration;
using MISA.Web06.APIS.Core.Entities;
using MISA.Web06.APIS.Core.Interfaces.Infrastructure;

namespace MISA.Web06.APIS.Infrastructure.Repository
{
    public class JobTitlesRepository : BaseRepository<JobTitle>, IJobTitlesRepository
    {
        #region Properties
        #endregion

        #region Constructor
        public JobTitlesRepository(IConfiguration configuration) : base(configuration)
        {
        }
        #endregion

        #region Methods

        #endregion
    }
}

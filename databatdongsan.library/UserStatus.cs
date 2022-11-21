using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using databatdongsan.helper;

namespace databatdongsan.library
{
    public class UserStatus
    {
        #region Properties

        public string ActBy { get; set; }
        public byte UserStatusId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? DisplayOrder { get; set; }
        public byte Display { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public UserStatus()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public UserStatus(string connection)
        {
            _connectionString = connection;
        }

        ~UserStatus()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        public async Task<List<UserStatus>> GetList()
        {
            List<UserStatus> listUserStatus;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                listUserStatus = await connection.QueryAsync<UserStatus>("SELECT * FROM [dbo].[UserStatus] WHERE [Display]=1") as List<UserStatus>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return listUserStatus;
        }

        #endregion

        #region Static Methods

        public static async Task<List<UserStatus>> Static_GetList()
        {
            UserStatus userStatus = new UserStatus();
            return await userStatus.GetList();
        }

        public static UserStatus Static_Get(byte userStatusId, List<UserStatus> list)
        {
            UserStatus retVal = new UserStatus();
            if (userStatusId > 0 && list != null && list.Count > 0)
            {
                retVal = list.Find(i => i.UserStatusId == userStatusId) ?? new UserStatus();
            }
            return retVal;
        }

        #endregion
    }
}

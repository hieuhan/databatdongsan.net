using Dapper;
using databatdongsan.helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace databatdongsan.library
{
    public class Actions
    {
        #region Properties

        public string ActBy { get; set; }
        public int ActionId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Path { get; set; }
        public int ParentActionId { get; set; }
        public byte StatusId { get; set; }
        public byte Display { get; set; }
        public string IconPath { get; set; }
        public int? DisplayOrder { get; set; }
        public byte ActionLevelId { get; set; }
        public int TreeOrder { get; set; }
        public int UserId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Actions()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Actions(string connection)
        {
            _connectionString = connection;
        }

        ~Actions()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        public List<Actions> GetByUser()
        {
            List<Actions> resultVar;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                resultVar = connection
                    .Query<Actions>("Actions_GetByUser", param, commandType: CommandType.StoredProcedure)
                         as List<Actions>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return resultVar;
        }

        #endregion

        #region Static Methods

        public static List<Actions> Static_GetByUser(string actBy, int userId)
        {
            Actions actions = new Actions
            {
                ActBy = actBy,
                UserId = userId
            };
            return actions.GetByUser();
        }

        #endregion
    }
}

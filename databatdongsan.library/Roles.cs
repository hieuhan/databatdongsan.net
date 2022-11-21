using Dapper;
using databatdongsan.helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace databatdongsan.library
{
    public class Roles
    {
        #region Properties

        public string ActBy { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte BuildIn { get; set; }
        public byte StatusId { get; set; }
        public int? DisplayOrder { get; set; }
        public int UserId { get; set; }
        public int UserRoleId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Roles()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Roles(string connection)
        {
            _connectionString = connection;
        }

        ~Roles()
        {

        }

        public virtual void Dispose()
        {

        }

        #endregion

        #region Methods

        public async Task<Tuple<string, string>> Insert()
        {
            try
            {
                return await InsertOrUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<string, string>> Update()
        {
            try
            {
                return await InsertOrUpdate();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Tuple<string, string>> InsertOrUpdate()
        {
            string actionStatus = string.Empty, actionMessage = string.Empty;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                if (!string.IsNullOrEmpty(this.Name))
                    param.Add("@Name", this.Name.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Description))
                    param.Add("@Description", this.Description.InjectionString(), DbType.String);
                param.Add("@BuildIn", this.BuildIn, DbType.Byte);
                param.Add("@StatusId", this.StatusId, DbType.Byte);
                if (this.DisplayOrder.HasValue)
                    param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                param.Add("@RoleId", this.RoleId, DbType.Int32, ParameterDirection.InputOutput);
                param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                await connection.ExecuteAsync("Roles_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                this.RoleId = param.Get<int>("RoleId");
                actionStatus = param.Get<string>("ActionStatus");
                actionMessage = param.Get<string>("ActionMessage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Tuple.Create(actionStatus, actionMessage);
        }

        public async Task<Tuple<string, string>> Delete()
        {
            string actionStatus = string.Empty, actionMessage = string.Empty;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@RoleId", this.RoleId, DbType.Int32);
                param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                await connection.ExecuteAsync("Roles_Delete", param, commandType: CommandType.StoredProcedure);
                actionStatus = param.Get<string>("ActionStatus");
                actionMessage = param.Get<string>("ActionMessage");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Tuple.Create(actionStatus, actionMessage);
        }

        public async Task<Roles> Get()
        {
            Roles retVal = new Roles();
            byte searchByDateType = 0;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            int orderByClauseId = 0, pageIndex = 0, pageSize = 1;
            Tuple<List<Roles>, int> tuple;
            try
            {
                tuple = await GetPage(dateFrom, dateTo, searchByDateType, orderByClauseId, pageIndex, pageSize);
                if (tuple.Item1 != null && tuple.Item1.Count > 0) retVal = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public async Task<Tuple<List<Roles>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, int orderByClauseId, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Roles> listRoles;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@RoleId", this.RoleId, DbType.Int32);
                if (!string.IsNullOrEmpty(this.Name))
                    param.Add("@Name", this.Name.InjectionString(), DbType.String);
                if (!string.IsNullOrEmpty(this.Description))
                    param.Add("@Description", this.Description.InjectionString(), DbType.String);
                param.Add("@BuildIn", this.BuildIn, DbType.Byte);
                param.Add("@StatusId", this.StatusId, DbType.Byte);
                param.Add("@DisplayOrder", this.DisplayOrder, DbType.Int32);
                if (!string.IsNullOrEmpty(this.CreatedBy))
                    param.Add("@CreatedBy", this.CreatedBy, DbType.String);
                if (!string.IsNullOrEmpty(this.UpdatedBy))
                    param.Add("@UpdatedBy", this.UpdatedBy, DbType.String);
                param.Add("@SearchByDateType", searchByDateType, DbType.Byte);
                if (dateFrom != DateTime.MinValue)
                    param.Add("@DateFrom", dateFrom, DbType.DateTime);
                if (dateTo != DateTime.MinValue)
                    param.Add("@DateTo", dateTo, DbType.DateTime);
                param.Add("@OrderByClauseId", orderByClauseId, DbType.Int32);
                param.Add("@UserId", this.UserId, DbType.Int32);
                param.Add("@PageSize", pageSize, DbType.Int32);
                param.Add("@PageIndex", pageIndex, DbType.Int32);
                param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                listRoles = await connection
                    .QueryAsync<Roles>("Roles_GetPage", param, commandType: CommandType.StoredProcedure)
                     as List<Roles>;
                rowCount = param.Get<int>("RowCount");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return Tuple.Create(listRoles, rowCount);
        }

        public async Task<List<Roles>> GetList()
        {
            List<Roles> listRoles;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();
                listRoles = await connection.QueryAsync<Roles>("SELECT * FROM [dbo].[Roles]") as List<Roles>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return listRoles;
        }

        public async Task<List<Roles>> GetListBy()
        {
            List<Roles> rolesList;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);

                rolesList = await connection
                    .QueryAsync<Roles>("Roles_GetListBy", param, commandType: CommandType.StoredProcedure)
                     as List<Roles>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return rolesList;
        }

        #endregion

        #region Static Methods

        public static async Task<List<Roles>> Static_GetList()
        {
            Roles roles = new Roles();
            return await roles.GetList();
        }

        public static async Task<List<Roles>> Static_GetListBy(string actBy, int userId)
        {
            Roles roles = new Roles
            {
                ActBy = actBy,
                UserId = userId
            };
            return await roles.GetListBy();
        }

        public static Roles Static_Get(byte roleId, List<Roles> list)
        {
            Roles retVal = new Roles();
            if (roleId > 0 && list != null && list.Count > 0)
            {
                retVal = list.Find(i => i.RoleId == roleId) ?? new Roles();
            }
            return retVal;
        }

        public static async Task<Roles> Static_GetById(string actBy, int roleId)
        {
            Roles roles = new Roles
            {
                ActBy = actBy,
                RoleId = roleId
            };
            return await roles.Get();
        }

        #endregion
    }
}

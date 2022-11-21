using Dapper;
using databatdongsan.helper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Threading.Tasks;

namespace databatdongsan.library
{
    public class Customers
    {
        #region Properties

        public string ActBy { get; set; }
        public int CustomerId { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public int UserId { get; set; }
        public string Note { get; set; }
        public byte StatusId { get; set; }
        public int Counter { get; set; }
        public int Total { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CrDateTime { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdDateTime { get; set; }

        private readonly string _connectionString;

        #endregion

        #region Constructors

        public Customers()
        {
            _connectionString = ConstantHelper.CommonConstr;
        }

        public Customers(string connection)
        {
            _connectionString = connection;
        }

        ~Customers()
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
                if (!string.IsNullOrEmpty(this.Mobile))
                    param.Add("@Mobile", StringHelper.InjectionString(this.Mobile), DbType.String);
                if (!string.IsNullOrEmpty(this.Email))
                    param.Add("@Email", StringHelper.InjectionString(this.Email), DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                if (!string.IsNullOrEmpty(this.Note))
                    param.Add("@Note", this.Note, DbType.String);
                param.Add("@CustomerId", this.CustomerId, DbType.Int32, ParameterDirection.InputOutput);
                param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                await connection.ExecuteAsync("Customers_InsertOrUpdate", param, commandType: CommandType.StoredProcedure);
                this.CustomerId = param.Get<int>("CustomerId");
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
                param.Add("@CustomerId", this.CustomerId, DbType.Int32);
                param.Add("@ActionStatus", dbType: DbType.String, direction: ParameterDirection.Output, size: 50);
                param.Add("@ActionMessage", dbType: DbType.String, direction: ParameterDirection.Output, size: 250);
                await connection.ExecuteAsync("Customers_Delete", param, commandType: CommandType.StoredProcedure);
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

        public async Task<Tuple<string, string, int, int, int>> Import(DataTable dtCustomers)
        {
            int rowsInserted = 0, rowsUpdated = 0, rowsDeleted = 0;
            string actionStatus = string.Empty, actionMessage = string.Empty;
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("Customers_Import"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = connection;
                        if(!string.IsNullOrWhiteSpace(this.ActBy))
                            cmd.Parameters.Add(new SqlParameter("@ActBy", this.ActBy));
                        cmd.Parameters.Add(new SqlParameter("@UserId", this.UserId));
                        cmd.Parameters.AddWithValue("@tblCustomers", dtCustomers);
                        cmd.Parameters.Add("@RowsInserted", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RowsUpdated", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@RowsDeleted", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ActionStatus", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@ActionMessage", SqlDbType.NVarChar, 250).Direction = ParameterDirection.Output;
                        connection.Open();
                        await cmd.ExecuteNonQueryAsync();
                        connection.Close();

                        rowsInserted = Convert.ToInt32(cmd.Parameters["@RowsInserted"].Value is DBNull ? "0" : cmd.Parameters["@RowsInserted"].Value.ToString());
                        rowsUpdated = Convert.ToInt32(cmd.Parameters["@RowsUpdated"].Value is DBNull ? "0" : cmd.Parameters["@RowsUpdated"].Value.ToString());
                        rowsDeleted = Convert.ToInt32(cmd.Parameters["@RowsDeleted"].Value is DBNull ? "0" : cmd.Parameters["@RowsDeleted"].Value.ToString());
                        actionStatus = cmd.Parameters["@ActionStatus"].Value is DBNull ? string.Empty : cmd.Parameters["@ActionStatus"].Value.ToString();
                        actionMessage = cmd.Parameters["@ActionMessage"].Value is DBNull ? string.Empty : cmd.Parameters["@ActionMessage"].Value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Tuple.Create(actionStatus, actionMessage, rowsInserted, rowsUpdated, rowsDeleted);
        }

        public async Task<Customers> Get()
        {
            Customers retVal = new Customers();
            string keywords = string.Empty, orderBy = string.Empty;
            int pageIndex = 0, pageSize = 1;
            DateTime dateFrom = DateTime.MinValue, dateTo = DateTime.MinValue;
            byte searchByDateType = 0;
            Tuple<List<Customers>, int> tuple;
            try
            {
                tuple = await GetPage(dateFrom, dateTo, searchByDateType, keywords, orderBy, pageIndex, pageSize);
                if (tuple.Item1 != null && tuple.Item1.Count > 0) retVal = tuple.Item1[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return retVal;
        }

        public async Task<Tuple<List<Customers>, int>> GetPage(DateTime dateFrom, DateTime dateTo, byte searchByDateType, string searchKeyword, string orderBy, int pageIndex, int pageSize)
        {
            int rowCount = 0;
            List<Customers> listCustomers;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                if (!string.IsNullOrEmpty(searchKeyword))
                    param.Add("@SearchKeyword", searchKeyword.InjectionString(), DbType.String);
                param.Add("@CustomerId", this.CustomerId, DbType.Int32);
                if (!string.IsNullOrEmpty(this.Mobile))
                    param.Add("@Mobile", StringHelper.InjectionString(this.Mobile), DbType.String);
                if (!string.IsNullOrEmpty(this.Email))
                    param.Add("@Email", StringHelper.InjectionString(this.Email), DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                if (!string.IsNullOrEmpty(this.CreatedBy))
                    param.Add("@CreatedBy", this.CreatedBy, DbType.String);
                if (!string.IsNullOrEmpty(this.UpdatedBy))
                    param.Add("@UpdatedBy", this.UpdatedBy, DbType.String);
                if (!string.IsNullOrWhiteSpace(orderBy))
                    param.Add("@OrderBy", orderBy.InjectionString(), DbType.String);
                param.Add("@SearchByDateType", searchByDateType, DbType.Byte);
                if (dateFrom != DateTime.MinValue)
                    param.Add("@DateFrom", dateFrom, DbType.DateTime);
                if (dateTo != DateTime.MinValue)
                    param.Add("@DateTo", dateTo, DbType.DateTime);
                param.Add("@PageSize", pageSize, DbType.Int32);
                param.Add("@PageIndex", pageIndex, DbType.Int32);
                param.Add("@RowCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                listCustomers = await connection
                    .QueryAsync<Customers>("Customers_GetPage", param, commandType: CommandType.StoredProcedure)
                     as List<Customers>;
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

            return Tuple.Create(listCustomers, rowCount);
        }

        public async Task<List<Customers>> GetReport(DateTime dateFrom, DateTime dateTo)
        {
            List<Customers> customersList;
            SqlConnection connection = new SqlConnection(_connectionString);
            try
            {
                if (connection.State == ConnectionState.Closed)
                    await connection.OpenAsync();

                DynamicParameters param = new DynamicParameters();
                if (!string.IsNullOrEmpty(this.ActBy))
                    param.Add("@ActBy", this.ActBy, DbType.String);
                param.Add("@UserId", this.UserId, DbType.Int32);
                if (dateFrom != DateTime.MinValue)
                    param.Add("@DateFrom", dateFrom, DbType.DateTime);
                if (dateTo != DateTime.MinValue)
                    param.Add("@DateTo", dateTo, DbType.DateTime);
                customersList = await connection
                    .QueryAsync<Customers>("Customers_GetReport", param, commandType: CommandType.StoredProcedure)
                     as List<Customers>;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }

            return customersList;
        }

        #endregion

        #region Static Methods

        public static Customers Static_Get(byte customerId, List<Customers> list)
        {
            Customers retVal = new Customers();
            if (customerId > 0 && list != null && list.Count > 0)
            {
                retVal = list.Find(i => i.CustomerId == customerId) ?? new Customers();
            }
            return retVal;
        }

        public static async Task<Customers> Static_GetById(string actBy, int customerId)
        {
            Customers customers = new Customers
            {
                ActBy = actBy,
                CustomerId = customerId
            };

            return await customers.Get();
        }

        public static async Task<Tuple<string, string, int, int, int>> Static_Import(string actBy, int userId, DataTable dtCustomers)
        {
            Customers customers = new Customers
            {
                ActBy = actBy,
                UserId = userId
            };

            return await customers.Import(dtCustomers);
        }

        public static async Task<List<Customers>> Static_GetReport(string actBy, int userId, DateTime dateFrom, DateTime dateTo)
        {
            Customers customers = new Customers
            {
                ActBy = actBy,
                UserId = userId
            };

            return await customers.GetReport(dateFrom, dateTo);
        }

        #endregion
    }
}

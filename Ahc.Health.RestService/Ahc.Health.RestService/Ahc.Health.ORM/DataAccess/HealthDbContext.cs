using System;
using System.Collections.Generic;
using System.Linq;
using Ahc.Health.ORM.Repository;
using System.Data;
using Dapper;
using System.IO;
//using LendingApp.ExtensionMethods;
using System.Configuration;
using Ahc.Health.Common.Constant;
using Ahc.Health.Model;
//using Microsoft.AspNet.Identity.Owin;

namespace Ahc.Health.ORM.DataAccess
{
    public class HealthDbContext : BaseRepository, IDisposable
    {
        public string GetConnection()
        {
            return string.Empty;
        }


        public User ValidateUser(User user)
        {
            using (IDbConnection connection = OpenConnection())
            {
                const string query = "Select Id FROM [Users] WHERE IsActive=1 and Username = @UserName and Password = @Password";
                return connection.Query<User>(query, user).FirstOrDefault();

            }
        }

        public IEnumerable<User> GetRegisteredUsers()
        {
            using (IDbConnection connection = OpenConnection())
            {
                var users = connection.Query<User>(StoredProcedure.uspGetRegisteredUssers, commandType: CommandType.StoredProcedure);
                
                return users;
            }
        }

        public HealthDataCollected AddHealthData(IEnumerable<HealthDataCollected> healthDatas)
        {
            using (IDbConnection connection = OpenConnection())
            {
                //var dParam = new DynamicParameters();
                //dParam.Add("@Id", healthData.Id);
                //dParam.Add("@UserId", healthData.UserId);
                //dParam.Add("@HealthMetric", healthData.HealthMetric);
                //dParam.Add("@JsonString", healthData.JsonString);
                //dParam.Add("@Status", healthData.Status);
                //dParam.Add("@PredictiveStatus", healthData.PredictiveStatus);
                //dParam.Add("@RetVal", dbType: DbType.Int32, direction: ParameterDirection.Output);

              var healthDataCollected =  connection.Execute(StoredProcedure.uspAddHealthData, healthDatas, commandType: CommandType.StoredProcedure);

                return healthDatas.Single();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~HealthDbContext() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion


    }
}

using System;
using System.Text;

using TotalBase;
using TotalBase.Enums;
using TotalModel.Models;

namespace TotalDAL.Helpers.SqlProgrammability.Commons
{
    public class Employee
    {
        private readonly TotalSmartCodingEntities totalSmartCodingEntities;

        public Employee(TotalSmartCodingEntities totalSmartCodingEntities)
        {
            this.totalSmartCodingEntities = totalSmartCodingEntities;
        }

        public void RestoreProcedure()
        {
            this.GetEmployeeIndexes();

            //this.EmployeeEditable(); 
            //this.EmployeeSaveRelative();

            this.GetEmployeeBases();
        }


        private void GetEmployeeIndexes()
        {
            string queryString;

            queryString = " @AspUserID nvarchar(128), @FromDate DateTime, @ToDate DateTime " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      Employees.EmployeeID, Employees.Code, Employees.Name " + "\r\n";
            queryString = queryString + "       FROM        Employees " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetEmployeeIndexes", queryString);
        }


        private void EmployeeSaveRelative()
        {
            string queryString = " @EntityID int, @SaveRelativeOption int " + "\r\n"; //SaveRelativeOption: 1: Update, -1:Undo
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       IF (@SaveRelativeOption = 1) " + "\r\n";
            queryString = queryString + "           BEGIN " + "\r\n";

            queryString = queryString + "               INSERT INTO EmployeeWarehouses (EmployeeID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      EmployeeID, 46 AS WarehouseID, " + (int)GlobalEnums.NmvnTaskID.SalesOrder + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Employees WHERE EmployeeID = @EntityID " + "\r\n";

            queryString = queryString + "               INSERT INTO EmployeeWarehouses (EmployeeID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      Employees.EmployeeID, Warehouses.WarehouseID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Employees INNER JOIN Warehouses ON Employees.EmployeeID = @EntityID AND Employees.EmployeeCategoryID NOT IN (4, 5, 7, 9, 10, 11, 12) AND Employees.EmployeeCategoryID = Warehouses.WarehouseCategoryID " + "\r\n";

            queryString = queryString + "               INSERT INTO EmployeeWarehouses (EmployeeID, WarehouseID, WarehouseTaskID, EntryDate, Remarks, InActive) " + "\r\n";
            queryString = queryString + "               SELECT      EmployeeID, 82 AS WarehouseID, " + (int)GlobalEnums.NmvnTaskID.DeliveryAdvice + " AS WarehouseTaskID, GETDATE(), '', 0 FROM Employees WHERE EmployeeID = @EntityID AND EmployeeCategoryID IN (4, 5, 7, 9, 10, 11, 12) " + "\r\n";

            queryString = queryString + "           END " + "\r\n";

            queryString = queryString + "       ELSE " + "\r\n"; //(@SaveRelativeOption = -1) 
            queryString = queryString + "           DELETE      EmployeeWarehouses WHERE EmployeeID = @EntityID " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("EmployeeSaveRelative", queryString);
        }


        private void EmployeeEditable()
        {
            string[] queryArray = new string[0];

            //queryArray[0] = " SELECT TOP 1 @FoundEntity = EmployeeID FROM Employees WHERE EmployeeID = @EntityID AND (InActive = 1 OR InActivePartial = 1)"; //Don't allow approve after void
            //queryArray[1] = " SELECT TOP 1 @FoundEntity = EmployeeID FROM GoodsIssueDetails WHERE EmployeeID = @EntityID ";

            this.totalSmartCodingEntities.CreateProcedureToCheckExisting("EmployeeEditable", queryArray);
        }


        private void GetEmployeeBases()
        {
            string queryString;

            queryString = " " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "    BEGIN " + "\r\n";

            queryString = queryString + "       SELECT      EmployeeID, Code, Name " + "\r\n";
            queryString = queryString + "       FROM        Employees " + "\r\n";

            queryString = queryString + "    END " + "\r\n";

            this.totalSmartCodingEntities.CreateStoredProcedure("GetEmployeeBases", queryString);
        }

    }
}

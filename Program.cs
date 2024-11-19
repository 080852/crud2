using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CompanyApp
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Position { get; set; }
        public decimal Salary { get; set; }
    }

    public class EmployeeRepository
    {
        private string connectionString = "server=INBOOK_X3_SLIM\\SQLEXPRESS;database=CompanyDB;trusted_connection=true;";

        // Add a new employee
        public void AddEmployee(Employee employee)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Employees (FirstName, LastName, Position, Salary) VALUES (@FirstName, @LastName, @Position, @Salary)";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", employee.FirstName);
                command.Parameters.AddWithValue("@LastName", employee.LastName);
                command.Parameters.AddWithValue("@Position", employee.Position);
                command.Parameters.AddWithValue("@Salary", employee.Salary);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Retrieve all employees by position
        public List<Employee> GetEmployeesByPosition(string position)
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees WHERE Position = @Position";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Position", position);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Position = reader["Position"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
            }
            return employees;
        }

        // Update employee salary
        public void UpdateEmployeeSalary(int employeeId, decimal percentageIncrease)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Employees SET Salary = Salary + (Salary * @Percentage / 100) WHERE EmployeeId = @EmployeeId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Percentage", percentageIncrease);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Delete employee
        public void DeleteEmployee(int employeeId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@EmployeeId", employeeId);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Get employees with salary above a certain amount
        public List<Employee> GetEmployeesBySalary(decimal minimumSalary)
        {
            List<Employee> employees = new List<Employee>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM Employees WHERE Salary > @MinimumSalary";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MinimumSalary", minimumSalary);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    employees.Add(new Employee
                    {
                        EmployeeId = Convert.ToInt32(reader["EmployeeId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        Position = reader["Position"].ToString(),
                        Salary = Convert.ToDecimal(reader["Salary"])
                    });
                }
            }
            return employees;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            EmployeeRepository repo = new EmployeeRepository();

            // Add employees
            repo.AddEmployee(new Employee { FirstName = "John", LastName = "Doe", Position = "Software Engineer", Salary = 90000 });
            repo.AddEmployee(new Employee { FirstName = "Jane", LastName = "Smith", Position = "Project Manager", Salary = 120000 });
            repo.AddEmployee(new Employee { FirstName = "Alice", LastName = "Brown", Position = "HR", Salary = 70000 });

            // Retrieve employees by position
            var engineers = repo.GetEmployeesByPosition("Software Engineer");
            Console.WriteLine("Software Engineers:");
            foreach (var emp in engineers)
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName}, Position: {emp.Position}, Salary: {emp.Salary}");
            }

            // Update employee salary
            repo.UpdateEmployeeSalary(1, 10);
            var updatedEmployee = repo.GetEmployeesByPosition("Software Engineer");
            Console.WriteLine("After Salary Update:");
            foreach (var emp in updatedEmployee)
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName}, Position: {emp.Position}, Salary: {emp.Salary}");
            }

            // Delete employee
            repo.DeleteEmployee(2);
            Console.WriteLine("After Deletion:");
            var allEmployees = repo.GetEmployeesBySalary(0); // Fetch all employees
            foreach (var emp in allEmployees)
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName}, Position: {emp.Position}, Salary: {emp.Salary}");
            }

            // Employees with salary above a certain amount
            Console.WriteLine("Employees earning above 100000:");
            var highSalaryEmployees = repo.GetEmployeesBySalary(100000);
            foreach (var emp in highSalaryEmployees)
            {
                Console.WriteLine($"{emp.FirstName} {emp.LastName}, Position: {emp.Position}, Salary: {emp.Salary}");
            }
            Console.ReadLine();
        }
    }
}

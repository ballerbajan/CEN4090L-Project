using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;

namespace CollegeCompanion.Library.Services
{
    public class DatabaseService
    {
        private static DatabaseService? _instance;
        private readonly SQLiteConnection _database;
        private const string DB_NAME = "college_companion.db";

        // Store current user in memory
        public User? CurrentUser { get; private set; }

        public static DatabaseService Current
        {
            get
            {
                _instance ??= new DatabaseService();
                return _instance;
            }
        }

        private DatabaseService()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(appDataPath, DB_NAME);
            Console.WriteLine($"[DATABASE] Database path: {dbPath}");

            _database = new SQLiteConnection(dbPath);
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            try
            {
                _database.CreateTable<UserTable>();
                _database.CreateTable<ExpenseTable>();
                _database.CreateTable<BudgetTable>();
                Console.WriteLine("[DATABASE] All tables created/verified");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DATABASE] Initialization error: {ex.Message}");
            }
        }

        // ============ PASSWORD HASHING ============

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var saltBytes = System.Security.Cryptography.RandomNumberGenerator.GetBytes(16);
            var passwordBytes = System.Text.Encoding.UTF8.GetBytes(password);
            var saltedPassword = new byte[saltBytes.Length + passwordBytes.Length];

            Buffer.BlockCopy(saltBytes, 0, saltedPassword, 0, saltBytes.Length);
            Buffer.BlockCopy(passwordBytes, 0, saltedPassword, saltBytes.Length, passwordBytes.Length);

            var hashBytes = sha256.ComputeHash(saltedPassword);
            var salt = Convert.ToBase64String(saltBytes);
            var hash = Convert.ToBase64String(hashBytes);

            return $"{salt}${hash}";
        }

        private bool VerifyPassword(string storedHash, string providedPassword)
        {
            try
            {
                var parts = storedHash.Split('$');
                if (parts.Length != 2) return false;

                var saltBytes = Convert.FromBase64String(parts[0]);
                var storedHashBytes = Convert.FromBase64String(parts[1]);

                using var sha256 = System.Security.Cryptography.SHA256.Create();
                var passwordBytes = System.Text.Encoding.UTF8.GetBytes(providedPassword);
                var saltedPassword = new byte[saltBytes.Length + passwordBytes.Length];

                Buffer.BlockCopy(saltBytes, 0, saltedPassword, 0, saltBytes.Length);
                Buffer.BlockCopy(passwordBytes, 0, saltedPassword, saltBytes.Length, passwordBytes.Length);

                var hashBytes = sha256.ComputeHash(saltedPassword);

                return storedHashBytes.SequenceEqual(hashBytes);
            }
            catch
            {
                return false;
            }
        }

        // ============ USER AUTHENTICATION ============

        public bool Login(string username, string password)
        {
            try
            {
                Console.WriteLine($"[DB LOGIN] Searching for: '{username}'");

                var user = _database.Table<UserTable>()
                    .FirstOrDefault(u => u.Username == username);

                if (user != null)
                {
                    Console.WriteLine($"[DB LOGIN] User found: {user.Username}");
                    Console.WriteLine($"[DB LOGIN] Verifying password...");

                    bool verified = VerifyPassword(user.PasswordHash, password);
                    Console.WriteLine($"[DB LOGIN] Password verified: {verified}");

                    if (verified)
                    {
                        CurrentUser = new User
                        {
                            Id = user.Id,
                            Username = user.Username,
                            Email = user.Email,
                            Name = user.Name,
                            Income = user.Income,
                            CreatedAt = user.CreatedAt
                        };
                        Console.WriteLine($"[DB LOGIN] CurrentUser set: {CurrentUser.Username}, Income: {CurrentUser.Income}");
                        return true;
                    }
                }
                else
                {
                    Console.WriteLine($"[DB LOGIN] User not found: '{username}'");
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB LOGIN] Error: {ex.Message}");
                return false;
            }
        }

        public bool Register(string username, string password, string email = "", string name = "")
        {
            try
            {
                Console.WriteLine($"[DB REGISTER] Registering: {username}");

                var existingUser = _database.Table<UserTable>()
                    .FirstOrDefault(u => u.Username == username);

                if (existingUser != null)
                {
                    Console.WriteLine($"[DB REGISTER] Username already exists: {username}");
                    return false;
                }

                var newUser = new UserTable
                {
                    Username = username,
                    PasswordHash = HashPassword(password),
                    Email = email,
                    Name = name,
                    Income = 0m,
                    CreatedAt = DateTime.Now
                };

                _database.Insert(newUser);
                Console.WriteLine($"[DB REGISTER] User registered successfully: {username}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB REGISTER] Error: {ex.Message}");
                return false;
            }
        }

        public User? GetCurrentUser()
        {
            return CurrentUser;
        }

        public void Logout()
        {
            CurrentUser = null;
            Console.WriteLine("[DB LOGOUT] User logged out successfully");
        }

        // ============ INCOME METHODS ============

        public bool UpdateIncome(decimal income)
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("[DB] Cannot update income: No user logged in");
                return false;
            }

            try
            {
                var user = _database.Table<UserTable>()
                    .FirstOrDefault(u => u.Id == CurrentUser.Id);

                if (user != null)
                {
                    user.Income = income;
                    _database.Update(user);

                    // Update current user in memory
                    CurrentUser.Income = income;

                    Console.WriteLine($"[DB] Income updated for {CurrentUser.Username}: ${income}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] Update income error: {ex.Message}");
                return false;
            }
        }

        public decimal GetIncome()
        {
            return CurrentUser?.Income ?? 0m;
        }

        // ============ EXPENSE METHODS ============

        public bool AddExpense(string description, decimal amount, string category)
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("[DB] Cannot add expense: No user logged in");
                return false;
            }

            try
            {
                var expense = new ExpenseTable
                {
                    UserId = CurrentUser.Id,
                    Description = description,
                    Amount = amount,
                    Category = category,
                    Date = DateTime.Now
                };

                _database.Insert(expense);
                Console.WriteLine($"[DB] Expense added for user {CurrentUser.Username}: {description} - ${amount}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] Add expense error: {ex.Message}");
                return false;
            }
        }

        public List<ExpenseData> GetUserExpenses()
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("[DB] Cannot get expenses: No user logged in");
                return new List<ExpenseData>();
            }

            try
            {
                var expenses = _database.Table<ExpenseTable>()
                    .Where(e => e.UserId == CurrentUser.Id)
                    .OrderByDescending(e => e.Date)
                    .ToList();

                Console.WriteLine($"[DB] Retrieved {expenses.Count} expenses for user {CurrentUser.Username}");

                return expenses.Select(e => new ExpenseData
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    Description = e.Description,
                    Amount = e.Amount,
                    Category = e.Category,
                    Date = e.Date
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] Get expenses error: {ex.Message}");
                return new List<ExpenseData>();
            }
        }

        public List<ExpenseData> GetExpensesByCategory(string category)
        {
            if (CurrentUser == null) return new List<ExpenseData>();

            try
            {
                var expenses = _database.Table<ExpenseTable>()
                    .Where(e => e.UserId == CurrentUser.Id && e.Category == category)
                    .OrderByDescending(e => e.Date)
                    .ToList();

                return expenses.Select(e => new ExpenseData
                {
                    Id = e.Id,
                    UserId = e.UserId,
                    Description = e.Description,
                    Amount = e.Amount,
                    Category = e.Category,
                    Date = e.Date
                }).ToList();
            }
            catch
            {
                return new List<ExpenseData>();
            }
        }

        public bool DeleteExpense(int expenseId)
        {
            if (CurrentUser == null) return false;

            try
            {
                var expense = _database.Table<ExpenseTable>()
                    .FirstOrDefault(e => e.Id == expenseId && e.UserId == CurrentUser.Id);

                if (expense != null)
                {
                    _database.Delete(expense);
                    Console.WriteLine($"[DB] Expense deleted: {expenseId}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] Delete expense error: {ex.Message}");
                return false;
            }
        }

        public bool UpdateExpense(int expenseId, string description, decimal amount, string category)
        {
            if (CurrentUser == null) return false;

            try
            {
                var expense = _database.Table<ExpenseTable>()
                    .FirstOrDefault(e => e.Id == expenseId && e.UserId == CurrentUser.Id);

                if (expense != null)
                {
                    expense.Description = description;
                    expense.Amount = amount;
                    expense.Category = category;
                    _database.Update(expense);
                    Console.WriteLine($"[DB] Expense updated: {expenseId}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] Update expense error: {ex.Message}");
                return false;
            }
        }

        public decimal GetTotalExpenses()
        {
            if (CurrentUser == null) return 0;

            try
            {
                return _database.Table<ExpenseTable>()
                    .Where(e => e.UserId == CurrentUser.Id)
                    .Sum(e => e.Amount);
            }
            catch
            {
                return 0;
            }
        }

        public decimal GetTotalExpensesByCategory(string category)
        {
            if (CurrentUser == null) return 0;

            try
            {
                return _database.Table<ExpenseTable>()
                    .Where(e => e.UserId == CurrentUser.Id && e.Category == category)
                    .Sum(e => e.Amount);
            }
            catch
            {
                return 0;
            }
        }

        // ============ BUDGET METHODS ============

        public bool AddBudget(string category, decimal amount, int month, int year)
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("[DB] Cannot add budget: No user logged in");
                return false;
            }

            try
            {
                var existing = _database.Table<BudgetTable>()
                    .FirstOrDefault(b => b.UserId == CurrentUser.Id &&
                                       b.Category == category &&
                                       b.Month == month &&
                                       b.Year == year);

                if (existing != null)
                {
                    existing.Amount = amount;
                    _database.Update(existing);
                    Console.WriteLine($"[DB] Budget updated for user {CurrentUser.Username}");
                    return true;
                }

                var budget = new BudgetTable
                {
                    UserId = CurrentUser.Id,
                    Category = category,
                    Amount = amount,
                    Month = month,
                    Year = year
                };

                _database.Insert(budget);
                Console.WriteLine($"[DB] Budget added for user {CurrentUser.Username}: {category} - ${amount}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] Add budget error: {ex.Message}");
                return false;
            }
        }

        public List<BudgetData> GetUserBudgets()
        {
            if (CurrentUser == null)
            {
                Console.WriteLine("[DB] Cannot get budgets: No user logged in");
                return new List<BudgetData>();
            }

            try
            {
                var budgets = _database.Table<BudgetTable>()
                    .Where(b => b.UserId == CurrentUser.Id)
                    .ToList();

                Console.WriteLine($"[DB] Retrieved {budgets.Count} budgets for user {CurrentUser.Username}");

                return budgets.Select(b => new BudgetData
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    Category = b.Category,
                    Amount = b.Amount,
                    Month = b.Month,
                    Year = b.Year
                }).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] Get budgets error: {ex.Message}");
                return new List<BudgetData>();
            }
        }

        public BudgetData? GetBudgetForCategory(string category, int month, int year)
        {
            if (CurrentUser == null) return null;

            try
            {
                var budget = _database.Table<BudgetTable>()
                    .FirstOrDefault(b => b.UserId == CurrentUser.Id &&
                                       b.Category == category &&
                                       b.Month == month &&
                                       b.Year == year);

                if (budget != null)
                {
                    return new BudgetData
                    {
                        Id = budget.Id,
                        UserId = budget.UserId,
                        Category = budget.Category,
                        Amount = budget.Amount,
                        Month = budget.Month,
                        Year = budget.Year
                    };
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateBudget(int budgetId, decimal newAmount)
        {
            if (CurrentUser == null) return false;

            try
            {
                var budget = _database.Table<BudgetTable>()
                    .FirstOrDefault(b => b.Id == budgetId && b.UserId == CurrentUser.Id);

                if (budget != null)
                {
                    budget.Amount = newAmount;
                    _database.Update(budget);
                    Console.WriteLine($"[DB] Budget updated: {budgetId}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] Update budget error: {ex.Message}");
                return false;
            }
        }

        public bool DeleteBudget(int budgetId)
        {
            if (CurrentUser == null) return false;

            try
            {
                var budget = _database.Table<BudgetTable>()
                    .FirstOrDefault(b => b.Id == budgetId && b.UserId == CurrentUser.Id);

                if (budget != null)
                {
                    _database.Delete(budget);
                    Console.WriteLine($"[DB] Budget deleted: {budgetId}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DB] Delete budget error: {ex.Message}");
                return false;
            }
        }

        // ============ HELPER METHODS ============

        public int GetUserCount()
        {
            try
            {
                return _database.Table<UserTable>().Count();
            }
            catch
            {
                return 0;
            }
        }

        public bool IsUserLoggedIn()
        {
            return CurrentUser != null;
        }
    }

    // ============ DATABASE TABLE CLASSES ============

    public class UserTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Unique]
        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public decimal Income { get; set; } = 0m;

        public DateTime CreatedAt { get; set; }
    }

    public class ExpenseTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; }

        public string Description { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public string Category { get; set; } = string.Empty;

        public DateTime Date { get; set; }
    }

    public class BudgetTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int UserId { get; set; }

        public string Category { get; set; } = string.Empty;

        public decimal Amount { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }
    }

    // ============ MODEL CLASSES ============

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Income { get; set; } = 0m;
        public DateTime CreatedAt { get; set; }
    }

    public class ExpenseData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string Category { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

    public class BudgetData
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Category { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
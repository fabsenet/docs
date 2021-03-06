﻿using System;
using System.Collections.Generic;
using System.Linq;

using Raven.Abstractions.Indexing;
using Raven.Client;
using Raven.Client.Document;
using Raven.Client.Indexes;
using Raven.Client.Linq;
using Raven.Documentation.CodeSamples.Orders;

namespace Raven.Documentation.Samples.Indexes
{
	public class Map
	{
		/*
		#region indexes_1
		public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
		{
			// ...
		}
		#endregion
		*/

		/*
		public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
		{
			#region indexes_2
			public Employees_ByFirstAndLastName()
			{
				Map = employees => from employee in employees
							select new
							{
								FirstName = employee.FirstName,
								LastName = employee.LastName
							};
			}
			#endregion
		}
		*/

		public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
		{
			#region indexes_3
			public Employees_ByFirstAndLastName()
			{
				Map = employees => employees
					.Select(employee => new
					{
						FirstName = employee.FirstName,
						LastName = employee.LastName
					});
			}
			#endregion
		}

		/*
		#region indexes_6
		public class Employees_ByFirstAndLastName : AbstractIndexCreationTask<Employee>
		{
			public Employees_ByFirstAndLastName()
			{
				Map = employees => from employee in employees
							select new
							{
								FirstName = employee.FirstName,
								LastName = employee.LastName
							};
			}
		}
		#endregion
		*/

		#region indexes_7
		public class Employees_ByFullName : AbstractIndexCreationTask<Employee>
		{
			public class Result
			{
				public string FullName { get; set; }
			}

			public Employees_ByFullName()
			{
				Map = employees => from employee in employees
							select new
							{
								FullName = employee.FirstName + " " + employee.LastName
							};
			}
		}
		#endregion

		#region indexes_1_0
		public class Employees_ByYearOfBirth : AbstractIndexCreationTask<Employee>
		{
			public class Result
			{
				public int YearOfBirth { get; set; }
			}

			public Employees_ByYearOfBirth()
			{
				Map = employees => from employee in employees
							select new
							{
								YearOfBirth = employee.Birthday.Year
							};
			}
		}
		#endregion

		#region indexes_1_2
		public class Employees_ByBirthday : AbstractIndexCreationTask<Employee>
		{
			public class Result
			{
				public DateTime Birthday { get; set; }
			}

			public Employees_ByBirthday()
			{
				Map = employees => from employee in employees
							select new
							{
								Birthday = employee.Birthday
							};
			}
		}
		#endregion

		#region indexes_1_4
		public class Employees_ByCountry : AbstractIndexCreationTask<Employee>
		{
			public class Result
			{
				public string Country { get; set; }
			}

			public Employees_ByCountry()
			{
				Map = employees => from employee in employees
							select new
							{
								Country = employee.Address.Country
							};
			}
		}
        #endregion

        #region indexes_1_6
        public class Employees_Query : AbstractIndexCreationTask<Employee>
        {
            public class Result
            {
                public string[] Query { get; set; }
            }

            public Employees_Query()
            {
                Map = employees => from employee in employees
                                   select new
                                   {
                                        Query = new[]
                                        {
                                            employee.FirstName,
                                            employee.LastName,
                                            employee.Title,
                                            employee.Address.City
                                        }
                                   };

                Index("Query", FieldIndexing.Analyzed);
            }
        }
        #endregion

        public Map()
		{
			using (var store = new DocumentStore())
			{
				using (var session = store.OpenSession())
				{
					#region indexes_4
					IList<Employee> employees = session
						.Query<Employee, Employees_ByFirstAndLastName>()
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_5
					IList<Employee> employees = session
						.Query<Employee>("Employees/ByFirstAndLastName")
						.Where(x => x.FirstName == "Robert")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_8
					// notice that we're 'cheating' here
					// by marking result type in 'Query' as 'Employees_ByFullName.Result' to get strongly-typed syntax
					// and changing type using 'OfType' before sending query to server
					IList<Employee> employees = session
						.Query<Employees_ByFullName.Result, Employees_ByFullName>()
						.Where(x => x.FullName == "Robert King")
						.OfType<Employee>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_9
					IList<Employee> employees = session
						.Advanced
						.DocumentQuery<Employee, Employees_ByFullName>()
						.WhereEquals("FullName", "Robert King")
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_1_1
					IList<Employee> employees = session
						.Query<Employees_ByYearOfBirth.Result, Employees_ByYearOfBirth>()
						.Where(x => x.YearOfBirth == 1963)
						.OfType<Employee>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_1_3
					DateTime startDate = new DateTime(1963, 1, 1);
					DateTime endDate = startDate.AddYears(1).AddMilliseconds(-1);
					IList<Employee> employees = session
						.Query<Employees_ByBirthday.Result, Employees_ByBirthday>()
						.Where(x => x.Birthday >= startDate && x.Birthday <= endDate)
						.OfType<Employee>()
						.ToList();
					#endregion
				}

				using (var session = store.OpenSession())
				{
					#region indexes_1_5
					IList<Employee> employees = session
						.Query<Employees_ByCountry.Result, Employees_ByCountry>()
						.Where(x => x.Country == "USA")
						.OfType<Employee>()
						.ToList();
					#endregion
				}

                using (var session = store.OpenSession())
                {
                    #region indexes_1_7
                    IList<Employee> employees = session
                        .Query<Employees_Query.Result, Employees_Query>()
                        .Search(x => x.Query, "John Doe")
                        .OfType<Employee>()
                        .ToList();
                    #endregion
                }

                using (var session = store.OpenSession())
                {
                    #region indexes_1_8
                    IList<Employee> employees = session
                        .Advanced
                        .DocumentQuery<Employees_Query.Result, Employees_Query>()
                        .Search(x => x.Query, "John Doe")
                        .SelectFields<Employee>()
                        .ToList();
                    #endregion
                }
            }
		}
	}
}

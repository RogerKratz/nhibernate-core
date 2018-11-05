﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using NHibernate.Cfg;
using NHibernate.Dialect;
using NUnit.Framework;
using Environment = NHibernate.Cfg.Environment;

namespace NHibernate.Test.DriverTest
{
	using System.Threading.Tasks;

	[TestFixture]
	public class Sql2008DateTime2TestAsync : TestCase
	{
		protected override void Configure(Configuration configuration)
		{
			configuration.SetProperty(Environment.PrepareSql, "true");
		}
		protected override string MappingsAssembly
		{
			get { return "NHibernate.Test"; }
		}

		protected override string[] Mappings
		{
			get { return new[] { "DriverTest.EntityForMs2008.hbm.xml" }; }
		}

		protected override bool AppliesTo(Dialect.Dialect dialect)
		{
			return dialect is MsSql2008Dialect;
		}

		[Test]
		public async Task CrudAsync()
		{
			var expectedMoment = new DateTime(1848, 6, 1, 12, 00, 00, 123);
			var expectedLapse = new TimeSpan((DateTime.Now - expectedMoment).Ticks);
			object savedId;
			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				savedId = await (s.SaveAsync(new EntityForMs2008
									{
										DateTimeProp = expectedMoment,
														TimeSpanProp = expectedLapse,
													}));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				var m = await (s.GetAsync<EntityForMs2008>(savedId));
				Assert.That(m.DateTimeProp, Is.EqualTo(expectedMoment));
				Assert.That(m.TimeSpanProp, Is.EqualTo(expectedLapse));
				await (t.CommitAsync());
			}

			using (ISession s = OpenSession())
			using (ITransaction t = s.BeginTransaction())
			{
				await (s.CreateQuery("delete from EntityForMs2008").ExecuteUpdateAsync());
				await (t.CommitAsync());
			}
		}

	}
}

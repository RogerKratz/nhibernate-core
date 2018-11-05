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
using System.Data;
using System.Data.Common;
using NHibernate.Transaction;
using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest
{
	using System.Threading.Tasks;
	using System.Threading;
	/// <summary>
	/// This fixture contains no mappings, it is thus faster and can be used
	/// to run tests for basic features that don't require any mapping files
	/// to function.
	/// </summary>
	[TestFixture]
	public class EmptyMappingsFixtureAsync : TestCase
	{
		protected override string[] Mappings
		{
			get { return Array.Empty<string>(); }
		}

		[Test]
		public async Task InvalidQueryAsync()
		{
			try
			{
				using (ISession s = OpenSession())
				{
					await (s.CreateQuery("from SomeInvalidClass").ListAsync());
				}
			}
			catch (QueryException)
			{
				//
			}
		}

		[Test]
		public async Task DisconnectShouldNotCloseUserSuppliedConnectionAsync()
		{
			var conn = await (Sfi.ConnectionProvider.GetConnectionAsync(CancellationToken.None));
			try
			{
				using (ISession s = OpenSession())
				{
					s.Disconnect();
					s.Reconnect(conn);
					Assert.AreSame(conn, s.Disconnect());
					Assert.AreEqual(ConnectionState.Open, conn.State);
				}
			}
			finally
			{
				Sfi.ConnectionProvider.CloseConnection(conn);
			}
		}
	}
}
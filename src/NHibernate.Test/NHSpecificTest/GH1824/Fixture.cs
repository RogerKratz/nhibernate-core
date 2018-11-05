using NUnit.Framework;

namespace NHibernate.Test.NHSpecificTest.GH1824
{
	[TestFixture]
	public class Fixture : BugTestCase
	{
		private object _fooId;
		private object _barId;

		protected override void OnSetUp()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				var bar = new Bar
				{
					MyProps =
					{
						["DynValString2"] = "a"
					},
					StaticProps = new StaticBarComponent
					{
						StaticValString2 = "b"
					}
				};
				_barId = session.Save(bar);
				_fooId = session.Save(
					new Foo
					{
						MyProps =
						{
							["DynPointer"] = bar
						},
						StaticProps = new StaticFooComponent
						{
							StaticPointer = bar
						}
					});
				transaction.Commit();
			}
		}

		protected override void OnTearDown()
		{
			using (var session = OpenSession())
			using (var transaction = session.BeginTransaction())
			{
				// The HQL delete does all the job inside the database without loading the entities, but it does
				// not handle delete order for avoiding violating constraints if any. Use
				// session.Delete("from System.Object");
				// instead if in need of having NHbernate ordering the deletes, but this will cause
				// loading the entities in the session.
				session.CreateQuery("delete from System.Object").ExecuteUpdate();

				transaction.Commit();
			}
		}

		[Test]
		public void CanReadDynPointerFromFoo()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var foo = session.Get<Foo>(_fooId);
				var bar = foo.MyProps["DynPointer"];
				Assert.That(bar, Is.Not.Null);
				Assert.That(bar, Is.InstanceOf<Bar>());
				Assert.That(bar, Has.Property("Id").EqualTo(_barId));
			}
		}

		[Test]
		public void CanReadStaticPointerFromFoo()
		{
			using (var session = OpenSession())
			using (session.BeginTransaction())
			{
				var foo = session.Get<Foo>(_fooId);
				var bar = foo.StaticProps.StaticPointer;
				Assert.That(bar, Is.Not.Null);
				Assert.That(bar, Has.Property("Id").EqualTo(_barId));
			}
		}
	}
}

﻿using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using RethinkDb.Driver.Model;
using RethinkDb.Driver.Tests.Utils;

namespace RethinkDb.Driver.Tests.ReQL
{
    public class Basket
    {
        public int id { get; set; }
        public List<string> Items { get; set; }
        public List<Revision> Revisions { get; set; }
        public int[][] ArrayOfInts { get; set; }
    }

    public class Revision
    {
        public byte[] Bytes { get; set; }
    }

    [TestFixture]
    public class GitHubIssues : QueryTestFixture
    {
        [Test]
        public void issue_12()
        {
            var table = r.db(DbName).table(TableName);
            table.delete().run(conn);

            var basket = new Basket {id = 99};

            table.insert(basket).run(conn);

            basket.Items = new List<string>
                {
                    "Apple",
                    "Orange",
                    "Kiwi"
                };

            basket.Revisions = new List<Revision>
                {
                    new Revision {Bytes = new byte[] {1, 2, 3}}
                };

            basket.ArrayOfInts = new[] {new[] {1, 2, 3}, new[] {4, 5, 6}};

            table.update(basket).run(conn);

            Basket fromDb = table.get(99).run<Basket>(conn);

            fromDb.Dump();

            fromDb.id.Should().Be(99);
            fromDb.Items.Should().Equal("Apple", "Orange", "Kiwi");
            fromDb.Revisions.ShouldBeEquivalentTo(basket.Revisions);
            fromDb.ArrayOfInts.ShouldBeEquivalentTo(new[] { new[] {1, 2, 3}, new[] {4, 5, 6}});
        }

        [Test]
        public void issue_20()
        {
            var table = r.db(DbName).table(TableName);
            table.delete().run(conn);

            Console.WriteLine("INSERT");
            var result = table.insert(new {foo = "bar"}).runResult(conn);
            var id = result.GeneratedKeys[0];
            result.AssertInserted(1);

            Console.WriteLine("UPDATE 1 / VALUE 1");
            var value = "VALUE1";
            result = table.get(id).update(new {Target = value}).runResult(conn);
            result.Dump();

            Console.WriteLine("UPDATE 2 / VALUE 2");
            value = "VALUE2";
            var optResult = table.get(id).update(new {Target = value})
                .optArg("return_changes", true).run(conn);
            ExtensionsForTesting.Dump(optResult);
        }
    }

}
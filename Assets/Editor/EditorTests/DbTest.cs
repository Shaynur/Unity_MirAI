using System.Collections.Generic;
using System.IO;
using Assets.MirAI.DB;
using Assets.MirAI.Models;
using NUnit.Framework;
using UnityEngine;

namespace Editor {

    public class DbTest {

        private string _dbFileName = "test.db";
        private DbContext _db;

        [SetUp]
        public void Setup() {
            _db = new DbContext(_dbFileName);
        }

        [Test]
        public void FullDbTest() {
            RecreateDb();
            AddProgram();
            UpdateEntities();
            RemoveProgram();
        }

        public void RecreateDb() {
            _db.Dispose();
            var fullName = Path.Combine(Application.dataPath, "DB", _dbFileName);
            File.Delete(fullName);
            Assert.IsFalse(File.Exists(fullName), "Delete dbFile");
            _db = new DbContext(_dbFileName);
            Assert.IsTrue(File.Exists(fullName), "Create new dbFile");
        }

        public void AddProgram() {
            var program = new Program { Name = "test" };
            var res1 = _db.Programs.Add(program);

            Node node1 = new Node {
                ProgramId = program.Id,
                Type = NodeType.Root
            };
            var res2 = _db.Nodes.Add(node1);
            program.Nodes.Add(node1);

            Node node2 = new Node {
                ProgramId = program.Id,
                Type = NodeType.Action
            };
            var res3 = _db.Nodes.Add(node2);
            program.Nodes.Add(node2);

            Link link = new Link(node1, node2);
            var res4 = _db.Links.Add(link);

            Assert.AreEqual(1, res1);
            Assert.AreEqual(1, res2);
            Assert.AreEqual(1, res3);
            Assert.AreEqual(1, res4, 1);
            Assert.AreEqual(1, program.Id);
            Assert.AreEqual(1, node1.Id);
            Assert.AreEqual(2, node2.Id);
            Assert.AreEqual(2, program.Nodes.Count);
        }

        public void UpdateEntities() {
            Program program = _db.Programs.GetById(1);
            Assert.AreNotEqual(null, program, "Get program with id=1");
            program.Name = "2_test_2";
            var res1 = _db.Programs.Update(program);
            Assert.AreEqual(1, res1, "Update program result");
            program = _db.Programs.GetById(1);
            Assert.AreNotEqual(null, program, "Get program with id=1");
            Assert.AreEqual("2_test_2", program.Name, "Update program name");

            Node node = _db.Nodes.GetById(1);
            Assert.AreNotEqual(null, node, "Get node with id=1");
            node.Type = NodeType.Condition;
            node.Command = 777;
            node.X = 100;
            node.Y = 100;
            res1 = _db.Nodes.Update(node);
            Assert.AreEqual(1, res1, "Update node result");
            node = _db.Nodes.GetById(1);
            Assert.AreNotEqual(null, node, "Get node with id=1");
            Assert.AreEqual(NodeType.Condition, node.Type, "Update node.Type");
            Assert.AreEqual(777, node.Command, "Update node.Command");
            Assert.AreEqual(100, node.X, "Update node.X");
            Assert.AreEqual(100, node.Y, "Update node.Y");
        }

        public void RemoveProgram() {
            Program program = _db.Programs.GetById(1);
            Assert.AreNotEqual(null, program, "Get program with id=1");
            var res1 = _db.Programs.Remove(program);
            Assert.AreEqual(1, res1, "Remove program result");
            program = _db.Programs.GetById(1);
            Assert.AreEqual(null, program, "Remove program");

            Assert.AreEqual(null, _db.Nodes.GetById(1), "Cascade remove node1 after remove program");
            Assert.AreEqual(null, _db.Nodes.GetById(2), "Cascade remove node2 after remove program");

            List<Link> links = _db.Links.ToList();
            Assert.AreEqual(0, links.Count, "Cascade remove links after remove program");
        }

        [TearDown]
        public void TearDown() {
            _db.Dispose();
        }
    }
}
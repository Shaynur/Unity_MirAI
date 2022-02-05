﻿using System.Collections.Generic;
using System.Linq;
using Assets.MirAI.DB;

namespace Assets.MirAI.Models {

    public class AiModel {

        public List<Program> Programs { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Link> Links { get; set; }

        public AiModel() { 
            LoadFromDB();
        }

        public void LoadFromDB() {
            using var db = new DbContext();
            LoadFromDB(db);
        }

        private void LoadFromDB(DbContext db) {
            Programs = db.Programs.ToList().ToList<Program>();
            Nodes = db.Nodes.ToList().ToList<Node>();
            Links = db.Links.ToList().ToList<Link>();
            CreateModelFromDbData();
        }

        public Program AddNewProgram(string name) {
            if (Programs.Find(p => p.Name == name) != null) return null;

            using var db = new DbContext();

            var program = new DbProgram { Name = name };
            db.Programs.Add(program);
            Programs.Add(program);

            var node = AddNode(db);
            program.Nodes.Add(node);

            node.ProgramId = program.Id;
            node.Type = NodeType.Root;
            db.Nodes.Update((DbNode)node);

            return program;
        }

        public void UpdateProgram(Program program) {
            using var db = new DbContext();
            db.Programs.Update((DbProgram)program);
        }

        public void RemoveProgram(int id) {
            using var db = new DbContext();
            db.Programs.Remove(id);
            LoadFromDB(db);
        }

        public Node AddChildNode(Node parent) {
            using var db = new DbContext();
            var child = AddNode(db);
            parent.AddChild(child);
            AddLink(parent.Id, child.Id, db);
            return child;
        }

        private Node AddNode(DbContext db) {
            var node = new DbNode();
            db.Nodes.Add(node);
            Nodes.Add(node);
            return node;
        }

        public void UpdateNode(Node node) {
            using var db = new DbContext();
            db.Nodes.Update((DbNode)node);
        }

        public void RemoveNode(int id) {
            using var db = new DbContext();
            db.Nodes.Remove(id);
            LoadFromDB(db);
        }

        public void AddLink(int parentNodeId, int childNodeId) {
            using var db = new DbContext();
            AddLink(parentNodeId, childNodeId, db);
        }

        private void AddLink(int parentNodeId, int childNodeId, DbContext db) {
            var link = new DbLink { FromId = parentNodeId, ToId = childNodeId };
            db.Links.Add(link);
            Links.Add(link);
        }

        public void RemoveLink(int fromId, int toId) {
            using var db = new DbContext();
            var link = Links.First(x => x.FromId == fromId && x.ToId == toId);
            db.Links.Remove((DbLink)link);
            Links.Remove(link);
        }

        private void CreateModelFromDbData() {
            CreateNodesLinks();
            CreateProgramsLinks();
        }

        private void CreateProgramsLinks() {
            foreach (var node in Nodes) {
                var progId = node.ProgramId;
                var prog = Programs.First(p => p.Id == progId);
                prog.Nodes.Add(node);
            }
        }

        private void CreateNodesLinks() {
            foreach (var link in Links) {
                var from = link.FromId;
                var to = link.ToId;
                var nodeFrom = Nodes.First(n => n.Id == from);
                var nodeTo = Nodes.First(n => n.Id == to);
                nodeFrom.AddChild(nodeTo);
            }
        }
    }
}
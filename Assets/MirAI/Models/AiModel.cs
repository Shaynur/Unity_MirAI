using System.Collections.Generic;
using System.Linq;
using Assets.MirAI.DB;
using UnityEngine.Events;

namespace Assets.MirAI.Models {

    public class AiModel {

        public List<Program> Programs { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Link> Links { get; set; }

        public UnityEvent ProgramsChanged = new UnityEvent();
        public UnityEvent NodesChanged = new UnityEvent();

        public Program CurrentProgram;

        public AiModel() {
            LoadFromDB();
        }

        public void LoadFromDB() {
            using var db = new DbContext();
            LoadFromDB(db);
        }

        private void LoadFromDB(DbContext db) {
            Programs = db.Programs.ToList();
            Nodes = db.Nodes.ToList();
            Links = db.Links.ToList();
            CreateModelFromDbData();
            if(Programs != null && Programs.Count > 0)
                CurrentProgram = Programs[0];
            else
                CurrentProgram = null;
            ProgramsChanged?.Invoke();
            NodesChanged?.Invoke();
        }

        public Program AddNewProgram(string name) {
            if (Programs.Find(p => p.Name == name) != null) return null;

            using var db = new DbContext();

            var program = new Program { Name = name };
            db.Programs.Add(program);
            Programs.Add(program);

            var node = AddNode(db);
            program.Nodes.Add(node);

            node.ProgramId = program.Id;
            node.Type = NodeType.Root;
            db.Nodes.Update(node);

            ProgramsChanged?.Invoke();
            return program;
        }

        public void UpdateProgram(Program program) {
            using var db = new DbContext();
            db.Programs.Update(program);
            ProgramsChanged?.Invoke();
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
            NodesChanged?.Invoke();
            return child;
        }

        private Node AddNode(DbContext db) {
            var node = new Node();
            db.Nodes.Add(node);
            Nodes.Add(node);
            return node;
        }

        public void UpdateNode(Node node) {
            using var db = new DbContext();
            db.Nodes.Update(node);
            NodesChanged?.Invoke();
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
            var link = new Link { FromId = parentNodeId, ToId = childNodeId };
            db.Links.Add(link);
            Links.Add(link);
        }

        public void RemoveLink(int fromId, int toId) {
            using var db = new DbContext();
            var link = Links.First(x => x.FromId == fromId && x.ToId == toId);
            db.Links.Remove(link);
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
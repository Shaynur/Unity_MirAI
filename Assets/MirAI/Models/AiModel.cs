using System.Collections.Generic;
using System.Linq;
using Assets.MirAI.DB;
using Assets.MirAI.Simulation;
using UnityEngine.Events;

namespace Assets.MirAI.Models {

    public class AiModel {

        public List<Program> Programs { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Link> Links { get; set; }
        public List<Unit> Units { get; set; }

        public UnityEvent OnLoaded { get; set; } = new UnityEvent();

        private static readonly AiModel _instance = new AiModel();
        public static AiModel Instance => _instance;
        private AiModel() {
            LoadFromDB();
        }

        public void LoadFromDB() {
            using var db = new DbContext();
            LoadFromDB(db);
            OnLoaded?.Invoke();
        }

        private void LoadFromDB(DbContext db) {
            Programs = db.Programs.ToList();
            Nodes = db.Nodes.ToList();
            Links = db.Links.ToList();
            Units = db.Units.ToList();
            BuildModelFromDbData();
        }

        public void AddNewProgram(string name) {
            if (Programs.Find(p => p.Name == name) != null) return;

            using var db = new DbContext();
            var program = new Program { Name = name };
            db.Programs.Add(program);
            Programs.Add(program);

            Node node = new Node {
                ProgramId = program.Id,
                Type = NodeType.Root
            };
            AddNode(node, db);
            program.Nodes.Add(node);
            LoadFromDB();
        }

        public void UpdateProgram(Program program) {
            using var db = new DbContext();
            db.Programs.Update(program);
        }

        public void RemoveProgram(Program program) {
            using var db = new DbContext();
            db.Programs.Remove(program);
            LoadFromDB();
        }

        public bool AddLinkAndChildNode(Link link) {
            var parent = link.NodeFrom;
            var child = link.NodeTo;
            if (!IsCanBeLinked(parent, child))
                return false;
            Programs.Find(x => x.Id == parent.ProgramId).Nodes.Add(child);
            parent.AddChild(link, child);
            child.ProgramId = parent.ProgramId;

            using var db = new DbContext();
            AddNode(child, db);
            link.ToId = child.Id;
            AddLink(link, db);
            return true;
        }

        public bool IsCanBeLinked(Node parent, Node child) {
            if (parent == null || child == null || parent == child) return false;
            if (Links.Exists(x => x.NodeFrom == parent && x.NodeTo == child)) return false;
            switch (parent.Type) {
                case NodeType.Nope:
                case NodeType.Action:
                case NodeType.SubAI:
                    return false;
                default:
                    return child.Type != NodeType.Nope && child.Type != NodeType.Root;
            }
        }

        public void AddNodes(Node[] nodes) {
            using var db = new DbContext();
            foreach (var node in nodes)
                AddNode(node, db);
            LoadFromDB();
        }

        private void AddNode(Node node, DbContext db) {
            db.Nodes.Add(node);
            Nodes.Add(node);
        }

        public void UpdateNode(Node node) {
            using var db = new DbContext();
            UpdateNode(node, db);
            LoadFromDB();
        }

        public void UpdateNodes(Node[] nodes) {
            using var db = new DbContext();
            foreach (var node in nodes)
                UpdateNode(node, db);
        }

        private void UpdateNode(Node node, DbContext db) {
            db.Nodes.Update(node);
        }

        public void RemoveNodes(Node[] nodes) {
            using var db = new DbContext();
            foreach (var node in nodes)
                db.Nodes.Remove(node);
            db.Dispose();
            LoadFromDB();
        }

        public bool AddNewLink(Node parentNode, Node childNode) {
            if (!IsCanBeLinked(parentNode, childNode))
                return false;
            using var db = new DbContext();
            AddNewLink(parentNode, childNode, db);
            return true;
        }

        public bool AddLink(Link link) {
            if (!IsCanBeLinked(link.NodeFrom, link.NodeTo))
                return false;
            using var db = new DbContext();
            AddLink(link, db);
            return true;
        }

        public void AddLinks(Link[] links) {
            using var db = new DbContext();
            foreach (var link in links)
                AddLink(link, db);
        }

        private void AddNewLink(Node parentNode, Node childNode, DbContext db) {
            var link = new Link(parentNode, childNode);
            AddLink(link, db);
        }

        private void AddLink(Link link, DbContext db) {
            db.Links.Add(link);
            Links.Add(link);
        }

        public void RemoveLink(Link link) {
            using var db = new DbContext();
            db.Links.Remove(link);
            Links.Remove(link);
        }

        private void BuildModelFromDbData() {
            CreateNodesLinks();
            CreateProgramsNodeLists();
            SortProgramNodesByAngle();
        }

        private void SortProgramNodesByAngle() {
            foreach (var program in Programs)
                program.SortNodesByAngle();
        }

        private void CreateProgramsNodeLists() {
            foreach (var node in Nodes) {
                var progId = node.ProgramId;
                var prog = Programs.First(p => p.Id == progId);
                prog.Nodes.Add(node);
            }
        }

        private void CreateNodesLinks() {
            foreach (var link in Links) {
                FillLinkNodes(link);
                link.NodeFrom.AddChild(link, link.NodeTo);
            }
        }

        private void FillLinkNodes(Link link) {
            var nodeFrom = Nodes.First(n => n.Id == link.FromId);
            var nodeTo = Nodes.First(n => n.Id == link.ToId);
            link.NodeFrom = nodeFrom;
            link.NodeTo = nodeTo;
        }
    }
}
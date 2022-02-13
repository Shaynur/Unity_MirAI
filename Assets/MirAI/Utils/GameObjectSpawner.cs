using UnityEngine;

namespace Assets.MirAI.Utils {

    public static class GameObjectSpawner {

        private const string ContainerName = "###SPAWNED###";

        public static GameObject Spawn(GameObject prefab, Vector3 position, string containerName = ContainerName) {
            var container = GetContainer(containerName);
            return GameObject.Instantiate(prefab, position, Quaternion.identity, container.transform);
        }

        public static GameObject Spawn(GameObject prefab, string containerName = ContainerName) {
            var container = GetContainer(containerName);
            return GameObject.Instantiate(prefab, container.transform);
        }

        public static GameObject GetContainer(string containerName) {
            var container = GameObject.Find(containerName);
            return container != null ? container : new GameObject(containerName);
        }
    }
}
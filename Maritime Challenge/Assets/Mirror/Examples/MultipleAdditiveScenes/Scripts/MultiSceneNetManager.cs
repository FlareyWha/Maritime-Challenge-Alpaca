using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
	Documentation: https://mirror-networking.gitbook.io/docs/components/network-manager
	API Reference: https://mirror-networking.com/docs/api/Mirror.NetworkManager.html
*/

namespace Mirror.Examples.MultipleAdditiveScenes
{
    [AddComponentMenu("")]
    public class MultiSceneNetManager : NetworkManager
    {
        //[Header("Spawner Setup")]
        //[Tooltip("Reward Prefab for the Spawner")]
        //public GameObject rewardPrefab;

        [Header("MultiScene Setup")]
        //public int instances = 3;

        [SerializeField]
        private string gameScene;
        [SerializeField]
        private string[] subScenesList;

        // This is set true after server loads all subscene instances
        bool subscenesLoaded;

        // subscenes are added to this list as they're loaded
        readonly List<Scene> subScenes = new List<Scene>();

        // Sequential index used in round-robin deployment of players into instances and score positioning
        int clientIndex;

        #region Server System Callbacks

        /// <summary>
        /// Called on the server when a client adds a new player with NetworkClient.AddPlayer.
        /// <para>The default implementation for this function creates a new player object from the playerPrefab.</para>
        /// </summary>
        /// <param name="conn">Connection from client.</param>
        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            StartCoroutine(OnServerAddPlayerDelayed(conn));
        }

        // This delay is mostly for the host player that loads too fast for the
        // server to have subscenes async loaded from OnStartServer ahead of it.
        IEnumerator OnServerAddPlayerDelayed(NetworkConnectionToClient conn)
        {
            // wait for server to async load all subscenes for game instances
            while (!subscenesLoaded)
                yield return null;

            // Send Scene message to client to additively load the game scene
            //conn.Send(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.LoadAdditive });

            // Wait for end of frame before adding the player to ensure Scene Message goes first
            yield return new WaitForEndOfFrame();

            base.OnServerAddPlayer(conn);

            // Do this only on server, not on clients
            // This is what allows the NetworkSceneChecker on player and scene objects
            // to isolate matches per scene instance on server.
            //if (subScenes.Count > 0)
            //{
            //    Scene startScene = SceneManager.GetSceneByName(subScenesList[0]);
            //    SceneManager.MoveGameObjectToScene(conn.identity.gameObject, SceneManager.GetSceneByName(subScenesList[0]));
            //}

            clientIndex++;
        }

        #endregion

        #region Start & Stop Callbacks

        /// <summary>
        /// This is invoked when a server is started - including when a host is started.
        /// <para>StartServer has multiple signatures, but they all cause this hook to be called.</para>
        /// </summary>
        public override void OnStartServer()
        {
            StartCoroutine(ServerLoadSubScenes());
        }

        // We're additively loading scenes, so GetSceneAt(0) will return the main "container" scene,
        // therefore we start the index at one and loop through instances value inclusively.
        // If instances is zero, the loop is bypassed entirely.
        IEnumerator ServerLoadSubScenes()
        {
            while (SceneManager.GetActiveScene().name != gameScene)
                yield return null;

            for (int index = 0; index < subScenesList.Length; index++)
            {
                yield return SceneManager.LoadSceneAsync(subScenesList[index], new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics2D });
                Debug.Log("Loaded Async Sub Scene " + subScenesList[index]);
                Scene newScene = SceneManager.GetSceneByName(subScenesList[index]);
                subScenes.Add(newScene);
            }
            subscenesLoaded = true;
            NetworkServer.SpawnObjects();
            Debug.Log("Spawned Network GOs Count: " + NetworkServer.spawned.Count);
            foreach (KeyValuePair<uint, NetworkIdentity> spawned in NetworkServer.spawned)
            {
                Debug.Log("Spawned: " + spawned.Value.gameObject.name);
            }
        }

        /// <summary>
        /// This is called when a server is stopped - including when a host is stopped.
        /// </summary>
        public override void OnStopServer()
        {
            NetworkServer.SendToAll(new SceneMessage { sceneName = gameScene, sceneOperation = SceneOperation.UnloadAdditive });
            StartCoroutine(ServerUnloadSubScenes());
            clientIndex = 0;
        }

        // Unload the subScenes and unused assets and clear the subScenes list.
        IEnumerator ServerUnloadSubScenes()
        {
            for (int index = 0; index < subScenes.Count; index++)
                yield return SceneManager.UnloadSceneAsync(subScenes[index]);

            subScenes.Clear();
            subscenesLoaded = false;

            yield return Resources.UnloadUnusedAssets();
        }

        /// <summary>
        /// This is called when a client is stopped.
        /// </summary>
        public override void OnStopClient()
        {
            // make sure we're not in host mode
            if (mode == NetworkManagerMode.ClientOnly)
                StartCoroutine(ClientUnloadSubScenes());
        }

        // Unload all but the active scene, which is the "container" scene
        IEnumerator ClientUnloadSubScenes()
        {
            for (int index = 0; index < SceneManager.sceneCount; index++)
            {
                if (SceneManager.GetSceneAt(index) != SceneManager.GetActiveScene())
                    yield return SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(index));
            }
        }

        #endregion
    }
}

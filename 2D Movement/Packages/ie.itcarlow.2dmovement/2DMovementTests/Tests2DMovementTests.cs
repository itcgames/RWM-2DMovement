using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;

namespace Tests
{
    public class Movement
    {
        private GameObject player;

        [SetUp]
        public void Setup()
        {
            SceneManager.LoadScene("TestScene", LoadSceneMode.Single);
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(player);
            SceneManager.UnloadSceneAsync("TestScene");
        }

        [UnityTest]
        public IEnumerator HorizontalMovement()
        {
            player = GameObject.Find("Player");
            Vector3 position = player.GetComponent<Rigidbody2D>().transform.position;
            player.GetComponent<Runtime2DMovement>().moveRight();
            yield return new WaitForSeconds(1.0f);
            Assert.Greater(player.GetComponent<Rigidbody2D>().position.x, position.x);

            position = player.GetComponent<Rigidbody2D>().position;
            player.GetComponent<Runtime2DMovement>().moveLeft();
            yield return new WaitForSeconds(1.0f);
            Assert.Less(player.GetComponent<Rigidbody2D>().position.x, position.x);
        }
    }
}